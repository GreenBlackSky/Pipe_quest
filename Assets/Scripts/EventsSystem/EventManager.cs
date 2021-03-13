using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;


public class EventManager {

    static Dictionary<int, EventListener> _allListeners;
    static Dictionary<(string, string), List<EventListener>> _activeTriggers;
    static LinkedList<EventListener> _eventsQueue;

    public static void Init(string level_name, QuestDoingHero questHero, CollectingHero itemsHero) {
        if(_allListeners == null) {
            _allListeners = new Dictionary<int, EventListener>();
            _activeTriggers = new Dictionary<(string, string), List<EventListener>>();
            _eventsQueue = new LinkedList<EventListener>();
        }
        BaseEventTrigger.Init();
        BaseEventCondition.Init(questHero);
        BaseEventValueProvider.Init(itemsHero);
        BaseEventCallback.Init(questHero);
        LoadAllListeners(level_name);
    }

    private void Update() {
        List<int> listenersToAdd = new List<int>();
        List<int> listenersToRemove = new List<int>();
        List<BaseEventCallback> callbacksToCall = new List<BaseEventCallback>();

        if(_eventsQueue.Count != 0) {
            EventListener listener = _eventsQueue.First.Value;
            _eventsQueue.RemoveFirst();

            if(!listener.CheckConditions()) {
                return;
            }

            foreach(BaseEventCallback callback in listener.callbacks) {
                if(callback is StopListenerCallback) {
                    int listenerID = ((StopListenerCallback)callback).getIntArg();
                    listenersToRemove.Add(listenerID);
                } else if (callback is StartListenerCallback) {
                    int listenerID = ((StartListenerCallback)callback).getIntArg();
                    listenersToAdd.Add(listenerID);
                } else {
                    callbacksToCall.Add(callback);
                }
            }

            foreach(int listenerID in listenersToRemove) {
                StopListening(listenerID);
            }

            foreach(int listenerID in listenersToAdd) {
                StartListening(listenerID);
            }

            foreach(BaseEventCallback callback in callbacksToCall) {
                 callback.Call();
            }

            if(listener.singleuse) {
                StopListening(listener.id);
            }
        }
    }

    public static void LoadAllListeners(string level_name) {
        ClearListeners();
        string path = @"" + "Assets/EventsData/" + level_name + ".xml";
        if(!System.IO.File.Exists(path)) {
            return;
        }
        XmlSerializer listenerSerializer = new XmlSerializer(typeof(EventListener));
        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        XmlNode root = doc.DocumentElement.SelectSingleNode("/level");
        foreach(XmlNode listenerData in root.ChildNodes) {
            EventListener listener = listenerSerializer.Deserialize(new XmlNodeReader(listenerData)) as EventListener;
            _allListeners[listener.id] = listener;
            if(listener.initial) {
                StartListening(listener.id);
            }
        }
    }

    public static void ClearListeners() {
        foreach(List<EventListener> listeners in _activeTriggers.Values) {
            listeners.Clear();
        }
        _activeTriggers.Clear();
        _allListeners.Clear();
    }

    public static void StartListening(int listenerId) {
        EventListener listener = _allListeners[listenerId];
        foreach(BaseEventTrigger trigger in listener.triggers) {
            (string, string) key = (trigger.GetType().Name, trigger.argument);
            if(!_activeTriggers.ContainsKey(key)) {
                _activeTriggers[key] = new List<EventListener>();
            }
            _activeTriggers[key].Add(listener);
        }
        Debug.Log(_activeTriggers.Count);
        foreach((string, string) key in _activeTriggers.Keys){
            Debug.Log(key);
        }
    }

    public static void StopListening(int listenerId) {
        EventListener listener = _allListeners[listenerId];
        foreach(BaseEventTrigger trigger in listener.triggers) {
            (string, string) key = (trigger.GetType().Name, trigger.argument);
            if(_activeTriggers.ContainsKey(key)) {
                _activeTriggers[key].Remove(listener);
            }
        }
    }

    public static void Trigger((string, string) triggerKey) {
        Debug.Log("EventManager.Trigger");
        if(!_activeTriggers.ContainsKey(triggerKey)) {
            return;
        }
        foreach(EventListener listener in _activeTriggers[triggerKey]) {
            _eventsQueue.AddLast(listener);
        }
    }
}
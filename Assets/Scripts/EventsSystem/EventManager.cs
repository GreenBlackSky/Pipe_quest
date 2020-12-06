using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;

public class EventManager {

    static Dictionary<int, EventListener> allListeners;
    static Dictionary<(string, string), List<EventListener>> activeTriggers;
    static LinkedList<EventListener> eventsQueue;

    public static void Init(QuestDoingHero questHero, CollectingHero itemsHero) {
        if(allListeners == null) {
            allListeners = new Dictionary<int, EventListener>();
            activeTriggers = new Dictionary<(string, string), List<EventListener>>();
            eventsQueue = new LinkedList<EventListener>();
        }
        BaseEventTrigger.Init();
        BaseEventCondition.Init(questHero);
        BaseEventValueProvider.Init(itemsHero);
        BaseEventCallback.Init(questHero);
    }

    private void Update() {
        List<int> listenersToAdd = new List<int>();
        List<int> listenersToRemove = new List<int>();
        List<BaseEventCallback> callbacksToCall = new List<BaseEventCallback>();

        if(eventsQueue.Count != 0) {
            EventListener listener = eventsQueue.First.Value;
            eventsQueue.RemoveFirst();

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
        // TODO load from xml
        string path = @"" + "Assets/EventsData/" + level_name + ".xml";
        XmlSerializer listenerSerializer = new XmlSerializer(typeof(EventListener));
        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        XmlNode root = doc.DocumentElement.SelectSingleNode("/level");
        foreach(XmlNode listenerData in root.ChildNodes) {
            EventListener listener = listenerSerializer.Deserialize(new XmlNodeReader(listenerData)) as EventListener;
            allListeners[listener.id] = listener;
            if(listener.initial) {
                StartListening(listener.id);
            }
        }
    }

    public static void ClearListeners() {
        foreach(List<EventListener> listeners in activeTriggers.Values) {
            listeners.Clear();
        }
        activeTriggers.Clear();
        allListeners.Clear();
    }

    public static void StartListening(int listenerId) {
        EventListener listener = allListeners[listenerId];
        foreach(BaseEventTrigger trigger in listener.triggers) {
            (string, string) key = (trigger.GetType().Name, trigger.argument);
            if(!activeTriggers.ContainsKey(key)) {
                activeTriggers[key] = new List<EventListener>();
            }
            activeTriggers[key].Add(listener);
        }
    }

    public static void StopListening(int listenerId) {
        EventListener listener = allListeners[listenerId];
        foreach(BaseEventTrigger trigger in listener.triggers) {
            (string, string) key = (trigger.GetType().Name, trigger.argument);
            if(activeTriggers.ContainsKey(key)) {
                activeTriggers[key].Remove(listener);
            }
        }
    }

    public static void Trigger((string, string) triggerKey) {
        if(!activeTriggers.ContainsKey(triggerKey)) {
            return;
        }
        foreach(EventListener listener in activeTriggers[triggerKey]) {
            eventsQueue.AddLast(listener);
        }
    }
}
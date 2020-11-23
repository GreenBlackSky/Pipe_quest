using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {

    static Dictionary<int, EventListener> allListeners;
    static Dictionary<(EventTriggerType, string), List<EventListener>> activeTriggers;

    static LinkedList<EventListener> eventsQueue;

    private void Start() {

        allListeners = new Dictionary<int, EventListener>();
        activeTriggers = new Dictionary<(EventTriggerType, string), List<EventListener>>();

        eventsQueue = new LinkedList<EventListener>();
    }

    public void Init(QuestDoingHero questHero, CollectingHero itemsHero) {
        EventCondition.Init(questHero);
        EventValueProvider.Init(itemsHero);
        EventCallback.Init();
    }

    private void Update() {
        List<EventListener> listenersToAdd = new List<EventListener>();
        List<EventListener> listenersToRemove = new List<EventListener>();
        List<EventCallback> callbacksToCall = new List<EventCallback>();

        if(eventsQueue.Count != 0) {
            EventListener listener = eventsQueue.First.Value;
            eventsQueue.RemoveFirst();

            if(!listener.CheckConditions()) {
                return;
            }

            foreach(EventCallback callback in listener.callbacks) {
                if(callback.type == CallbackType.stop_listener) {
                    int listener_id = int.Parse(callback.getArg("listener_id"));
                    EventListener listenerToRemove = allListeners[listener_id];
                    listenersToRemove.Add(listenerToRemove);
                } else if (callback.type == CallbackType.start_listener) {
                    int listener_id = int.Parse(callback.getArg("listener_id"));
                    EventListener listenerToAdd = allListeners[listener_id];
                    listenersToAdd.Add(listenerToAdd);
                } else {
                    callbacksToCall.Add(callback);
                }
            }

            foreach(EventListener listenerToRemove in listenersToRemove) {
                StopListening(listener.id);
            }

            foreach(EventListener listenerToAdd in listenersToAdd) {
                StartListening(listener.id);
            }

            foreach(EventCallback callback in callbacksToCall) {
                 callback.Call();
            }
        }
    }

    public static void LoadAllListeners(string level_name) {
        ClearListeners();

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
        foreach((EventTriggerType, string) trigger in listener.triggers) {
            if(!activeTriggers.ContainsKey(trigger)) {
                activeTriggers[trigger] = new List<EventListener>();
            }
            activeTriggers[trigger].Add(listener);
        }
    }

    public static void StopListening(int listenerId) {
        EventListener listener = allListeners[listenerId];
        foreach((EventTriggerType, string) trigger in listener.triggers) {
            if(activeTriggers.ContainsKey(trigger)) {
                activeTriggers[trigger].Remove(listener);
            }
        }
    }

    public static void TriggerEvent(EventTriggerType trigger, string triggerArg, int arg) {
        foreach(EventListener listener in activeTriggers[(trigger, triggerArg)]) {
            eventsQueue.AddLast(listener);
        }
    }
}
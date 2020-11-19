using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour {
    Dictionary<int, EventListener> allListeners;
    Dictionary<(EventTriggerType, string), List<EventListener>> activeListeners;

    LinkedList<(int, int)> eventsQueue;

    public static EventManager Instance { get; private set; }

    private void Start() {
        allListeners = new Dictionary<int, EventListener>();
        activeListeners = new Dictionary<(EventTriggerType, string), List<EventListener>>();
        eventsQueue = new LinkedList<(int, int)>();
        Instance = this;
    }

    private void Update() {
        // TODO check time of execution
        List<EventListener> listenersToAdd = new List<EventListener>();
        List<EventListener> listenersToRemove = new List<EventListener>();
        while(eventsQueue.Count != 0) {
            var (listenerID, arg) = eventsQueue.First.Value;
            eventsQueue.RemoveFirst();

            if(!allListeners.ContainsKey(listenerID)) {
                continue;
            }

            bool conditionsSet = true;
            EventListener listener = allListeners[listenerID];
            foreach(EventCondition condition in listener.conditions) {
                if(!condition.check()) {
                    conditionsSet = false;
                    break;
                }
            }

            if(!conditionsSet) {
                continue;
            }

            foreach(EventCallback callback in listener.callbacks) {
                if(callback.type == CallbackType.stop_listener) {

                } else if (callback.type == CallbackType.start_listener) {

                } else {
                    callback.call();
                }
            }
        }
    }

    public void LoadAllListeners(string level_name) {
        allListeners.Clear();
        activeListeners.Clear();
        string path = @"" + "Assets/EventsData/" + level_name + ".xml";
        XmlSerializer listenerSerializer = new XmlSerializer(typeof(EventListener));
        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        XmlNode root = doc.DocumentElement.SelectSingleNode("/level");
        foreach(XmlNode listenerData in root.ChildNodes) {
            EventListener listener = listenerSerializer.Deserialize(new XmlNodeReader(listenerData)) as EventListener;
            allListeners[listener.id] = listener;
            if(listener.isInitial) {
                StartListening(listener.id);
            }
        }
    }

    public static void StartListening(int listenerId) {
        EventListener listener = Instance.allListeners[listenerId];
        foreach((EventTriggerType, string) trigger in listener.triggers) {
            if(!Instance.activeListeners.ContainsKey(trigger)) {
                Instance.activeListeners[trigger] = new List<EventListener>();
            }
            Instance.activeListeners[trigger].Add(listener);
        }
    }

    public static void StopListening(int listenerId) {
        EventListener listener = Instance.allListeners[listenerId];
        
    }

    public static void TriggerEvent(EventTriggerType trigger, string arg) {
        // enque events
    }
}
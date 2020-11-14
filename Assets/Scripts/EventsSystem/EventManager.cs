using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;

public class EventManager : MonoBehaviour {
    // private Dictionary<string, UnityEvent> eventDictionary;
    private List<EventListener> listeners;

    public static EventManager Instance { get; private set; }

    private void Start() {
        // eventDictionary = new Dictionary<string, UnityEvent>();
        listeners = new List<EventListener>();
        Instance = this;
    }

    public void LoadAllListeners(string level_name) {
        listeners.Clear();
        string path = @"" + "Assets/EventsData/" + level_name + ".xml";
        XmlSerializer listenerSerializer = new XmlSerializer(typeof(EventListener));
        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        XmlNode root = doc.DocumentElement.SelectSingleNode("/level");
    }

    public static void StartListening(string eventName, UnityAction listener) {
        // if (Instance.eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent)) {
        //     thisEvent.AddListener(listener);
        // } else {
        //     thisEvent = new UnityEvent();
        //     thisEvent.AddListener(listener);
        //     Instance.eventDictionary.Add(eventName, thisEvent);
        // }
    }

    public static void StopListening(string eventName, UnityAction listener) {
        // if (Instance.eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent)) {
        //     thisEvent.RemoveListener(listener);
        // }
    }

    public static void TriggerEvent(string eventName) {
        // if (Instance.eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent)) {
        //     thisEvent.Invoke();
        // }
    }
}
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class EventManager : MonoBehaviour {
    private Dictionary<string, UnityEvent> eventDictionary;

    public static EventManager Instance { get; private set; }

    private void Start() {
        eventDictionary = new Dictionary<string, UnityEvent>();
        Instance = this;
    }

    public static void StartListening(string eventName, UnityAction listener) {
        if (Instance.eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent)) {
            thisEvent.AddListener(listener);
        } else {
            thisEvent = new UnityEvent();
            thisEvent.AddListener(listener);
            Instance.eventDictionary.Add(eventName, thisEvent);
        }
    }

    public static void StopListening(string eventName, UnityAction listener) {
        if (Instance.eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent)) {
            thisEvent.RemoveListener(listener);
        }
    }

    public static void TriggerEvent(string eventName) {
        if (Instance.eventDictionary.TryGetValue(eventName, out UnityEvent thisEvent)) {
            thisEvent.Invoke();
        }
    }
}
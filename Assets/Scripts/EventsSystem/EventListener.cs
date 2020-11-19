using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class EventListener {
    public int id;
    public bool isInitial;

    public List<(EventTriggerType, string)> triggers;
    public List<EventCondition> conditions;
    public List<EventCallback> callbacks;
}
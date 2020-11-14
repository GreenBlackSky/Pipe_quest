using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class EventListener {
    public int id;
    private List<EventTrigger> triggers;
    private List<EventCondition> conditions;
    private List<EventCallback> callbacks;
}
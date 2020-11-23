using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;

public class EventListener {
    public int id;
    public bool initial;
    public bool singleuse;

    public List<(EventTriggerType, string)> triggers;
    public List<EventCondition> conditions;
    public List<EventCallback> callbacks;

    public bool CheckConditions() {
        foreach(EventCondition condition in conditions) {
            if(!condition.Check()) {
                return false;
            }
        }
        return true;
    }
}
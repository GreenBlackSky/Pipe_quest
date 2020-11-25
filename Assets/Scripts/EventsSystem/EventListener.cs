using UnityEngine;
using System.Collections.Generic;

public class EventListener {
    public int id;
    public bool initial;
    public bool singleuse;

    public List<BaseEventTrigger> triggers;
    public List<BaseEventCondition> conditions;
    public List<BaseEventCallback> callbacks;

    public bool CheckConditions() {
        foreach(BaseEventCondition condition in conditions) {
            if(!condition.Check()) {
                return false;
            }
        }
        return true;
    }
}
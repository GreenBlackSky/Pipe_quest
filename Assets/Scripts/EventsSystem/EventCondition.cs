using System;
using System.Collections;
using System.Collections.Generic;

public enum EventConditionType {
    not,
    and,
    or,
    more_than,
    less_than,
    equal_to,
    flag_set,
    quest_completed,
}

public class EventCondition {
    public EventConditionType type;
    public List<EventCondition> innerConditions;
    public ConditionProvider provider;
    public int value;

    public EventCondition() {
        innerConditions = new List<EventCondition>();
    }

    public static void Init() {

    }

    public bool Check() {
        switch(type) {
            case EventConditionType.not:
                return !innerConditions[0].Check();
            case EventConditionType.and:
                foreach(EventCondition innerCondition in innerConditions) {
                    if(!innerCondition.Check()) {
                        return false;
                    }
                }
                return true;
            case EventConditionType.or:
                foreach(EventCondition innerCondition in innerConditions) {
                    if(innerCondition.Check()) {
                        return true;
                    }
                }
                return false;
            case EventConditionType.more_than:
                return provider.Provide() > value;
            case EventConditionType.less_than:
                return provider.Provide() < value;
            case EventConditionType.equal_to:
                return provider.Provide() == value;
            case EventConditionType.flag_set:

                break;
            case EventConditionType.quest_completed:

                break;
        }
        return true;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;

public enum EventConditionType {
    complex_condition,
    not,
    and,
    or,
    more_than,
    less_than,
    flag_set,
    quest_completed,
}

public class EventCondition {
    public EventConditionType type;
    public EventCondition innerCondition;
    public ConditionProvider provider;

    public EventCondition() {}

    public void Init() {

    }

    public bool check() {
        return true;
    }
}

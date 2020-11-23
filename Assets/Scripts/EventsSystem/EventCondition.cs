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
    public int intValue;
    public string strValue;
    static QuestDoingHero questHero;

    public EventCondition() {
        innerConditions = new List<EventCondition>();
    }

    public static void Init(QuestDoingHero hero) {
        questHero = hero;
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
                return provider.Provide() > intValue;
            case EventConditionType.less_than:
                return provider.Provide() < intValue;
            case EventConditionType.equal_to:
                return provider.Provide() == intValue;
            case EventConditionType.flag_set:
                return (questHero.flags.ContainsKey(strValue) && questHero.flags[strValue] > 0);
            case EventConditionType.quest_completed:
                return (questHero.completedQuests.ContainsKey(intValue));
        }
        return true;
    }
}

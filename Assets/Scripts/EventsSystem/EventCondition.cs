using System;
using System.Collections;
using System.Collections.Generic;

public class BaseEventCondition {
    protected static QuestDoingHero questHero;

    public static void Init(QuestDoingHero hero) {
        questHero = hero;
    }

    public virtual bool Check() {
        throw new Exception("not implementerd");
    }
}

public class EventConditionNot : BaseEventCondition {
    public BaseEventCondition innerCondition;

    public override bool Check() {
        return !innerCondition.Check();
    }
}

public class EventConditionAnd : BaseEventCondition {
    public List<BaseEventCondition> innerConditions;

    public EventConditionAnd() {
        innerConditions = new List<BaseEventCondition>();
    }

    public override bool Check() {
        foreach(BaseEventCondition innerCondition in innerConditions) {
            if(!innerCondition.Check()) {
                return false;
            }
        }
        return true;
    }
}

public class EventConditionOr : BaseEventCondition {
    public List<BaseEventCondition> innerConditions;

    public EventConditionOr() {
        innerConditions = new List<BaseEventCondition>();
    }

    public override bool Check() {
        foreach(BaseEventCondition innerCondition in innerConditions) {
            if(innerCondition.Check()) {
                return true;
            }
        }
        return false;
    }
}

public class EventConditionMoreThan : BaseEventCondition {
    public BaseEventValueProvider provider;
    public int intValue;

    public override bool Check() {
        return provider.Provide() > intValue;
    }
}

public class EventConditionLessThan : BaseEventCondition {
    public BaseEventValueProvider provider;
    public int intValue;

    public override bool Check() {
        return provider.Provide() < intValue;
    }
}

public class EventConditionEqualTo : BaseEventCondition {
    public BaseEventValueProvider provider;
    public int intValue;

    public override bool Check() {
        return provider.Provide() == intValue;
    }
}

public class EventConditionFlagSet : BaseEventCondition {
    public string strValue;

    public override bool Check() {
        return (questHero.flags.ContainsKey(strValue) && questHero.flags[strValue] > 0);
    }
}

public class EventConditionQuestCompleted : BaseEventCondition {
    public int intValue;

    public override bool Check() {
        return questHero.completedQuests.ContainsKey(intValue);
    }
}
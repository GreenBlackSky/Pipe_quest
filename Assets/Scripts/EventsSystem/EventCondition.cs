using System;
using System.Collections.Generic;
using UnityEngine;


public class BaseEventCondition {
    protected static QuestDoingHero questHero;

    public static void Init(QuestDoingHero hero) {
        questHero = hero;
    }

    public virtual bool Check() {
        throw new Exception("not implementerd");
    }

    public void devPing() {
        Debug.Log($"PING CONDITION {this.GetType()}");
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
    public int intArg;

    public override bool Check() {
        return provider.Provide() > intArg;
    }
}

public class EventConditionLessThan : BaseEventCondition {
    public BaseEventValueProvider provider;
    public int intArg;

    public override bool Check() {
        return provider.Provide() < intArg;
    }
}

public class EventConditionEqualTo : BaseEventCondition {
    public BaseEventValueProvider provider;
    public int intArg;

    public override bool Check() {
        return provider.Provide() == intArg;
    }
}
public class EventConditionFlagSet : BaseEventCondition {
    public string strArg;

    public override bool Check() {
        return (questHero.flags.ContainsKey(strArg) && questHero.flags[strArg] > 0);
    }
}

public class EventConditionQuestCompleted : BaseEventCondition {
    public int intArg;

    public override bool Check() {
        return questHero.completedQuests.ContainsKey(intArg);
    }
}
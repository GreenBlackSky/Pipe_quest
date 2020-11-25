using System;

// TODO static trigger
public class BaseEventTrigger {
    static EventManager eventManager;
    
    public static void Init(EventManager manager) {
        eventManager = manager;
    }

    public virtual void Trigger() {
        throw new Exception("not implementerd");
    }
}

public class LostItemTrigger : BaseEventTrigger {}
public class CollectItemTrigger : BaseEventTrigger {}
public class EquipItemTrigger : BaseEventTrigger {}

public class DialogueReplyTrigger : BaseEventTrigger {}

public class ArriveAtTrigger : BaseEventTrigger {}

public class EnterBattleTrigger : BaseEventTrigger {}
public class LeaveBattleTrigger : BaseEventTrigger {}

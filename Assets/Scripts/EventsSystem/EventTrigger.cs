using System;
using UnityEngine;

public class BaseEventTrigger {
    static EventManager eventManager;
    public string argument;

    public static void Init() {
    }

    public void Trigger(string argument) {
        string triggerName = this.GetType().Name;
        Debug.Log($"BaseEventTrigger.Trigger {triggerName} {argument}");
        if(triggerName == "BaseEventTrigger") {
            throw new Exception("not implementerd");
        }
        EventManager.Trigger((triggerName, argument));
    }

    public void devPing() {
        Debug.Log($"PING TRIGGER {this.GetType()}");
    }
}


public class ReachMarkerTrigger : BaseEventTrigger {}
public class LostItemTrigger : BaseEventTrigger {}
public class CollectItemTrigger : BaseEventTrigger {}
public class EquipItemTrigger : BaseEventTrigger {}
public class DialogueReplyTrigger : BaseEventTrigger {}
public class ArriveAtTrigger : BaseEventTrigger {}
public class EnterBattleTrigger : BaseEventTrigger {}
public class LeaveBattleTrigger : BaseEventTrigger {}
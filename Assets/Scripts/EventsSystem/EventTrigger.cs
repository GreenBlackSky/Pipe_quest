using System;
using System.Collections;
using System.Collections.Generic;

public enum EventTriggerType{
    lost_item,
    collect_item,
    dialogue_reply,
    arrive_at,
    equip_item,
    enter_battle,
    leave_battle,
}

public class EventTrigger {
    public EventTriggerType type;
    public string agr;

    public EventTrigger() {}

}

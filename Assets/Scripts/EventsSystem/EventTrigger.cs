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


public class  EventTrigger {
    static Dictionary<(EventTriggerType, string), EventTrigger> triggers;
    
    public EventTriggerType type;
    public string arg;

    public EventTrigger() {}

    public EventTrigger(EventTriggerType type, string arg) {
        this.type = type;
        this.arg = arg;

        if(triggers == null) {
            triggers = new Dictionary<(EventTriggerType, string), EventTrigger>();
        }

        triggers[(type, arg)] = this;
    }
}

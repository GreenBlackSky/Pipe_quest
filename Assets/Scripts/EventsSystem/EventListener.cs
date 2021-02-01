using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;

[XmlRoot("listener")]
public class EventListener {
    [XmlAttribute("id")]        public int id;
    [XmlAttribute("initial")]   public bool initial;
    [XmlAttribute("singleuse")] public bool singleuse;

    [XmlArray("triggers")]
    [XmlArrayItem("trigger")]
    public List<BaseEventTrigger> triggers;

    [XmlArray("conditions")]
    [XmlArrayItem("condition")]
    public List<BaseEventCondition> conditions;

    [XmlArray("callbacks")]
    [XmlArrayItem("callback")]
    public List<BaseEventCallback> callbacks;

    public bool CheckConditions() {
        foreach(BaseEventCondition condition in conditions) {
            if(!condition.Check()) {
                return false;
            }
        }
        return true;
    }

    public void devPing() {
        Debug.Log($"LISTENER {id} PING");
        foreach(var trigger in this.triggers) {
            trigger.devPing();
        }
        foreach(var condition in this.conditions) {
            condition.devPing();
        }
        foreach(var callback in this.callbacks) {
            callback.devPing();
        }
    }
}
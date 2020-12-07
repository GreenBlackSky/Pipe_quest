using System.Xml.Serialization;
using System.Collections.Generic;


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
}
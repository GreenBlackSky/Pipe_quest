using System.Collections.Generic;
using System.Xml.Serialization;


[XmlRoot("quest")]
public class Quest {
    [XmlElement("id")]    public string id;
    [XmlElement("name")]  public string name;

    [XmlArray("nodes")]
    [XmlArrayItem("node")]
    public List<QuestNode> nodes;
}

[XmlRoot("counter")]
public class QuestCounter {
    [XmlElement("text")]    public string text;
    [XmlElement("target")]  public int couterTarget;
    [XmlElement("value")]   public int coubterValue;
}

[XmlRoot("node")]
public class QuestNode {
    [XmlAttribute("id")]        public int id; 
    [XmlElement("description")] public string description;
    [XmlElement("counter")] public QuestCounter counter;

    [XmlArray("children")]
    [XmlArrayItem("child_id")]
    public List<int> childrenNodes;
}
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;


[XmlRoot("reply")]
public class DialogueReply : ScriptableObject {
    [XmlElement("text")]
    public string text;

    [XmlElement("nextLineUID")]
    public int nextLineUID;
}

[XmlRoot("line")]
public class DialogueNode : ScriptableObject {
    [XmlElement("uid")]
    public int lineUID;

    [XmlElement("speakerUID")]
    public int speakerUID;

    [XmlElement("text")]
    public string text;

    [XmlArray("replies")]
    [XmlArrayItem("reply")]
    public List<DialogueReply> replies;
}
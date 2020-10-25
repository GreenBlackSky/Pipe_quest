using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;


[XmlRoot("reply")]
public class DialogueReply {
    [XmlElement("text")]
    public string text;

    [XmlElement("nextLineUID")]
    public int nextLineUID;

    public DialogueReply() {
        text = "";
        nextLineUID = -1;
    }
}

[XmlRoot("line")]
public class DialogueNode {
    [XmlElement("uid")]
    public int lineUID;

    [XmlElement("speakerUID")]
    public string speakerUID;

    [XmlElement("text")]
    public string text;

    [XmlArray("replies")]
    [XmlArrayItem("reply")]
    public List<DialogueReply> replies;

    public DialogueNode() {
        replies = new List<DialogueReply>();
    }
}
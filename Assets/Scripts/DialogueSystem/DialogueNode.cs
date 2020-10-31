using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;


[XmlRoot("reply")]
// [XmlType(Namespace = "Game")]
public class DialogueReply {
    [XmlElement("text")]        public string text;
    [XmlElement("nextLineID")] public int nextLineID;

    public DialogueReply() {
        text = "";
        nextLineID = -1;
    }
}

[XmlRoot("line")]
public class DialogueNode {
    [XmlElement("uid")]         public int lineID;
    [XmlElement("speakerUID")]  public string speakerUID;
    [XmlElement("text")]        public string text;

    [XmlArray("replies")]
    [XmlArrayItem("reply")]
    public List<DialogueReply> replies;

    public DialogueNode() {
        replies = new List<DialogueReply>();
    }
}
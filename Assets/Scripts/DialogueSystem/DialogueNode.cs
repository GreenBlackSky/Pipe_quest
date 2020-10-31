using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;


[XmlRoot("reply")]
public class DialogueReply {
    [XmlElement("text")]        public string text;
    [XmlElement("nextLineID")]  public int nextLineID;

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
    [XmlElement("position")]    public Vector2 position; // HACK it shouldn't be here

    [XmlArray("replies")]
    [XmlArrayItem("reply")]
    public List<DialogueReply> replies;

    public DialogueNode(int lineID, Vector2 position) {
        replies = new List<DialogueReply>();
        this.position = position;
        this.lineID = lineID;
    }
    public DialogueNode() {
        replies = new List<DialogueReply>();
    }
}
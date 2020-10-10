using System;
using System.Xml;
using System.Collections.Generic;
using UnityEngine;

public class Talkable : MonoBehaviour, Interactable
{
    public int speakerUID;
    public string speakerName;
    public GameObject icon;
    public int initialNodeUID;
    public Dictionary<int, DialogueNode> lines;

    void Start() {
        string path = "Assets/DialoguesData/" + speakerName + ".xml";        
        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        XmlNode root = doc.DocumentElement.SelectSingleNode("/conversation");

        initialNodeUID = Int32.Parse(root.SelectSingleNode("initialLineUID").InnerText);
        XmlNode xmlLines = root.SelectSingleNode("lines");
        lines = new Dictionary<int, DialogueNode>();
        foreach(XmlNode lineData in xmlLines.ChildNodes) {
            int lineUID = Int32.Parse(lineData.SelectSingleNode("uid").InnerText);
            lines[lineUID] = ScriptableObject.CreateInstance<DialogueNode>();
            lines[lineUID].lineUID = lineUID;
            lines[lineUID].text = lineData.SelectSingleNode("text").InnerText;
            lines[lineUID].replies = new List<Reply>();
            foreach(XmlNode replyData in lineData.SelectSingleNode("replies").ChildNodes){
                Reply reply = new Reply();
                reply.text = replyData.SelectSingleNode("text").InnerText;
                reply.nextLineUID = Int32.Parse(replyData.SelectSingleNode("nextLineUID").InnerText);
                lines[lineUID].replies.Add(reply);
            }
        }
    }
}

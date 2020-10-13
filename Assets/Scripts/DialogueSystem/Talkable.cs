using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;

// [XmlRoot("conversation")]
public class Talkable : MonoBehaviour, Interactable
{
    public int speakerUID;
    public string speakerName;
    public GameObject icon;
    public Dictionary<int, DialogueNode> lines;
    
    // [XmlElement("initialLineUID")]
    public int initialNodeUID;

    void Start() {
        // TODO refactor serialization
        string path = "Assets/DialoguesData/" + speakerName + ".xml";        
        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        XmlNode root = doc.DocumentElement.SelectSingleNode("/conversation");
        XmlNode xmlLines = root.SelectSingleNode("lines");

        initialNodeUID = Int32.Parse(root.SelectSingleNode("initialLineUID").InnerText);
        lines = new Dictionary<int, DialogueNode>();

        XmlSerializer nodeSerializer = new XmlSerializer(typeof(DialogueNode));
        foreach(XmlNode lineData in xmlLines.ChildNodes) {
            DialogueNode node = (nodeSerializer.Deserialize(new XmlNodeReader(lineData)) as DialogueNode);
            lines[node.lineUID] = node;
        }
    }
}

using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections;
using UnityEngine;

public class Talkable : MonoBehaviour, Interactable
{
    public string speakerUID;
    public string speakerFullName;
    public GameObject icon;

    // TODO load in speaker manager
    public DialogueNode[] lines;
    public int initialNodeUID;

    void Start() {
        string path = "Assets/DialoguesData/" + speakerUID + ".xml";        
        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        XmlNode root = doc.DocumentElement.SelectSingleNode("/conversation");

        initialNodeUID = Int32.Parse(root.SelectSingleNode("initialLineUID").InnerText);
        speakerFullName = root.SelectSingleNode("fullName").InnerText;

        XmlSerializer nodeSerializer = new XmlSerializer(typeof(DialogueNode));
        XmlNode xmlLines = root.SelectSingleNode("lines");
        lines = new DialogueNode[xmlLines.ChildNodes.Count];
        foreach(XmlNode lineData in xmlLines.ChildNodes) {
            DialogueNode node = (nodeSerializer.Deserialize(new XmlNodeReader(lineData)) as DialogueNode);
            lines[node.lineUID] = node;
        }
    }
}

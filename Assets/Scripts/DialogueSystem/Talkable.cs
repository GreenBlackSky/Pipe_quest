// using System;
// using System.Xml;
// using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;

public class Talkable : MonoBehaviour, Interactable
{
    public string speakerUID;
    public GameObject icon;

    // [XmlElement("fullName")]        
    public string speakerFullName;

    // [XmlElement("initialLineUID")] 
    public int initialNodeUID;

    // [XmlArray("lines")]
    // [XmlArrayItem("line")]
    public List<DialogueNode> lines;
}

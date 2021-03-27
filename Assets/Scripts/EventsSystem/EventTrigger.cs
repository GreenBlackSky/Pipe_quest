using System;
using UnityEngine;
using System.Xml.Serialization;


public class BaseEventTrigger {
    [XmlAttribute("arg")] public string arg;
    [XmlAttribute("type")] public string type;

    public void devPing() {
        Debug.Log($"PING TRIGGER {this.type}, {this.arg}");
    }
}
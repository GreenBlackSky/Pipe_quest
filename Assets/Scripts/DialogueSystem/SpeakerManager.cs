using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerManager : MonoBehaviour
{
    static public SpeakerManager instance;

    Dictionary<string, Talkable> speakers;

    void Start()
    {
        speakers = new Dictionary<string, Talkable>();
        Talkable[] foundSpeakers = FindObjectsOfType<Talkable>();

        XmlSerializer nodeSerializer = new XmlSerializer(typeof(DialogueNode));
        foreach(Talkable speaker in foundSpeakers) {
            LoadSpeaker(speaker, nodeSerializer);
            speakers[speaker.speakerUID] = speaker;
        }
        instance = this;
    }

    private void LoadSpeaker(Talkable speaker, XmlSerializer nodeSerializer) {
        string path = "Assets/DialoguesData/" + speaker.speakerUID + ".xml";        
        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        XmlNode root = doc.DocumentElement.SelectSingleNode("/conversation");

        speaker.initialNodeUID = Int32.Parse(root.SelectSingleNode("initialLineUID").InnerText);
        speaker.speakerFullName = root.SelectSingleNode("fullName").InnerText;

        XmlNode xmlLines = root.SelectSingleNode("lines");
        speaker.lines = new List<DialogueNode>();
        foreach(XmlNode lineData in xmlLines.ChildNodes) {
            speaker.lines.Add(nodeSerializer.Deserialize(new XmlNodeReader(lineData)) as DialogueNode);
        }
    }

    public Talkable GetSpeaker(string name) {
        return speakers[name];
    }
}

using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour {
    public static Dictionary<string, Quest> allQuests;

    void Start() {
        allQuests = new Dictionary<string, Quest>();
    }

    public void LoadAllQuests(string level_name) {
        allQuests.Clear();
        string path = "Assets/QuestData/" + level_name + ".xml";
        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        XmlNode root = doc.DocumentElement.SelectSingleNode("/Level");

        XmlSerializer nodeSerializer = new XmlSerializer(typeof(Quest));
        foreach(XmlNode questData in root.ChildNodes) {
            Quest quest = nodeSerializer.Deserialize(new XmlNodeReader(questData)) as Quest;
            allQuests[quest.id] = quest;
        }
        // TODO link nodes
    }
}

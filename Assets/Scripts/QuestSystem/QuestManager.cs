using System;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager {
    public static Dictionary<string, Quest> allQuests;

    public static void Init(string level_name) {
        if(allQuests == null) {
            allQuests = new Dictionary<string, Quest>();
        } else {
            allQuests.Clear();
        }

        string path = "Assets/QuestData/" + level_name + ".xml";
        XmlDocument doc = new XmlDocument();
        doc.Load(path);
        XmlNode root = doc.DocumentElement.SelectSingleNode("/level");

        XmlSerializer questSerializer = new XmlSerializer(typeof(Quest));
        foreach(XmlNode questData in root.ChildNodes) {
            Quest quest = questSerializer.Deserialize(new XmlNodeReader(questData)) as Quest;
            allQuests[quest.id] = quest;
        }
    }
}

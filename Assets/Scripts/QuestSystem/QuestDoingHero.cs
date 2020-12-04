using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuestDoingHero : MonoBehaviour {
    public GameObject QuestsUI;
    public Dictionary<string, int> flags;
    public Dictionary<int, Quest> allQuests;
    public Dictionary<int, Quest> activeQuests;
    public Dictionary<int, Quest> completedQuests;

    private void Start() {
        flags = new Dictionary<string, int>();
        allQuests = new Dictionary<int, Quest>();
        activeQuests = new Dictionary<int, Quest>();
        completedQuests = new Dictionary<int, Quest>();
        // TODO load quests from xml
    }

    public void SetFlag(string flagName) {
        if(!flags.ContainsKey(flagName)) {
            flags[flagName] = 0;
        }
        flags[flagName]++;
    }

    public void UnsetFlag(string flagName) {
        if(!flags.ContainsKey(flagName)) {
            return;
        }
        flags[flagName]--;
        if(flags[flagName] == 0) {
            flags.Remove(flagName);
        }
    }
}

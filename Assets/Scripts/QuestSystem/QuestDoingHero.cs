using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class QuestDoingHero : MonoBehaviour {
    public GameObject QuestsUI;
    public Dictionary<string, int> flags;
    public Dictionary<int, Quest> activeQuests;
    public Dictionary<int, Quest> completedQuests;

    private void Start() {
        flags = new Dictionary<string, int>();
        activeQuests = new Dictionary<int, Quest>();
        completedQuests = new Dictionary<int, Quest>();
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

    public void StartQuest(Quest quest) {

    }

    public void ProgressQuestNode(int questId, int value) {

    }

    public void SwitchQuestNode(int questId, int newNodeId) {
        
    }
}

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
}

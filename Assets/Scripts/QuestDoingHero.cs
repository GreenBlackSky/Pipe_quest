using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;


public class QuestDoingHero : MonoBehaviour, InteractionListener {
    // TODO pass description and name to ui
    public GameObject QuestsUI;
    Dictionary<string, HashSet<QuestNode>> _activeQuests;

    Dictionary<string, UnityAction> _callbacks;

    // TODO unuse InteractionListener and chain a start of a quest to the dialog
    public void interact(GameObject interactable) {
        QuestGiver questGiver = interactable.GetComponent<QuestGiver>();
        if (questGiver is null)
        {
            return;
        }

        QuestNode quest = questGiver.quest.head;
        if(_activeQuests[quest.type].Count == 0) {
            EventManager.StartListening(quest.type, _callbacks[quest.type]);
        }
        _activeQuests[quest.type].Add(quest);
        Text[] textFields = QuestsUI.GetComponentsInChildren<Text>();
        textFields[0].text = questGiver.quest.name;
        textFields[1].text = questGiver.quest.description;

    }

    void Callback(string Action) {
        List<QuestNode> CompletedQuests = new List<QuestNode>();
        List<QuestNode> NewQuests = new List<QuestNode>();
        foreach(QuestNode quest in _activeQuests[Action]) {
            quest.progress++;
            if(quest.progress == quest.targetAmount) {
                CompletedQuests.Add(quest);
                if(!(quest.nextNode is null)) {
                    NewQuests.Add(quest.nextNode);
                }
                Text[] textFields = QuestsUI.GetComponentsInChildren<Text>();
                textFields[0].text = "";
                textFields[1].text = "";
                // TODO notifications
                // TODO completed quests
            }
        }

        foreach(QuestNode quest in NewQuests) {
            if(_activeQuests[quest.type].Count == 0) {
                EventManager.StartListening(quest.type, _callbacks[quest.type]);
            }
            _activeQuests[quest.type].Add(quest);
        }

        foreach(QuestNode quest in CompletedQuests) {
            _activeQuests[Action].Remove(quest);
        }
        if(_activeQuests[Action].Count == 0) {
            EventManager.StopListening(Action, _callbacks[Action]);
        }
    }

    void Collected() {
        Callback("Collect");
    }

    void Talked() {
        Callback("Talk");
    }

    void Start() {
        _callbacks = new Dictionary<string, UnityAction>() {
            {"Collect", Collected},
            {"Talk", Talked}
        };
        _activeQuests = new Dictionary<string, HashSet<QuestNode>>() {
            {"Collect", new HashSet<QuestNode>()},
            {"Talk", new HashSet<QuestNode>()}
        };
    }
}

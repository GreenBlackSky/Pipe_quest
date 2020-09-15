using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

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

    // TODO new reach event
    void Start() {
        _callbacks = new Dictionary<string, UnityAction>() {
            {"Collect", Collected}
        };
        _activeQuests = new Dictionary<string, HashSet<QuestNode>>() {
            {"Collect", new HashSet<QuestNode>()}
        };
    }
}

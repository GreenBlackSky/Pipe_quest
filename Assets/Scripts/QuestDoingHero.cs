using System.Collections.Generic;
using UnityEngine;

public class QuestDoingHero : MonoBehaviour, InteractionListener
{
    // TODO use structs to track quest progress
    HashSet<Quest> _activeQuests;

    public GameObject QuestsUI;
    // TODO unuse InteractionListener and chain a start of a quest to the start of a dialog
    public void interact(GameObject interactable)
    {
        QuestGiver questGiver = interactable.GetComponent<QuestGiver>();
        if (questGiver is null)
        {
            return;
        }
        _activeQuests.Add(questGiver.quest);
        EventManager.StartListening(questGiver.quest.type, ConditionMet);
    }

    void ConditionMet()
    {
        Debug.Log("Completed");
    }

    void Start()
    {
        _activeQuests = new HashSet<Quest>();
    }
}

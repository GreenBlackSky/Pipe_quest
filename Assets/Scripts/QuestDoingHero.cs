using System.Collections.Generic;
using UnityEngine;

public class QuestDoingHero : MonoBehaviour, InteractionListener
{
    HashSet<Quest> _activeQuests;

    public GameObject QuestsUI;
    public void interact(GameObject interactable)
    {
        QuestGiver questGiver = interactable.GetComponent<QuestGiver>();
        if (questGiver is null)
        {
            return;
        }
        _activeQuests.Add(questGiver.quest);
    }

    void Start()
    {
        _activeQuests = new HashSet<Quest>();
    }
}

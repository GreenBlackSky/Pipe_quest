using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UIManager;

public class TalkingHero : MonoBehaviour, InteractionListener
{
    public GameObject DialogPanel;
    
    Talkable talkable;
    int currentNodeId;

    public void nextLine()
    {
        // QD_Dialogue dialogue = talkable.dialogue;
        // if (dialogue.Conversations.Count == 0)
        // {
        //     UIManager.Instance.SwitchState(State.GAMEPLAY);
        //     return;
        // }

        // QD_Message message = dialogue.GetMessage(currentNodeId);
        // if(message == null) {
        //     UIManager.Instance.SwitchState(State.GAMEPLAY);
        //     return;
        // }
        
        // DialogPanel.GetComponentInChildren<Text>().text = message.MessageText; 
        
        // int nextNodeId = message.NextMessage;
        // QD_Choice choice = dialogue.GetChoice(nextNodeId);
        // if (choice == null) {
        //     currentNodeId = nextNodeId;
        //     return;
        // }
    }

    public void interact(GameObject interactable)
    {
        talkable = interactable.GetComponent<Talkable>();
        currentNodeId = talkable.initianNodeId;
        if (talkable is null)
        {
            return;
        }
        UIManager.Instance.SwitchState(State.DIALOG);
        nextLine();
    }

    void Start()
    {}
}

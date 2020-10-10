using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UIManager;

public class TalkingHero : MonoBehaviour, InteractionListener
{
    public int heroSpeakerUID;
    public string heroSpeakerName;
    public GameObject heroIcon;
    public GameObject replyButton;

    Text speakerNameArea;
    Image iconSlot;
    Text textPanel;
    GameObject repliesArea;
    
    
    Talkable talkable;
    int currentSpeakerUID = -1;

    public void nextLine(int nodeId)
    {
        DialogueNode node = talkable.lines[nodeId];
        if(currentSpeakerUID != node.speakerUID) {
            // TODO allow more speakers
            if(currentSpeakerUID == heroSpeakerUID) {
                speakerNameArea.text = heroSpeakerName;
                GameObject icon = Instantiate(heroIcon, iconSlot.transform, false);
            } else {
                speakerNameArea .text = talkable.speakerName;
                GameObject icon = Instantiate(talkable.icon, iconSlot.transform, false);
            }
        }
        textPanel.text = node.text;
        int verticalShift = 0; // TODO position reply
        foreach(Reply reply in node.replies) {
            GameObject replayButtonInstance = Instantiate(replyButton, repliesArea.transform, false);
            replayButtonInstance.GetComponentInChildren<Text>().text = reply.text;
            // replayButtonInstance.GetComponent<Button>().onClick.AddListener(nextLine); 
            // TODO callback on reply
            replayButtonInstance.GetComponent<Transform>().localPosition = new Vector3(0, verticalShift, 0);
            verticalShift += 50;
        }
    }

    public void interact(GameObject interactable)
    {
        talkable = interactable.GetComponent<Talkable>();
        if (talkable is null) {
            return;
        }
        UIManager.Instance.SwitchState(State.DIALOG);
        nextLine(talkable.initialNodeUID);
    }

    void Start() {
        UIManager.Instance.SwitchState(State.DIALOG);
        speakerNameArea = GameObject.Find("SpeakerNameArea").GetComponent<Text>();
        textPanel = GameObject.Find("DialogTextArea").GetComponent<Text>();
        iconSlot = GameObject.Find("SpeakerIconSlot").GetComponent<Image>();
        repliesArea = GameObject.Find("RepliesContentArea");
        UIManager.Instance.SwitchState(State.GAMEPLAY);
    }
}

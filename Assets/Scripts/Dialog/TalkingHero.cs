using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UIManager;

public class TalkingHero : MonoBehaviour, InteractionListener
{
    public int speakerUID;
    public string speakerName;
    public GameObject icon;

    Text speakerNameArea;
    Image iconSlot;
    Text textPanel;
    
    Talkable talkable;
    int currentSpeakerUID = -1;

    public void nextLine(int nodeId)
    {
        DialogNode node = talkable.nodes[nodeId];
        if(currentSpeakerUID != node.speakerUID) {
            if(currentSpeakerUID == speakerUID) {
                speakerNameArea.text = speakerName;
                Instantiate(icon, iconSlot.transform, false);
            } else {
                speakerNameArea .text = talkable.speakerName;
                Instantiate(talkable.icon, iconSlot.transform, false);
            }
        }
        textPanel.text = node.text;
        // TODO create replies
    }
    // TODO get replies

    public void interact(GameObject interactable)
    {
        talkable = interactable.GetComponent<Talkable>();
        if (talkable is null) {
            return;
        }
        UIManager.Instance.SwitchState(State.DIALOG);
        speakerNameArea = GameObject.Find("SpeakerNameArea").GetComponent<Text>();
        textPanel = GameObject.Find("DialogTextArea").GetComponent<Text>();
        iconSlot = GameObject.Find("SpeakerIconSlot").GetComponent<Image>();
        nextLine(talkable.initianNodeUID);
    }

    void Start() {

    }
}

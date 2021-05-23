using UnityEngine;
using UnityEngine.UI;
using static GameManager;


// TODO partly move to DialogueController
public class TalkingHero : MonoBehaviour, InteractionListener
{
    public GameObject heroIcon;
    public GameObject replyButton;

    public int replyButtonDistance = 10;
    bool initialized = false;

    Text speakerNameArea;
    Image iconSlot;
    Text textPanel;
    GameObject repliesArea;
    Transform speakerIcon;
    
    Speaker speaker;
    string currentSpeakerName = "";

    void setSpeaker(DialogueNode node) {
        if(currentSpeakerName != node.speakerUID) {
            Speaker speaker = DialogueManager.GetSpeaker(node.speakerUID);
            speakerNameArea.text = this.speaker.speakerFullName;

            Destroy(speakerIcon); // BUG can't remove transform becouse script depends on it
            // TODO animated icons
            // TODO set in editor
            Transform icon = speaker.transform.Find("Image");
            speakerIcon = Instantiate(icon, iconSlot.transform, false);
            currentSpeakerName = node.speakerUID;
        }
    }

    void setReplies(DialogueNode node) {
        foreach(Transform child in repliesArea.transform) {
            Destroy(child.gameObject);
        }
        int i = 0;
        float buttonWidth = replyButton.GetComponent<RectTransform>().sizeDelta.y;
        foreach(DialogueReply reply in node.replies) {
            GameObject replayButtonInstance = Instantiate(replyButton, repliesArea.transform, false);
            Vector3 position = replayButtonInstance.GetComponent<Transform>().localPosition;
            replayButtonInstance.GetComponent<Transform>().localPosition = new Vector3(
                position.x, 
                position.y - i * (buttonWidth + replyButtonDistance),
                position.z
            );
            replayButtonInstance.GetComponentInChildren<Text>().text = reply.text;
            if(reply.nextLineID == -1) {
                // replayButtonInstance.GetComponent<Button>().onClick.AddListener(delegate{
                //     GameManager.Instance.SwitchState(State.GAMEPLAY);
                // }); 
            } else {
                replayButtonInstance.GetComponent<Button>().onClick.AddListener(delegate{
                    nextLine(reply.nextLineID);
                }); 
            }
            i++;
        }
    }

    public void nextLine(int nodeID)
    {
        // TODO trigger event
        DialogueNode node = speaker.lines[nodeID];
        setSpeaker(node);
        textPanel.text = node.text;
        setReplies(node);
    }

    public void interact(GameObject interactable)
    {
        speaker = interactable.GetComponent<Speaker>();
        if (speaker is null) {
            return;
        }
        // GameManager.Instance.SwitchState(State.DIALOG);
        if(!initialized) {
            Init();
        }
        nextLine(speaker.initialNodeID);
    }

    void Init() {
        // TODO set in editor
        speakerNameArea = GameObject.Find("SpeakerNameArea").GetComponent<Text>();
        textPanel = GameObject.Find("DialogTextArea").GetComponent<Text>();
        iconSlot = GameObject.Find("SpeakerIconSlot").GetComponent<Image>();
        repliesArea = GameObject.Find("RepliesContentArea");
        initialized = true;
    }
}

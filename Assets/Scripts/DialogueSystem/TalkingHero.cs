using UnityEngine;
using UnityEngine.UI;
using static GameManager;


public class TalkingHero : MonoBehaviour, InteractionListener
{
    public GameObject heroIcon;
    public GameObject replyButton;

    public int replyButtonDistance = 10;

    Text speakerNameArea;
    Image iconSlot;
    Text textPanel;
    GameObject repliesArea;
    Transform speakerIcon;
    
    
    Speaker speaker;
    string currentSpeakerName = "";

    void setSpeaker(DialogueNode node) {
        if(currentSpeakerName != node.speakerUID) {
            Speaker speaker = SpeakerManager.instance.GetSpeaker(node.speakerUID);
            speakerNameArea.text = this.speaker.speakerFullName;

            Destroy(speakerIcon);
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
                replayButtonInstance.GetComponent<Button>().onClick.AddListener(delegate{
                    GameManager.Instance.SwitchState(State.GAMEPLAY);
                }); 
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
        // TODO events
        // TODO phasing
        // TODO scrolling
        // TODO Sounds
        // TODO resize replies area
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
        GameManager.Instance.SwitchState(State.DIALOG);
        nextLine(speaker.initialNodeID);
    }

    void Start() {
        GameManager.Instance.SwitchState(State.DIALOG);
        speakerNameArea = GameObject.Find("SpeakerNameArea").GetComponent<Text>();
        textPanel = GameObject.Find("DialogTextArea").GetComponent<Text>();
        iconSlot = GameObject.Find("SpeakerIconSlot").GetComponent<Image>();
        repliesArea = GameObject.Find("RepliesContentArea");
        GameManager.Instance.SwitchState(State.PAUSE_MENU);
    }
}

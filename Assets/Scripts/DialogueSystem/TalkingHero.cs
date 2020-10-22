using UnityEngine;
using UnityEngine.UI;
using static UIManager;

public class TalkingHero : MonoBehaviour, InteractionListener
{
    public string heroSpeakerName;
    public GameObject heroIcon;
    public GameObject replyButton;

    public int replyButtonDistance = 10;

    Text speakerNameArea;
    Image iconSlot;
    Text textPanel;
    GameObject repliesArea;
    
    
    Talkable talkable;
    string currentSpeakerName = "";

    void setSpeaker(DialogueNode node) {
        if(currentSpeakerName != node.speakerName) {
            // TODO allow more speakers
            if(currentSpeakerName == heroSpeakerName) {
                speakerNameArea.text = heroSpeakerName;
                GameObject icon = Instantiate(heroIcon, iconSlot.transform, false);
            } else {
                speakerNameArea .text = talkable.speakerName;
                GameObject icon = Instantiate(talkable.icon, iconSlot.transform, false);
            }
        }
    }

    void setReplies(DialogueNode node) {
        foreach(Transform child in repliesArea.transform) {
            Destroy(child.gameObject);
        }
        // FIXME resize replies area
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
            if(reply.nextLineUID == -1) {
                replayButtonInstance.GetComponent<Button>().onClick.AddListener(delegate{
                    UIManager.Instance.SwitchState(State.GAMEPLAY);
                }); 
            } else {
                replayButtonInstance.GetComponent<Button>().onClick.AddListener(delegate{
                    nextLine(reply.nextLineUID);
                }); 
            }
            i++;
        }
    }

    public void nextLine(int nodeID)
    {
        // TODO add events
        // TODO phasing
        // TODO scrolling
        DialogueNode node = talkable.lines[nodeID];
        setSpeaker(node);
        textPanel.text = node.text;
        setReplies(node);
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

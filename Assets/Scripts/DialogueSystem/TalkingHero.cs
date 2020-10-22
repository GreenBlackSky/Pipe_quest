using UnityEngine;
using UnityEngine.UI;
using static UIManager;

public class TalkingHero : MonoBehaviour, InteractionListener
{
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
        if(currentSpeakerName != node.speakerUID) {
            Talkable speaker = SpeakerManager.instance.GetSpeaker(node.speakerUID);
            Debug.Log(speaker.speakerFullName);
            speakerNameArea.text = talkable.speakerFullName;
            GameObject icon = Instantiate(speaker.icon, iconSlot.transform, false);
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
        // TODO events
        // TODO phasing
        // TODO scrolling
        // TODO Sounds
        // TODO resize replies area
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

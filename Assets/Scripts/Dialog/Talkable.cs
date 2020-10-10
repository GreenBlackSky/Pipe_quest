using System.Collections.Generic;
using UnityEngine;

public class Talkable : MonoBehaviour, Interactable
{
    public int speakerUID;
    public string speakerName;
    public GameObject icon;
    public int initianNodeUID;
    public Dictionary<int, DialogNode> nodes;

    void Start() {
        // TODO fill all data for all objects on startup
        nodes = new Dictionary<int, DialogNode>();
        speakerUID = 1;
        initianNodeUID = 0;
        nodes[initianNodeUID] = ScriptableObject.CreateInstance<DialogNode>();
        nodes[initianNodeUID].nodeUID = initianNodeUID;
        nodes[initianNodeUID].speakerUID = speakerUID;
        nodes[initianNodeUID].text = "Hello";
        nodes[initianNodeUID].replies = new Dictionary<int, Reply>();
    }
}

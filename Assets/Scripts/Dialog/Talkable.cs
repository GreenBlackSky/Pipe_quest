using System.Collections.Generic;
using UnityEngine;

public class Talkable : MonoBehaviour, Interactable
{
    int speakerUID;
    public string personName;
    public GameObject icon;
    public int initianNodeId;
    public Dictionary<int, DialogNode> nodes;
}

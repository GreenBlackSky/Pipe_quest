using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour, Interactable
{
    public string speakerUID;
    public GameObject icon;

    public string speakerFullName;

    public int initialNodeID;

    public List<DialogueNode> lines;
}

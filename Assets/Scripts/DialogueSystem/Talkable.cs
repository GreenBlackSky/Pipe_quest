using System.Collections.Generic;
using UnityEngine;

public class Talkable : MonoBehaviour, Interactable
{
    public string speakerUID;
    public GameObject icon;

    public string speakerFullName;

    public int initialNodeUID;

    public List<DialogueNode> lines;
}

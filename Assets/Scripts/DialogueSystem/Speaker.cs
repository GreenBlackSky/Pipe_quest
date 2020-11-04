using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour, Interactable
{
    public string speakerUID;
    
    [HideInInspector]
    public string speakerFullName;

    [HideInInspector]
    public int initialNodeID;

    [HideInInspector]
    public List<DialogueNode> lines;
}

using System.Collections.Generic;
using UnityEngine;

public class Speaker : MonoBehaviour, Interactable
{
    // TODO search for icon
    public string speakerUID;
    
    [HideInInspector]
    public string speakerFullName;

    [HideInInspector]
    public int initialNodeID;

    [HideInInspector]
    public List<DialogueNode> lines;
}

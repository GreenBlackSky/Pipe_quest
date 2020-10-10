using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Reply {
    public string text;
    public int nextLineUID;
}

public class DialogueNode : ScriptableObject
{
    public int lineUID;
    public int speakerUID;
    public string text;
    public List<Reply> replies;
}

// TODO Editor
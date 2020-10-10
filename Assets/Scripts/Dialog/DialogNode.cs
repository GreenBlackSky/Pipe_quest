using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Reply {
    string text;
    int nodeUID;
}

public class DialogNode : ScriptableObject
{
    public int nodeUID;
    public int speakerUID;
    public string text;
    public Dictionary<int, Reply> replies;
}

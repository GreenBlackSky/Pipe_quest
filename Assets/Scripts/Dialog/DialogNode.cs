using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogNode : MonoBehaviour
{
    int NodeUID;
    int speakerUID;
    public string text;
    public Dictionary<int, string> reples;
    public Dictionary<int, int> children;
}

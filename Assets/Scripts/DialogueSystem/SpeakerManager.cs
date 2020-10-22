using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeakerManager : MonoBehaviour
{
    static public SpeakerManager instance;

    Dictionary<string, Talkable> speakers;

    void Start()
    {
        speakers = new Dictionary<string, Talkable>();
        Talkable[] foundSpeakers = FindObjectsOfType<Talkable>();
        foreach(Talkable speaker in foundSpeakers) {
            speakers[speaker.speakerUID] = speaker;
        }
        instance = this;
    }

    public Talkable GetSpeaker(string name) {
        return speakers[name];
    }
}

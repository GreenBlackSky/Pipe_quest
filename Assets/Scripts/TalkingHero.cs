using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static GameplayManager;

public class TalkingHero : MonoBehaviour, InteractionListener
{
    public GameObject DialogPanel;

    Queue<String> _lines;

    public void nextLine()
    {
        if (_lines.Count != 0)
        {
            DialogPanel.GetComponentInChildren<Text>().text = _lines.Dequeue();
        } else
        {
            GameplayManager.Instance.SwitchState(State.GAMEPLAY);
        }
    }

    public void interact(GameObject interactable)
    {
        Talkable talkable = interactable.GetComponent<Talkable>();
        if (talkable is null)
        {
            return;
        }
        foreach(String line in talkable.lines)
        {
            _lines.Enqueue(line);
        }
        nextLine();
        GameplayManager.Instance.SwitchState(State.DIALOG);
    }

    void Start()
    {
        _lines = new Queue<string>();
    }
}

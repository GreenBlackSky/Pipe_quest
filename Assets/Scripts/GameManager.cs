using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // TODO menu and levels
    public enum State {MAIN_MENU, PLAYING}
    State _state;

    void Start()
    {
        _state = State.MAIN_MENU;
    }

    void Update()
    {
        
    }

    void LeaveState(State state)
    {

    }

    void EnterState(State state)
    {

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO menu animation
// TODO menu keys
public class MainMenuManager : MonoBehaviour {
    public enum State {
        MAIN_MENU,
        NEW_GAME_MENU,
        LOAD_GAME_MENU,
        SETTINGS_MENU,
        SCREEN_SETTINGS,
        AUDIO_SETTINGS,
        CONTROLS_SETTINGS,
        
    }

    State _state;

    public void SwitchState(State state) {

    }

    void _leaveState() {

    }

    void _enterState() {

    }

    void Start() { }

    void Update() { }

}

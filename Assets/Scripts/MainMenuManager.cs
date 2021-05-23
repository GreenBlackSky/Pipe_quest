using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO menu animation
// TODO menu keys
public class MainMenuManager : MonoBehaviour {

    public static MainMenuManager Instance { get; private set; }

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
    
    public GameObject MainMenuPanel;
    public GameObject NewGamePanel;
    public GameObject LoadGamePanel;
    public GameObject SettingsPanel;
    public GameObject ScreenSettingsPanel;
    public GameObject AudioSettingsPanel;
    public GameObject ControlsSettingsPanel;

    Dictionary<State, GameObject> _uiPanels;

    public void SwitchState(State state) {
        this._uiPanels[this._state].SetActive(false);
        this._state = state;
        this._uiPanels[this._state].SetActive(true);
    }

    void Start() {
        this._uiPanels = new Dictionary<State, GameObject>() {
            {State.MAIN_MENU, this.MainMenuPanel},
            {State.NEW_GAME_MENU, this.NewGamePanel},
            {State.LOAD_GAME_MENU, this.LoadGamePanel},
            {State.SETTINGS_MENU, this.SettingsPanel},
            {State.SCREEN_SETTINGS, this.ScreenSettingsPanel},
            {State.AUDIO_SETTINGS, this.AudioSettingsPanel},
            {State.CONTROLS_SETTINGS, this.ControlsSettingsPanel},
        };
        MainMenuManager.Instance = this;
    }

    void Update() { }

}

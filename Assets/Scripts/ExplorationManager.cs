using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayManager : MonoBehaviour {
    public enum State {
        EXPLORATION,
        TRADE,
        PAUSE_MENU,
        INVENTORY,
        DIALOG,
        SKILL_MENU,
        MAP_MENU,
        JOURNAL_MENU
    }

    State _state;
    State _previousState;

    public GameObject[] LevelPrefabs;
    public GameObject AvatarPrefab;
    
    GameObject _currentLevel;
    GameObject _currentAvatar;

    public GameObject InteractionButton;
    public GameObject ExplorationUI;
    public GameObject TradeUI;
    public GameObject PauseMenuPanel;
    public GameObject InventoryPanel;
    public GameObject QuestsPanel;
    public GameObject DialoguePanel;
    public GameObject MapMenuPanel;
    public GameObject SkillsMenuPanel;

    Dictionary<State, GameObject> _uiPanels;

    public void SwitchState(State state) {
        this._leaveState();
        this._previousState = this._state;
        this._state = state;
        this._enterState();
    }

    void _leaveState() {
        this._uiPanels[this._state].SetActive(false);
    }

    void _enterState() {
        this._uiPanels[this._state].SetActive(true);
    }

    void Start() {
        this._uiPanels = new Dictionary<State, GameObject>() {
            {State.EXPLORATION, this.ExplorationUI},
            {State.PAUSE_MENU, this.PauseMenuPanel},
            {State.INVENTORY, this.InventoryPanel},
            {State.SKILL_MENU, this.SkillsMenuPanel},
            {State.MAP_MENU, this.MapMenuPanel},
            {State.DIALOG, this.DialoguePanel},
            {State.JOURNAL_MENU, this.QuestsPanel},
            {State.TRADE, this.TradeUI},
        };
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.I)) {
            SwitchState(State.INVENTORY);
        } else if (Input.GetKeyDown(KeyCode.J)) {
            SwitchState(State.JOURNAL_MENU);
        } else if (Input.GetKeyDown(KeyCode.Tab)) {
            SwitchState(this._previousState);
        } else if (Input.GetKeyDown(KeyCode.Escape)) {
            SwitchState(State.PAUSE_MENU);
        } 
    }
}

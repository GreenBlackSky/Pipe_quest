using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject GameplayUI;
    public GameObject PauseMenuPanel;
    public GameObject InventoryPanel;
    public GameObject DialogPanel;
    public GameObject SkillMenuPanel;
    public GameObject MapMenuPanel;
    public GameObject CombatUI;
    public GameObject QuestsPanel;

    public static GameManager Instance { get; private set; }

    public enum State { GAMEPLAY, MAIN_MENU, COMBAT, PAUSE_MENU, INVENTORY, DIALOG, SKILL_MENU, MAP_MENU, QUESTS }
    State _state;

    public void SwitchState(State state)
    {
        LeaveState(_state);
        _state = state;
        EnterState();
    }

    void Start()
    {
        Instance = this;
        SwitchState(State.MAIN_MENU);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.I)) {
            SwitchState(State.INVENTORY);
        } else if (Input.GetKeyDown(KeyCode.J)) {
            SwitchState(State.QUESTS);
        } else if (Input.GetKeyDown(KeyCode.Tab)) {
            SwitchState(State.GAMEPLAY);
        } else if (Input.GetKeyDown(KeyCode.Escape)) {
            SwitchState(State.PAUSE_MENU);
        } 
    } 

    void LeaveState(State state)
    {
        switch (_state)
        {
            case State.GAMEPLAY:
                Time.timeScale = 0;
                GameplayUI.SetActive(false);
                break;
            case State.PAUSE_MENU:
                PauseMenuPanel.SetActive(false);
                break;
            case State.INVENTORY:
                InventoryPanel.SetActive(false);
                break;
            case State.SKILL_MENU:
                SkillMenuPanel.SetActive(false);
                break;
            case State.MAP_MENU:
                MapMenuPanel.SetActive(false);
                break;
            case State.DIALOG:
                DialogPanel.SetActive(false);
                break;
            case State.COMBAT:
                CombatUI.SetActive(false);
                break;
            case State.QUESTS:
                QuestsPanel.SetActive(false);
                break;
            case State.MAIN_MENU:
                MainMenu.SetActive(false);
                break;
        }
    }

    void EnterState()
    {
        switch (_state)
        {
            case State.GAMEPLAY:
                Time.timeScale = 1;
                GameplayUI.SetActive(true);
                break;
            case State.PAUSE_MENU:
                PauseMenuPanel.SetActive(true);
                break;
            case State.INVENTORY:
                InventoryPanel.SetActive(true);
                break;
            case State.SKILL_MENU:
                SkillMenuPanel.SetActive(true);
                break;
            case State.MAP_MENU:
                MapMenuPanel.SetActive(true);
                break;
            case State.DIALOG:
                DialogPanel.SetActive(true);
                break;
            case State.COMBAT:
                CombatUI.SetActive(true);
                break;
            case State.QUESTS:
                QuestsPanel.SetActive(true);
                break;
            case State.MAIN_MENU:
                MainMenu.SetActive(true);
                break;
        }
    }
}
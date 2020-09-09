using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameplayManager : MonoBehaviour
{
    public GameObject GameplayUI;
    public GameObject PauseMenuPanel;
    public GameObject InventoryPanel;
    public GameObject DialogPanel;
    public GameObject SkillMenuPanel;
    public GameObject MapMenuPanel;
    public GameObject CombatUI;

    public enum State { GAMEPLAY, COMBAT, PAUSE_MENU, INVENTORY, DIALOG_MENU, SKILL_MENU, MAP_MENU }
    State _state;

    void SwitchState(State state)
    {
        LeaveState(_state);
        _state = state;
        EnterState();
    }

    void Start()
    {
        SwitchState(State.GAMEPLAY);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            SwitchState((State)(((int)_state + 1) % 6));
        }
    } 

    void LeaveState(State state)
    {
        switch (_state)
        {
            case State.GAMEPLAY:
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
            case State.DIALOG_MENU:
                DialogPanel.SetActive(false);
                break;
            case State.COMBAT:
                CombatUI.SetActive(false);
                break;
        }
    }

    void EnterState()
    {
        switch (_state)
        {
            case State.GAMEPLAY:
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
            case State.DIALOG_MENU:
                DialogPanel.SetActive(true);
                break;
            case State.COMBAT:
                CombatUI.SetActive(true);
                break;
        }
    }
}
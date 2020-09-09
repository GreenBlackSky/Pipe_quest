using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlayManager : MonoBehaviour
{
    public GameObject GameplayUIPanel;
    public GameObject PauseMenuPanel;
    public GameObject InventoryPanel;
    public GameObject SkillMenuPanel;
    public GameObject MapMenuPanel;


    public enum State { GAMEPLAY, PAUSE_MENU, INVENTORY, SKILL_MENU, MAP_MENU }
    State _state = State.GAMEPLAY;

    void SwitchState(State state)
    {
        LeaveState(_state);
        _state = state;
        EnterState(_state);
    }

    void Start()
    {

    }

    void Update()
    {

    }

    void LeaveState(State state)
    {
        switch (_state)
        {
            case State.GAMEPLAY:
                GameplayUIPanel.SetActive(false);
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
        }
    }

    void EnterState(State state)
    {
        switch (_state)
        {
            case State.GAMEPLAY:
                GameplayUIPanel.SetActive(true); 
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
        }
    }
}
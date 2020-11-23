using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public GameObject MainMenuPanel;
    public GameObject GameplayUI;
    public GameObject PauseMenuPanel;
    public GameObject InventoryPanel;
    public GameObject DialogPanel;
    public GameObject SkillMenuPanel;
    public GameObject MapMenuPanel;
    public GameObject CombatUI;
    public GameObject QuestsPanel;
    public GameObject UICamera;

    Dictionary<State, GameObject> _gameMenus;

    public GameObject[] levels;
    GameObject _currentLevel;
    int _currentLevelID = 0;

    public GameObject[] _playerAvatars;
    GameObject _currentAvatar;
    int _currentAvatarID = 0;

    public static GameManager Instance { get; private set; }

    public enum State { 
        GAMEPLAY,
        LOADING,
        MAIN_MENU,
        COMBAT,
        PAUSE_MENU,
        INVENTORY,
        DIALOG,
        SKILL_MENU,
        MAP_MENU,
        JOURNAL_MENU
    }
    State _state;

    public void SwitchState(State state) {
        LeaveState(_state);
        _state = state;
        EnterState();
    }

    void Start() {
        _gameMenus = new Dictionary<State, GameObject>() {
            {State.PAUSE_MENU, PauseMenuPanel},
            {State.INVENTORY, InventoryPanel},
            {State.SKILL_MENU, SkillMenuPanel},
            {State.MAP_MENU, MapMenuPanel},
            {State.DIALOG, DialogPanel},
            {State.COMBAT, CombatUI},
            {State.JOURNAL_MENU, QuestsPanel},
        };
        Instance = this;
        SwitchState(State.MAIN_MENU);
    }

    void Update() {
        if(Input.GetKeyDown(KeyCode.I)) {
            SwitchState(State.INVENTORY);
        } else if (Input.GetKeyDown(KeyCode.J)) {
            SwitchState(State.JOURNAL_MENU);
        } else if (Input.GetKeyDown(KeyCode.Tab)) {
            SwitchState(State.GAMEPLAY);
        } else if (Input.GetKeyDown(KeyCode.Escape)) {
            SwitchState(State.PAUSE_MENU);
        } 
    } 

    void LeaveState(State state) {
        switch (_state) {
            case State.GAMEPLAY:
                Time.timeScale = 0;
                GameplayUI.SetActive(false);
                break;
            case State.MAIN_MENU:
                LeaveMainMenu();
                break;
            default:
                _gameMenus[_state].SetActive(false);
                break;
        }
    }

    void EnterState() {
        switch (_state) {
            case State.GAMEPLAY:
                Time.timeScale = 1;
                GameplayUI.SetActive(true);
                break;
            case State.MAIN_MENU:
                EnterMainMenu();
                break;
            default:
                _gameMenus[_state].SetActive(true);
                break;
        }
    }

    void EnterMainMenu() {
        Destroy(_currentLevel);
        Destroy(_currentAvatar);
        UICamera.SetActive(true);
        MainMenuPanel.SetActive(true);
    }

    void LeaveMainMenu() {
        UICamera.SetActive(false);
        MainMenuPanel.SetActive(false);
        _currentLevel = Instantiate(levels[_currentLevelID]);
        string current_level_name = levels[_currentLevelID].name;
        _currentAvatar = Instantiate(_playerAvatars[_currentAvatarID]);
        LinkAvatar(_currentAvatar);
        SpeakerManager.instance.LoadAllSpeakers(current_level_name);
    }

    void LinkAvatar(GameObject avatar) {
        GameObject interactButton = GameplayUI.transform.Find("InteractButton").gameObject;
        avatar.GetComponent<InteractingHero>().interactionButton = interactButton;
        interactButton.GetComponent<Button>().onClick.AddListener(() => avatar.GetComponent<InteractingHero>().interact());
        avatar.GetComponent<CollectingHero>().InventoryPanel = InventoryPanel;

        QuestDoingHero questHero = avatar.GetComponent<QuestDoingHero>();
        questHero.QuestsUI = QuestsPanel;
        avatar.GetComponent<EventManager>().Init(questHero);
    }
}
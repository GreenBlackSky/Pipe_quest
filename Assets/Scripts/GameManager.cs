using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

    
public class GameManager : MonoBehaviour
{
    public GameObject UICamera;
    public GameObject UICanvas;

    Dictionary<State, GameObject> _gameMenus;
    Dictionary<string, GameObject> _allUIMenus;

    Dictionary<string, string> _allLevels;
    GameObject _currentLevel;
    public string currentLevelID;

    Dictionary<string, string> _allAvatars; // TODO load level configuration from xml
    GameObject _currentAvatar;
    public string currentAvatarID;

    public static GameManager Instance { get; private set; }

    public enum State {
        GAMEPLAY,
        LOADING,  // TODO async loading
        MAIN_MENU,
        COMBAT,
        PUZZLE,
        PAUSE_MENU,
        INVENTORY,
        DIALOG,
        SKILL_MENU,
        MAP_MENU,
        JOURNAL_MENU
    }
    State _state = State.GAMEPLAY;

    public void SwitchState(State state) {
        LeaveState();
        _state = state;
        EnterState();
    }

    void Start() {
        PrepareUI();
        _allLevels = GetResourcesPaths("Assets/Levels/");
        _allAvatars = GetResourcesPaths("Assets/Avatars/");
        Instance = this;
        SwitchState(State.MAIN_MENU);
    }

    void Update() {
        // BUG keys in main menu
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

    void LeaveState() {
        switch (_state) {
            case State.GAMEPLAY:
                Time.timeScale = 0;
                _allUIMenus["GameplayUI"].SetActive(false);
                break;
            case State.MAIN_MENU:
                LeaveMainMenu();
                LoadLevel();
                LoadAvatar();
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
                _allUIMenus["GameplayUI"].SetActive(true);
                break;
            case State.MAIN_MENU:
                Destroy(_currentLevel);
                Destroy(_currentAvatar);
                EnterMainMenu();
                break;
            default:
                _gameMenus[_state].SetActive(true);
                break;
        }
    }

    Dictionary<string, string> GetResourcesPaths(string resourcePath) {
        Dictionary<string, string> ret = new Dictionary<string, string>();
        List<string> paths = new List<string>(Directory.GetFiles(@"" + resourcePath));
        foreach(string path in paths) {
            if(path.EndsWith("meta")) {
                continue;
            }
            string[] pathParts = path.Split('/');
            string name = pathParts[pathParts.Length - 1].Split('.')[0];
            ret[name] = path;
        }
        return ret;
    }

    void PrepareUI() {
        _allUIMenus = new Dictionary<string, GameObject>();
        List<string> paths = new List<string>(Directory.GetFiles(@"" + "Assets/UI/"));
        foreach(string path in paths) {
            if(path.EndsWith("meta")) {
                continue;
            }
            UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath(path, typeof(GameObject));
            string[] pathParts = path.Split('/');
            string name = pathParts[pathParts.Length - 1].Split('.')[0];
            _allUIMenus[name] = Instantiate(prefab, UICanvas.transform, false) as GameObject;  
        }

        _gameMenus = new Dictionary<State, GameObject>() {
            {State.PAUSE_MENU, _allUIMenus["PauseMenuPanel"]},
            {State.INVENTORY, _allUIMenus["InventoryPanel"]},
            {State.SKILL_MENU, _allUIMenus["SkillMenuPanel"]},
            {State.MAP_MENU, _allUIMenus["MapMenuPanel"]},
            {State.DIALOG, _allUIMenus["DialogPanel"]},
            {State.COMBAT, _allUIMenus["CombatUI"]},
            {State.JOURNAL_MENU, _allUIMenus["QuestsPanel"]},
        };
    }

    void EnterMainMenu() {        
        UICamera.SetActive(true);
        _allUIMenus["MainMenuPanel"].SetActive(true);
    }

    void LeaveMainMenu() {
        UICamera.SetActive(false);
        _allUIMenus["MainMenuPanel"].SetActive(false);
    }

    void LoadLevel() {
        UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath(_allLevels[currentLevelID], typeof(GameObject));
        _currentLevel =  Instantiate(prefab) as GameObject;
        DialogueManager.LoadAllSpeakers(currentLevelID);
        QuestManager.Init(currentLevelID);
    }

    void LoadAvatar() {
        
        UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath(_allAvatars[currentAvatarID], typeof(GameObject));
        _currentAvatar = Instantiate(prefab) as GameObject;

        GameObject interactButton = _allUIMenus["GameplayUI"].transform.Find("InteractButton").gameObject;
        CollectingHero itemsHero = _currentAvatar.GetComponent<CollectingHero>();
        QuestDoingHero questHero = _currentAvatar.GetComponent<QuestDoingHero>();

        _currentAvatar.GetComponent<InteractingHero>().interactionButton = interactButton;
        interactButton.GetComponent<Button>().onClick.AddListener(() => _currentAvatar.GetComponent<InteractingHero>().interact());
        itemsHero.InventoryPanel = _allUIMenus["InventoryPanel"];
        questHero.QuestsUI = _allUIMenus["QuestsPanel"];

        EventManager.Init(currentLevelID, questHero, itemsHero);
    }
}
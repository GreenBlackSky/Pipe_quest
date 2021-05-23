using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

    
public class GameManager : MonoBehaviour {

    Dictionary<string, string> _allLevels;
    GameObject _currentLevel;

    public static GameManager Instance { get; private set; }

    // TODO cut scenes
    // TODO notifications
    // TODO async loading
    public enum State {
        GREETING,
        LOADING,
        EXPLORATION,
        COMBAT,
        PUZZLE,
        MAIN_MENU,
        CUTSCENE,
    }

    State _state = State.MAIN_MENU;

    public void SwitchState(State state) {

    }

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    void Start() {
        Instance = this;
        SwitchState(State.GREETING);
    }

    void Update() {
    }

    // void _processGameplayInput() {
    //     if(Input.GetKeyDown(KeyCode.I)) {
    //         SwitchState(State.INVENTORY);
    //     } else if (Input.GetKeyDown(KeyCode.J)) {
    //         SwitchState(State.JOURNAL_MENU);
    //     } else if (Input.GetKeyDown(KeyCode.Tab)) {
    //         SwitchState(State.GAMEPLAY);
    //     } else if (Input.GetKeyDown(KeyCode.Escape)) {
    //         SwitchState(State.PAUSE_MENU);
    //     } 
    // } 


    void LoadLevel() {
        // UnityEngine.Object prefab = AssetDatabase.LoadAssetAtPath(_allLevels[currentLevelID], typeof(GameObject));
        // _currentLevel =  Instantiate(prefab) as GameObject;
        // DialogueManager.LoadAllSpeakers(currentLevelID);
        // QuestManager.Init(currentLevelID);
    }

    // void LoadLevel(string levelID) {
    //     GameObject prefab = null;
    //     foreach(GameObject level in this.LevelPrefabs) {
    //         if(level.name == levelID) {
    //             prefab = level;
    //             break;
    //         }
    //     }
    //     if(prefab == null) {
    //         throw new System.Exception($"No level {levelID}");
    //     }
    //     this._currentLevel = Instantiate(prefab) as GameObject;
    //     DialogueManager.LoadAllSpeakers(levelID);
    //     QuestManager.Init(levelID);
    // }

    // void LoadAvatar(string avatarType) {
    //     this._currentAvatar = Instantiate(this.AvatarPrefab) as GameObject;
    //     CollectingHero itemsHero = _currentAvatar.GetComponent<CollectingHero>();
    //     QuestDoingHero questHero = _currentAvatar.GetComponent<QuestDoingHero>();

    //     this._currentAvatar.GetComponent<InteractingHero>().interactionButton = this.InteractionButton;
    //     InteractionButton.GetComponent<Button>().onClick.AddListener(() => _currentAvatar.GetComponent<InteractingHero>().interact());
    //     itemsHero.InventoryPanel = this.InventoryPanel;
    //     questHero.QuestsUI = this.QuestsPanel;
    // }
}
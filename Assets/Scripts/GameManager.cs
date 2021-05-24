using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

    
public class GameManager : MonoBehaviour {

    public Scene MainMenu;
    public Scene ForestExploration;
    public Scene AnorakExploration;

    public static GameManager Instance { get; private set; }

    // TODO notifications
    // TODO async loading
    public enum Level {
        GREETING,
        LOADING,
        MAIN_MENU,
        FOREST_EXPLORATION,
        FOREST_COMBAT,
        ANORAK_EXPLORATION,
    }

    // Level _state = Level.MAIN_MENU;

    void Awake() {
        DontDestroyOnLoad(this.gameObject);
        if(GameManager.Instance == null) {
            GameManager.Instance = this;
        }
    }

    void Start() {
        LoadLevel(Level.GREETING);
    }

    void Update() { }

    public void LoadLevel(Level state) {

    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
    public void OnNewGameClicked() {
        GameManager.Instance.SwitchState(GameManager.State.NEW_GAME_MENU);
    }

    public void OnLoadGameClicked() {
        GameManager.Instance.SwitchState(GameManager.State.LOAD_GAME_MENU);
    }

    public void OnSettingsClicked() {
        GameManager.Instance.SwitchState(GameManager.State.SETTINGS_MENU);
    }

    public void OnExitClicked() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

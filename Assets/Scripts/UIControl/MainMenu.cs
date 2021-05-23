using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
    public void OnNewGameClicked() {
        MainMenuManager.Instance.SwitchState(MainMenuManager.State.NEW_GAME_MENU);
    }

    public void OnLoadGameClicked() {
        MainMenuManager.Instance.SwitchState(MainMenuManager.State.LOAD_GAME_MENU);
    }

    public void OnSettingsClicked() {
        MainMenuManager.Instance.SwitchState(MainMenuManager.State.SETTINGS_MENU);
    }

    public void OnExitClicked() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

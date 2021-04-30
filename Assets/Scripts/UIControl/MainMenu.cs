using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {
    public void OnNewGameClicked() {
        GameManager.Instance.SwitchState(GameManager.State.GAMEPLAY);
    }
    public void OnLoadGameClicked() {}

    public void OnOptionsClicked() {}

    public void OnExitClicked() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

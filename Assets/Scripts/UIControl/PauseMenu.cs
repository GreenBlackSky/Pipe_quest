using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void OnContinueClicked() {
        GameManager.Instance.SwitchState(GameManager.State.GAMEPLAY);
    }
    public void OnExitClicked() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}

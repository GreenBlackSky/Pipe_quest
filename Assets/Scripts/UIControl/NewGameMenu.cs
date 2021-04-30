using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewGameMenu : MonoBehaviour {
    public void OnNewGame1Clicked() {
        GameManager.Instance.SwitchState(GameManager.State.GAMEPLAY);
    }

    public void OnNewGame2Clicked() {
        GameManager.Instance.SwitchState(GameManager.State.GAMEPLAY);
    }

    public void OnNewGame3Clicked() {
        GameManager.Instance.SwitchState(GameManager.State.GAMEPLAY);
    }

    public void OnBackClicked() {
        GameManager.Instance.SwitchState(GameManager.State.MAIN_MENU);
    }
}

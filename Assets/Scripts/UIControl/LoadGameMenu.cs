using UnityEngine;

public class LoadGameMenu : MonoBehaviour {
    public void OnNewGame1Clicked() {
        // GameManager.Instance.SwitchState(GameManager.State.EXPLORATION);
    }

    public void OnNewGame2Clicked() {
        // GameManager.Instance.SwitchState(GameManager.State.EXPLORATION);
    }

    public void OnNewGame3Clicked() {
        // GameManager.Instance.SwitchState(GameManager.State.EXPLORATION);
    }

    public void OnBackClicked() {
        MainMenuManager.Instance.SwitchState(MainMenuManager.State.MAIN_MENU);
    }
}

using UnityEngine;

public class NewGameMenu : MonoBehaviour {
    public void OnNewGame1Clicked() {
        GameManager.Instance.LoadLevel(GameManager.Level.FOREST_EXPLORATION);
    }

    public void OnNewGame2Clicked() {
        GameManager.Instance.LoadLevel(GameManager.Level.FOREST_EXPLORATION);
    }

    public void OnNewGame3Clicked() {
        GameManager.Instance.LoadLevel(GameManager.Level.FOREST_EXPLORATION);
    }

    public void OnBackClicked() {
        MainMenuManager.Instance.SwitchState(MainMenuManager.State.MAIN_MENU);
    }
}

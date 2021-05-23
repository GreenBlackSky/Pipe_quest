using UnityEngine;

public class SettingsMenu : MonoBehaviour {
    public void OnScreenClicked() {
        MainMenuManager.Instance.SwitchState(MainMenuManager.State.SCREEN_SETTINGS);
    }

    public void OnAudioClicked() {
        MainMenuManager.Instance.SwitchState(MainMenuManager.State.AUDIO_SETTINGS);
    }

    public void OnControlClicked() {
        MainMenuManager.Instance.SwitchState(MainMenuManager.State.CONTROLS_SETTINGS);
    }

    public void OnBackClicked() {
        MainMenuManager.Instance.SwitchState(MainMenuManager.State.MAIN_MENU);
    }
}

using UnityEngine;

public class SettingsMenu : MonoBehaviour {
    public void OnScreenClicked() {
        GameManager.Instance.SwitchState(GameManager.State.SCREEN_SETTINGS);
    }

    public void OnAudioClicked() {
        GameManager.Instance.SwitchState(GameManager.State.AUDIO_SETTINGS);
    }

    public void OnControlClicked() {
        GameManager.Instance.SwitchState(GameManager.State.CONTROLS_SETTINGS);
    }

    public void OnBackClicked() {
        GameManager.Instance.SwitchState(GameManager.State.MAIN_MENU);
    }
}

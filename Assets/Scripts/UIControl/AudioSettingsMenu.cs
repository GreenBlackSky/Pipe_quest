using UnityEngine;

public class AudioSettingsMenu : MonoBehaviour {
    public void OnBackClicked() {
        MainMenuManager.Instance.SwitchState(MainMenuManager.State.SETTINGS_MENU);
    }
}

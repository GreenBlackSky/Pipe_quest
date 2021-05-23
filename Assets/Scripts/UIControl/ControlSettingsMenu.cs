using UnityEngine;

public class ControlSettingsMenu : MonoBehaviour {
    public void OnBackClicked() {
        MainMenuManager.Instance.SwitchState(MainMenuManager.State.SETTINGS_MENU);
    }
}

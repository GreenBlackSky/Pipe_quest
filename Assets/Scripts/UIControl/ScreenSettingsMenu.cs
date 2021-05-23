using UnityEngine;

public class ScreenSettingsMenu : MonoBehaviour {
    public void OnBackClicked() {
        MainMenuManager.Instance.SwitchState(MainMenuManager.State.SETTINGS_MENU);
    }
}

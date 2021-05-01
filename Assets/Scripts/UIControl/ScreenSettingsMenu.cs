using UnityEngine;

public class ScreenSettingsMenu : MonoBehaviour {
    public void OnBackClicked() {
        GameManager.Instance.SwitchState(GameManager.State.SETTINGS_MENU);
    }
}

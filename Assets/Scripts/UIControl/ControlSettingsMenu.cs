using UnityEngine;

public class ControlSettingsMenu : MonoBehaviour {
    public void OnBackClicked() {
        GameManager.Instance.SwitchState(GameManager.State.SETTINGS_MENU);
    }
}

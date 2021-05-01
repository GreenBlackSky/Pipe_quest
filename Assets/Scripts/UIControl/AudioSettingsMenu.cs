using UnityEngine;

public class AudioSettingsMenu : MonoBehaviour {
    public void OnBackClicked() {
        GameManager.Instance.SwitchState(GameManager.State.SETTINGS_MENU);
    }
}

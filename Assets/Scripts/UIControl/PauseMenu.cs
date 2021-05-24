
using UnityEngine;
using static GameManager;

public class PauseMenu : MonoBehaviour
{
    public void OnContinueClicked() {
        // GameManager.Instance.SwitchState(GameManager.State.EXPLORATION);
    }
    public void OnExitClicked() {
        GameManager.Instance.LoadLevel(GameManager.Level.MAIN_MENU);
    }
}

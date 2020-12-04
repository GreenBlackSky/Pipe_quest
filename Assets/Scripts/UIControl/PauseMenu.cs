
using UnityEngine;
using static GameManager;

public class PauseMenu : MonoBehaviour
{
    public void OnContinueClicked() {
        GameManager.Instance.SwitchState(GameManager.State.GAMEPLAY);
    }
    public void OnExitClicked() {
        GameManager.Instance.SwitchState(GameManager.State.MAIN_MENU);
    }
}

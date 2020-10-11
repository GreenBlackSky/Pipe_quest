using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractButton : MonoBehaviour
{
    public Button _button;

    void Update()
    {
        if(Input.GetKeyUp(KeyCode.F))
        {
            _button.onClick.Invoke();
        }
    }
}

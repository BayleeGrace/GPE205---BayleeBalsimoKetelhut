using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPressToMainMenu : MonoBehaviour
{
    public void ChangeToMainMenu()
    {
        if (GameManager.instance != null) 
        {
            GameManager.instance.DeactiveGameplayState();
            GameManager.instance.ActivateMainMenuScreen();
        }
    }
}

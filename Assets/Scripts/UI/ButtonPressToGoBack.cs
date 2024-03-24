using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPressToGoBack : MonoBehaviour
{
    public void ChangeToPauseMenu()
    {
        if (GameManager.instance != null) 
        {
            GameManager.instance.ActivatePauseMenuScreen();
        }
    }
}

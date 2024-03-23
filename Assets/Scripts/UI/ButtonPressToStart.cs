using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPressToStart : MonoBehaviour
{
    public void ChangeToGameplay()
    {
        if (GameManager.instance != null) 
        {
            GameManager.instance.ActivateGameplay();
        }
    }

    public void DeactivatePauseState()
    {
        if (GameManager.instance != null) 
        {
            GameManager.instance.DeactivatePauseMenuState();
        }
    }
}

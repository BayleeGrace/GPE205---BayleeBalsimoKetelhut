using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPressToCredits : MonoBehaviour
{
    public void ChangeToCredits()
    {
        if (GameManager.instance != null) 
        {
            GameManager.instance.ActivateCreditsScreen();
        }
    }
}

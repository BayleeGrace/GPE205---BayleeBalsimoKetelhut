using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPressForOptions : MonoBehaviour
{
    public void ChangeToOptions()
    {
        if (GameManager.instance != null) {
            GameManager.instance.ActivateOptionsScreen();
        }
    }

    public void ShowSountOptions()
    {
        // Hide current options (if applicable)
        // Set sound options to visible
    }

    public void ShowGameplayOptions()
    {
        // Hide current options (if applicable)
        // Set gameplay options to visible
    }

    public void ShowVideoOptions()
    {
        // Hide current options (if applicable)
        // Set video options to visible
    }
}

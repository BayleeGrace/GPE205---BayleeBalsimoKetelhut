using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPressToPauseOptions : MonoBehaviour
{
    public GameObject soundOptionsScreen;
    public GameObject graphicsOptionsScreen;

    public void ChangeToOptions()
    {
        if (GameManager.instance != null) 
        {
            GameManager.instance.ActivatePauseMenuOptionsScreen();
            ShowSoundOptions();
        }
    }

    private void HideAllOptions()
    {
        soundOptionsScreen.SetActive(false);
        graphicsOptionsScreen.SetActive(false);
    }

    public void ShowSoundOptions()
    {
        // Hide current options (if applicable)
        HideAllOptions();
        // Set sound options to visible
        soundOptionsScreen.SetActive(true);
    }

    public void ShowVideoOptions()
    {
        // Hide current options (if applicable)
        HideAllOptions();
        // Set video options to visible
        graphicsOptionsScreen.SetActive(true);
    }

    public void ChangeToPauseMenuScreen()
    {
        if (GameManager.instance != null) 
        {
            GameManager.instance.ActivatePauseMenuScreen();
        }
    }
}

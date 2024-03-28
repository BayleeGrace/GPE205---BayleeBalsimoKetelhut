using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPressForOptions : MonoBehaviour
{
    public GameObject soundOptionsScreen;
    public GameObject gameplayOptionsScreen;
    public GameObject graphicsOptionsScreen;

    public void ChangeToOptions()
    {
        if (GameManager.instance != null) 
        {
            GameManager.instance.ActivateOptionsScreen();
            ShowSoundOptions();
        }
    }

    private void HideAllOptions()
    {
        soundOptionsScreen.SetActive(false);
        gameplayOptionsScreen.SetActive(false);
        graphicsOptionsScreen.SetActive(false);
    }

    public void ShowSoundOptions()
    {
        // Hide current options (if applicable)
        HideAllOptions();
        // Set sound options to visible
        soundOptionsScreen.SetActive(true);
    }

    public void ShowGameplayOptions()
    {
        // Hide current options (if applicable)
        HideAllOptions();
        // Set gameplay options to visible
        gameplayOptionsScreen.SetActive(true);
    }

    public void ShowVideoOptions()
    {
        // Hide current options (if applicable)
        HideAllOptions();
        // Set video options to visible
        graphicsOptionsScreen.SetActive(true);
    }

    public void ChangeToMainMenuScreen()
    {
        if (GameManager.instance != null) 
        {
            GameManager.instance.ActivateMainMenuScreen();
        }
    }

}

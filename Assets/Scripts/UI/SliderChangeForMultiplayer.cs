using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderChangeForMultiplayer : MonoBehaviour
{

    public Toggle toggle;

    public void OnSliderChange()
    {
        GameManager.instance.isMultiplayer = true;
    }

    public void OnToggleChange()
    {
        if (toggle.isOn)
        {
            GameManager.instance.isMultiplayer = true;
        }
        else if (!toggle.isOn)
        {
            GameManager.instance.isMultiplayer = false;
        }
    }
}

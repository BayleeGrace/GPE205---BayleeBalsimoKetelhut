using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderChangeForMultiplayer : MonoBehaviour
{
    public void OnSliderChange()
    {
        GameManager.instance.isMultiplayer = true;
    }

    public void OnToggleChange()
    {
        GameManager.instance.isMultiplayer = true;
    }
}

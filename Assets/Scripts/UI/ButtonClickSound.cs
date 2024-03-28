using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClickSound : MonoBehaviour
{
    #region Sound variables
    public AudioSource buttonClickedAudioSource;
    public AudioClip buttonClickedAudioClip;
    #endregion Sound variables

    // Start is called before the first frame update
    public void PlaySound()
    {
        buttonClickedAudioSource.PlayOneShot(buttonClickedAudioClip);
    }

}

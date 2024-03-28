using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePickup : Pickup
{
    public ScorePowerup powerup;
    #region Sound variables
    public AudioSource deathAudioSource;
    public AudioClip deathAudioClip;
    #endregion Sound variables

    public void Awake()
    {
        PlayDeathSound();
    }
    
    public override void OnTriggerEnter(Collider other)
    {
        // store other colliding object's powerupManager
        PowerupManager powerupManager = other.GetComponent<PowerupManager>();

        if (powerupManager != null)
        {
            // Add the powerup
            powerupManager.Add(powerup);

            // Destroy this pickup
            Destroy(gameObject);
        }
    }

    public void PlayDeathSound()
    {
        //AudioSource.PlayClipAtPoint(tankRotateClip, gameObject.transform.position);
        deathAudioSource.PlayOneShot(deathAudioClip);
    }
}

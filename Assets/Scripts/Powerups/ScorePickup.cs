using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePickup : Pickup
{
    public ScorePowerup powerup;
    
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
}

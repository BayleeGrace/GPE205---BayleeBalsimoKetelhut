using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreasedDamagePickup : Pickup
{
    public IncreasedDamagePowerup powerup;

    // Start is called before the first frame update
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

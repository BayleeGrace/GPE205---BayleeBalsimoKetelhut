using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // this lets us access THIS data from the powerup

public class HealthPowerup : Powerup
{
    public float healthToAdd;

    public override void Apply(PowerupManager target)
    {
        // TODO: Apply health changes
        // Get the health component
        Health targetHealth = target.GetComponent<Health>();

        // Check if there is a health component
        if (targetHealth != null)
        {
            targetHealth.ReplenishHealth(healthToAdd, target.GetComponent<Pawn>());
        }
    }

    public override void Remove(PowerupManager target)
    {
        // TODO: Remove health changes
        // Get the health component
        Health targetHealth = target.GetComponent<Health>();

        // Check if there is a health component
        if (targetHealth = null)
        {
            targetHealth.TakeDamage(healthToAdd, target.GetComponent<Pawn>());
        }
    }
}

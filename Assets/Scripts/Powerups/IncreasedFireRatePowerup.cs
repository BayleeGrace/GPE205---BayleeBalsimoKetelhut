using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // this lets us access THIS data from the powerup

public class IncreasedFireRatePowerup : Powerup
{
    public float fireRateMultiplier;

    public override void Apply(PowerupManager target)
    {
        TankPawn tankPawn = target.GetComponent<TankPawn>();

        // if the colliding object has a shooter component
        if(tankPawn != null)
        {
            float newFireRate = tankPawn.fireRate;

            // reference the "DamageOnHit" component and increase damage done base on the nultiplier
            newFireRate = newFireRate * fireRateMultiplier;
            Debug.Log("Fire Rate is now " + newFireRate);
        }
    }

    public override void Remove(PowerupManager target)
    {
        TankPawn tankPawn = target.GetComponent<TankPawn>();

        // if the colliding object has a shooter component
        if(tankPawn != null)
        {
            float newFireRate = tankPawn.fireRate;
            
            newFireRate = newFireRate * 1;
            Debug.Log("Fire Rate is now " + newFireRate);
        }
    }
}

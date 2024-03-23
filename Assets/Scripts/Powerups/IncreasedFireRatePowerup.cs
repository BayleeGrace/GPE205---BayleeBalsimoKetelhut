using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // this lets us access THIS data from the powerup

public class IncreasedFireRatePowerup : Powerup
{
    public float fireRateMultiplier;
    private float newFireRate;

    public override void Apply(PowerupManager target)
    {
        TankPawn targetPawn = target.GetComponent<TankPawn>();

        newFireRate = targetPawn.timerDelay * fireRateMultiplier;
        // reference the "DamageOnHit" component and increase damage done base on the nultiplier
        targetPawn.timerDelay = newFireRate;
    }

    public override void Remove(PowerupManager target)
    {
        TankPawn targetPawn = target.GetComponent<TankPawn>();

        targetPawn.timerDelay = newFireRate / fireRateMultiplier;
    }
}

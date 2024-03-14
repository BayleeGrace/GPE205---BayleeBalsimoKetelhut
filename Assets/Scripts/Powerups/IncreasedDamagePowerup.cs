using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // this lets us access THIS data from the powerup

public class IncreasedDamagePowerup : Powerup
{
    public float damageMultiplier;

    public override void Apply(PowerupManager target)
    {
        TankPawn tankPawn = target.GetComponent<TankPawn>();

        // if the colliding object has a shooter component
        if(tankPawn != null)
        {
            float newDamage = tankPawn.damageDone;

            // reference the "DamageOnHit" component and increase damage done base on the nultiplier
            newDamage = newDamage * damageMultiplier;
            Debug.Log("Damage for " + target + "is now " + newDamage);
        }
    }

    public override void Remove(PowerupManager target)
    {
        TankPawn tankPawn = target.GetComponent<TankPawn>();

        // if the colliding object has a shooter component
        if(tankPawn != null)
        {
            float newDamage = tankPawn.damageDone;

            // reference the "DamageOnHit" component and increase damage done base on the nultiplier
            newDamage = newDamage * 1;
            Debug.Log("Damage for " + target + "is now " + newDamage);
        }
    }
}

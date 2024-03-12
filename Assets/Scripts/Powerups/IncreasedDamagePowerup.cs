using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // this lets us access THIS data from the powerup

public class IncreasedDamagePowerup : Powerup
{
    public float damageMultiplier;

    public override void Apply(PowerupManager target)
    {
        DamageOnHit damageOnHit = target.GetComponent<DamageOnHit>();

        // if the colliding object has a shooter component
        if(damageOnHit != null)
        {
            // reference the "DamageOnHit" component and increase damage done base on the nultiplier
            damageOnHit.damageDone = damageOnHit.damageDone * damageMultiplier;
        }
    }

    public override void Remove(PowerupManager target)
    {

    }
}

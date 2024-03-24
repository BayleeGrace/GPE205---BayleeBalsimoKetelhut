using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable] // this lets us access THIS data from the powerup

public class ScorePowerup : Powerup
{
    public int scoreToAdd;

    public override void Apply(PowerupManager target)
    {
        Pawn targetPawn = target.GetComponent<Pawn>();

        if (targetPawn != null)
        {
            targetPawn.controller.AddToScore(scoreToAdd);
        }
    }

    public override void Remove(PowerupManager target)
    {
        if (isPermanentPowerup == false)
        {
            Pawn targetPawn = target.GetComponent<Pawn>();

            if (targetPawn != null)
            {
                targetPawn.controller.AddToScore(-scoreToAdd);
            }
        }
    }
}

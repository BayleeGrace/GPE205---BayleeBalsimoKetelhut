using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankerAIController : AIController
{

    #region Variables;
    private Health health; // Tracks this AI's health to trigger the enraged state
    public float healthToEnrage;
    public float enragedMoveSpeed; // Move speed multiplier when enraged
    public float enragedFireRate; // Fire rate multiplier when enraged
    #endregion Variables;
    
    public override void Start()
    {
        health = pawn.GetComponent<Health>();
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void ProcessInputs()
    {
        base.ProcessInputs();
        if (IsHasTarget())
        {
            switch(currentState) // Switch changes states
            {
                case AIState.Chase:
                    DoChaseState();
                    currentState = AIState.Chase;
                    // Check for transitions
                    if (IsInLineOfSight(targetPlayer) && IsCanSee(targetPlayer) && IsDistanceLessThan(targetPlayer, 8/*change to variable after testing*/))
                    {
                        ChangeState(AIState.Attack);
                    }
                    else if (targetPlayer == null || !IsDistanceLessThan(targetPlayer, hearingDistance))
                    {
                        ChangeState(AIState.Patrol);
                    }
                    break;

                case AIState.Attack:
                    base.DoAttackState();
                    currentState = AIState.Attack;
                    if (health.currentHealth <= healthToEnrage)
                    {
                        ChangeState(AIState.Enraged);
                    }
                    else if (!IsInLineOfSight(targetPlayer))
                    {
                        ChangeState(AIState.Chase);
                    }
                    else if(!IsDistanceLessThan(targetPlayer, hearingDistance) || !IsCanSee(targetPlayer))
                    {
                        ChangeState(AIState.Patrol);
                    }
                    break;

                case AIState.Enraged:
                    DoEnragedState();
                    currentState = AIState.Enraged;
                    if(health.currentHealth > healthToEnrage)
                    {
                        ChangeState(AIState.Attack);
                    }
                    break;
            }
        }
        else if (GameManager.instance.players != null)
        {
            TargetPlayerOne();
        }
        else if (GameManager.instance.players == null)
        {
            ChangeState(AIState.Patrol);
        }

    }

    public override void DoEnragedState()
    {
        if (targetPlayer != null)
        {
            Enrage();
            // Chase
            Seek(targetPlayer);
            // Shoot
            pawn.Shoot();
        }
        else if (targetPlayer = null)
        {
            ChangeState(AIState.Patrol);
        }
    }

    public void Enrage()
    {
        pawn.moveSpeed = enragedMoveSpeed;
        pawn.fireRate = enragedFireRate;
    }

}

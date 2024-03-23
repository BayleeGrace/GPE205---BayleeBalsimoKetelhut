using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffensiveAIController : AIController
{
    
    // Offensive variables
    public float distanceUntilChase;
    public float distanceUntilAttack;
    
    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void ProcessInputs() // this is overriding the Controller "ProcessInputs()"
    {
        base.ProcessInputs();
        if (IsHasTarget())
        {
            switch(currentState) // Switch changes states
            {
                case AIState.Chase:
                    if(IsHasTarget())
                    {
                        DoChaseState();
                        currentState = AIState.Chase;
                    }
                    else
                    {
                        TargetPlayerOne();
                    }
                    // Check for transitions
                    if(IsInLineOfSight(targetPlayer) && IsCanSee(targetPlayer) && IsDistanceLessThan(targetPlayer, 8/*change to variable after testing*/))
                    {
                        ChangeState(AIState.Attack);
                    }
                    else if(targetPlayer == null || !IsDistanceLessThan(targetPlayer, hearingDistance))
                    {
                        ChangeState(AIState.Patrol);
                    }
                    break;

                case AIState.Patrol:
                    base.DoPatrolState();
                    currentState = AIState.Patrol;
                    // Check for transitions
                    if(IsCanHear(targetPlayer) || IsCanSee(targetPlayer) || (IsCanHear(targetPlayer) && IsCanSee(targetPlayer)))
                    {
                        ChangeState(AIState.Chase);
                    }
                    break;

                case AIState.Attack:
                    base.DoAttackState();
                    currentState = AIState.Attack;
                    // Check for transitions
                    if (!IsInLineOfSight(targetPlayer))
                    {
                        ChangeState(AIState.Chase);
                    }
                    else if(!IsDistanceLessThan(targetPlayer, hearingDistance) || !IsCanSee(targetPlayer))
                    {
                        ChangeState(AIState.Patrol);
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

    // Chase State

    public override void DoChaseState()
    {
        base.DoChaseState();
        /*// Seek our target
        if (targetPlayer != null)
        {
        Seek(targetPlayer);
        if(IsDistanceLessThan(targetPlayer, 9))
        {
            ChangeState(AIState.Attack);
        }
        }*/
    }

    // Attack State
    
    public override void DoAttackState()
    {
        base.DoAttackState();
        /*if (targetPlayer != null)
        {
            // Chase
            Seek(targetPlayer);
            // Shoot
            pawn.Shoot();
        }
        else if (targetPlayer == null)
        {
            ChangeState(AIState.Patrol);
        }*/
    }
}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffensiveAIController : AIController
{
    
    // Offensive variables
    public float distanceUntilChase;
    public float distanceUntilAttack;
    
    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void ProcessInputs() // this is overriding the Controller "ProcessInputs()"
    {
        
        switch(currentState) // Switch changes states
        {
            case AIState.Idle:
                // Do work for the Idle state
                DoIdleState();
                currentState = AIState.Idle;
                //Check for any transitions
                if(IsDistanceLessThan(target, distanceUntilChase) && IsCanHear(target))
                {
                    ChangeState(AIState.Chase);
                }
                break;
                // break; is important because it will only execute the "Idle" state before executing the other states.

            case AIState.Chase:
                if(IsHasTarget())
                {
                DoChaseState();
                }
                else
                {
                    TargetPlayerOne();
                }
                currentState = AIState.Chase;
                // Check for transitions
                if(IsDistanceLessThan(target, distanceUntilAttack))
                {
                    ChangeState(AIState.Attack);
                }
                else if(!IsDistanceLessThan(target, distanceUntilChase))
                {
                    ChangeState(AIState.Patrol);
                }
                break;

            case AIState.Patrol:
                DoPatrolState();
                currentState = AIState.Patrol;
                // Check for transitions
                if(IsCanSee(target) && IsCanHear(target))
                {
                    ChangeState(AIState.Chase);
                }
                break;

            case AIState.Attack:
                DoAttackState();
                currentState = AIState.Attack;
                //Check for transitions
                if (!IsDistanceLessThan(target, distanceUntilAttack))
                {
                    ChangeState(AIState.Chase);
                }
                break;

        }

    }

    // Chase State

    public override void DoChaseState()
    {
        base.DoChaseState();
        // Seek our target
        if (target != null)
        {
        Seek(target);
        if(IsDistanceLessThan(target, 9))
        {
            ChangeState(AIState.Attack);
        }
        }
    }

    // Attack State
    
    public override void DoAttackState()
    {
        base.DoAttackState();
        if (target != null)
        {
            // Chase
            Seek(target);
            // Shoot
            pawn.Shoot();
        }
        else if (target == null)
        {
            ChangeState(AIState.Patrol);
        }
    }
}

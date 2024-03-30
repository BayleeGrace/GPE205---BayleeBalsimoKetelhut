using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FleeingAIController : AIController
{
    
    public float fleeDistance;

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

    public override void ProcessInputs()
    {
        if (IsHasTarget())
        {
            switch(currentState) // Switch changes states
            {
                case AIState.Idle:
                    // Do work for the Idle state
                    base.DoIdleState();
                    currentState = AIState.Idle;
                    //Check for any transitions
                    if(IsCanHear(targetPlayer) || IsCanSee(targetPlayer) || (IsCanHear(targetPlayer) && IsCanSee(targetPlayer)))  
                    {
                        ChangeState(AIState.Flee);
                    }   
                    break;
                    // break; is important because it will only execute the "Idle" state before executing the other states.

                case AIState.Patrol:
                    base.DoPatrolState();
                    currentState = AIState.Patrol;
                    // Check for transitions
                    if(IsCanHear(targetPlayer) || IsCanSee(targetPlayer) || (IsCanHear(targetPlayer) && IsCanSee(targetPlayer)))  
                    {
                        ChangeState(AIState.Flee);
                    }
                    break;

                case AIState.Flee:
                    if(IsHasTarget())
                    {
                        DoFleeState();
                    }
                    else
                    {
                        TargetPlayerOne();
                    }
                    currentState = AIState.Flee;
                    // Check for transitions
                    if(targetPlayer == null || (!IsCanHear(targetPlayer) && !IsCanSee(targetPlayer)))  
                    {
                        ChangeState(AIState.Flee);
                    }
                    {
                        ChangeState(AIState.Patrol);
                    }
                    break;
            }
        }
        else if ((GameManager.instance.players.Count > 0) || IsHasTarget() == false)
        {
            if (GameManager.instance.players[0] != null)
            {
                base.TargetPlayerOne();
            }
            else if (GameManager.instance.players[1] != null)
            {
                base.TargetPlayerTwo();
            }
        }
        else if (GameManager.instance.players.Count <= 0)
        {
            ChangeState(AIState.Patrol);
        }
    }

    public override void DoFleeState()
    {
        base.DoFleeState();
        // Find the Vector to our target
        Vector3 vectorToTarget = targetPlayer.transform.position - pawn.transform.position;
        // Find the Vector away from our target by multiplying by -1
        Vector3 vectorAwayFromTarget = -vectorToTarget;
        // Find the vector we would travel down in order to flee
        Vector3 fleeVector = vectorAwayFromTarget.normalized * fleeDistance;
        // Seek the point that is "fleeVector" away from our current position
        Seek(pawn.transform.position + fleeVector);
    }
    
}

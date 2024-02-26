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
        base.ProcessInputs();
        switch(currentState) // Switch changes states
        {
            case AIState.Idle:
                // Do work for the Idle state
                DoIdleState();
                currentState = AIState.Idle;
                //Check for any transitions
                if(IsDistanceLessThan(target, fleeDistance))
                {
                    ChangeState(AIState.Flee);
                }
                break;
                // break; is important because it will only execute the "Idle" state before executing the other states.

            case AIState.Patrol:
                DoPatrolState();
                currentState = AIState.Patrol;
                // Check for transitions
                if(IsDistanceLessThan(target, fleeDistance))   
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
                if(!IsDistanceLessThan(target, fleeDistance))
                {
                    ChangeState(AIState.Patrol);
                }
                break;
        }
    }

    protected void DoFleeState()
    {
        // Find the Vector to our target
        Vector3 vectorToTarget = target.transform.position - pawn.transform.position;
        // Find the Vector away from our target by multiplying by -1
        Vector3 vectorAwayFromTarget = -vectorToTarget;
        // Find the vector we would travel down in order to flee
        Vector3 fleeVector = vectorAwayFromTarget.normalized * fleeDistance;
        // Seek the point that is "fleeVector" away from our current position
        Seek(pawn.transform.position + fleeVector);
    }
    
}

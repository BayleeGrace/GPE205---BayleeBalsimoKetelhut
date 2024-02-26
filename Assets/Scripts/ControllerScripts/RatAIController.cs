using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatAIController : AIController
{

    public Transform[] hidingSpots;
    public float hidingSpotStopDistance;
    public float timeUntilLeavePost;
    private int currentHidingSpot = 0;
    private Transform nearestHidingSpot;

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
                if(IsCanHear(target))
                {
                    ChangeState(AIState.Flee);
                }
                break;

            case AIState.Hide:
                DoHideState();
                currentState = AIState.Hide;
                // Check for transitions
                if(!IsDistanceLessThan(target, 7))   
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
                else
                {
                    ChangeState(AIState.Hide);
                }
                break;
        }

    }

    public override void DoPatrolState()
    {
        base.DoPatrolState();
    }

    public override void DoFleeState()
    {
        base.DoFleeState();
    }

    public override void DoHideState()
    {
        base.DoHideState();
        if (hidingSpots.Length > currentHidingSpot)
            {
                // Then seek that waypoint
                Seek(hidingSpots[currentHidingSpot]);
                if (Vector3.Distance(pawn.transform.position, hidingSpots[currentHidingSpot].position) < hidingSpotStopDistance)
                {
                    currentHidingSpot++;
                }
            }
            else
            {
                RestartHidingSpots();
            }
    }

    public void RestartHidingSpots()
    {
        currentHidingSpot = 0;
    }

}

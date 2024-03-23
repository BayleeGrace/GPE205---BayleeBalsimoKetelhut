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
        if (IsHasTarget())
        {
            switch(currentState) // Switch changes states
            {
                case AIState.Patrol:
                    base.DoPatrolState();
                    currentState = AIState.Patrol;
                    // Check for transitions
                    if(IsCanHear(targetPlayer) || IsCanSee(targetPlayer) || (IsCanHear(targetPlayer) && IsCanSee(targetPlayer)))
                    {
                        ChangeState(AIState.Flee);
                    }
                    break;

                case AIState.Hide:
                    DoHideState();
                    currentState = AIState.Hide;
                    // Check for transitions
                    if(!IsDistanceLessThan(targetPlayer, 7))   
                    {
                        ChangeState(AIState.Flee);
                    }            
                    break;

                case AIState.Flee:
                    DoFleeState();
                    currentState = AIState.Flee;
                    // Check for transitions
                    if(!IsCanHear(targetPlayer))
                    {
                        ChangeState(AIState.Patrol);
                    }
                    else if (!IsDistanceLessThan(targetPlayer, fleeDistance))
                    {
                        ChangeState(AIState.Hide);
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

    public override void DoPatrolState()
    {
        base.DoPatrolState();
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

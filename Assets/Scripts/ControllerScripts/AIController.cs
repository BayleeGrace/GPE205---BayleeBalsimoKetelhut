using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    // An enum is less taxing on the system like an int, but gives the string to other programmers to understand.
    // Each str is being stored as a numeric value (Guard = 1, Chase = 2, etc.)
    public enum AIState { Idle, Chase, Flee, Patrol, Attack, Scan, BackToPost };

    public AIState currentState;

    // Variable for tracking how long it takes for states to change
    private float lastStateChangeTime;
    
    // Start is called before the first frame update
    public override void Start()
    {
        currentState = AIState.Idle;
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        ProcessInputs();
        
        base.Update();
    }

    public override void ProcessInputs()
    {
        Debug.Log("Making Decisions...");
        
        // Switch changes states
        switch(currentState)
        {
            case AIState.Idle:
                // Do work for the Idle state
                DoIdleState();
                //Check for any transitions
                break;
                // break; is important because it will only execute the "Guard" state before executing the other states.

            case AIState.Chase:
                break;

            case AIState.Flee:
                break;

            case AIState.Patrol:
                break;

            case AIState.Attack:
                break;

            case AIState.Scan:
                break;

            case AIState.BackToPost:
                break;
        }
    }

    protected void DoIdleState()
    {
        // Do nothing
    }

    protected void DoChaseState()
    {
        // Seek our target
    }

    public void Seek(GameObject target)
    {
        // This function takes a game object as a target
        // Rotate towards the target

        // Move Forward towards the target

        // 
    }

    public void Seek(Vector3 targetPosition)
    {
        // Do Seek Behavior (seek a certain position)
    }

    protected bool IsDistanceLessThan(GameObject target, float distance)
    {
        // Compare transform distance of two pawns, the owner of this component and target pawn
        if (Vector3.Distance(pawn.transform.position, target.transform.position) < distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public virtual void ChangeState(AIState newState)
    {
        currentState = newState;
        lastStateChangeTime = Time.time; // Tracks how long it takes to change states when ChangeState is called
    }
}

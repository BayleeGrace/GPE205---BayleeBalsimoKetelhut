using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : Controller
{
    // An enum is less taxing on the system like an int, but gives the string to other programmers to understand.
    // Each str is being stored as a numeric value (Guard = 1, Chase = 2, etc.)

    public enum AIState { Idle, Seek, Flee, Patrol, Attack, Scan, BackToPost };
    public AIState currentState;
    private float lastStateChangeTime; // Variable for tracking how long it takes for states to change
    public GameObject target; // Stores the target that the AIController will be seeking
    
    public override void Start()
    {
        currentState = AIState.Idle;
        ChangeState(AIState.Idle);
        base.Start();
    }

    public override void Update()
    {
        ProcessInputs();
        base.Update();
    }

    public override void ProcessInputs() // this is overriding the Controller "ProcessInputs()"
    {
        
        switch(currentState) // Switch changes states
        {
            case AIState.Idle:
                // Do work for the Idle state
                DoIdleState();
                if(IsDistanceLessThan(target, 20))
                {
                    ChangeState(AIState.Seek);
                }
                //Check for any transitions
                break;
                // break; is important because it will only execute the "Idle" state before executing the other states.

            case AIState.Seek:
                DoSeekState();
                if(!IsDistanceLessThan(target, 20))
                {
                    ChangeState(AIState.Idle);
                }
                break;

            case AIState.Flee:
                break;

            case AIState.Patrol:
                break;

            case AIState.Attack:
                DoAttackState();
                if (target = null)
                {
                    ChangeState(AIState.Patrol);
                }
                break;

            case AIState.Scan:
                break;

            case AIState.BackToPost:
                break;
        }

    }

    protected void DoIdleState() // This is a function that runs the IDLE state, not the action of being idle
    {
        // Do nothing
    }

    protected void DoSeekState()
    {
        // Seek our target
        Seek(target);
    }
    
    public void Seek(GameObject target)
    {
        // Chase a GAMEOBJECT's position
        if(target != null)
        {
            if(IsDistanceLessThan(target, 10))
                {
                    ChangeState(AIState.Attack);
                }
                else
                {
                    currentState = AIState.Seek;
                    // Rotate towards the target
                    pawn.RotateTowards(target.transform.position);
                    // Move Forward towards the target
                    pawn.MoveForward();
                }
        }
    }

    //public void Seek(Transform targetTransform)
    //{
        // Chase a TRANSFORM's position
        //Seek(targetTransform.position);
    //}

    //public void Seek(Pawn targetPawn)
    //{
        // Chase a PAWN's position
        //Seek(targetPawn.transform);
    //}

    //public void Seek(GameObject targetTransform)
    //{
        // Do Seek Behavior (seek a certain position)
        //targetTransform = target.GetComponent<Transform>();
        //if (targetTransform != null)
        //{
            //Seek(targetTransform.position);
        //}
    //}

    public void DoAttackState()
    {
        if (target != null)
        {
            Seek(target);
            pawn.Shoot();
        }
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

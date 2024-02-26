using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIController : Controller
{
    // An enum is less taxing on the system like an int, but gives the string to other programmers to understand.
    // Each str is being stored as a numeric value (Guard = 1, Chase = 2, etc.)

    public enum AIState { Idle, Chase, Patrol, Attack, Flee, Hide, Enraged };

    // State variables
    public AIState currentState;
    private float lastStateChangeTime; // Variable for tracking how long it takes for states to change
    public GameObject target; // Stores the target that the AIController will be seeking

    // Patrol variables
    public Transform [] patrolWaypoints;
    public float waypointStopDistance;
    private int currentWaypoint = 0;

    public float hearingDistance;

    public float fieldOfView;
    public float maxViewDistance;

    public GameObject lastHit;
    public Vector3 collision = Vector3.zero;

    
    public override void Start()
    {
        TargetPlayerOne();
        currentState = AIState.Patrol;
        ChangeState(AIState.Patrol);
        base.Start();
    }

    public override void Update()
    {
        ProcessInputs();
        CheckRaycast();
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
                break;
                // break; is important because it will only execute the "Idle" state before executing the other states.

            case AIState.Patrol:
                DoPatrolState();
                currentState = AIState.Patrol;
                // Check for transitions
                if (IsInLineOfSight(target) && IsCanSee(target))
                {
                    ChangeState(AIState.Attack);
                }
                break;

            case AIState.Attack:
                DoAttackState();
                if(!IsInLineOfSight(target))
                {
                    ChangeState(AIState.Patrol);
                }
                break;

        }

    }

    // Idle State

    public void DoIdleState() // This is a function that runs the IDLE state, not the action of being idle
    {
        
    }
    
    public void Seek(GameObject target)
    {
        // Chase a GAMEOBJECT's position
        if (target != null)
        {
            currentState = AIState.Chase;
            // Rotate towards the target
            pawn.RotateTowards(target.transform.position);
            // Move Forward towards the target
            pawn.MoveForward();
        }
        else
        {
            ChangeState(AIState.Patrol);
        }
    }

    public void Seek(Transform targetTransform)
    {
        // Seek the position of our target transform
        Seek(targetTransform.gameObject);
    }

    public void Seek(Vector3 targetPosition)
    {
        // RotateTowards the Funciton
        pawn.RotateTowards(targetPosition);
        // Move Forward
        pawn.MoveForward();
    }

    // Patrol State

    public virtual void DoPatrolState()
    {
        if(IsHasTarget())
        {
            // If there are still waypoints we can travel to
            if (patrolWaypoints.Length > currentWaypoint)
            {
                // Then seek that waypoint
                Seek(patrolWaypoints[currentWaypoint]);
                if (Vector3.Distance(pawn.transform.position, patrolWaypoints[currentWaypoint].position) <= waypointStopDistance)
                {
                    currentWaypoint++;
                }
            }
            else
            {
                RestartPatrol();
            }
        }
        else
        {
            TargetPlayerOne();
        }
    }

    protected void RestartPatrol()
    {
        // Reset the current array index back to 0
        currentWaypoint = 0;
    }

    // Chase State

    public virtual void DoChaseState()
    {
        
    }

    // Attack State

    public virtual void DoAttackState()
    {
        Debug.Log(pawn.name + "Is Attacking...");
    }

    // Flee State

    public virtual void DoFleeState()
    {

    }

    // Hide State

    public virtual void DoHideState()
    {

    }

    // Enraged

    public virtual void DoEnragedState()
    {

    }

    // Compare distance between this object and target game object

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

    public void TargetPlayerOne()
    {
        // If the GameManager exists
        if(GameManager.instance != null)
        {
            // And there are existing players
            if(GameManager.instance.players.Count > 0)
            {
                //then target the first player controller in the Game instance
                target = GameManager.instance.players[0].pawn.gameObject;
            }
        }
    }

    protected bool IsHasTarget()
    {
        // return true, we have target
        return (target != null);
        // false, we dont have a target
    }

    public bool IsCanHear(GameObject target)
    {
        // Get the target's NoiseMaker
        NoiseMaker noiseMaker = target.GetComponent<NoiseMaker>();
        // If they don't have one, they can't make noise, so return false
        if (noiseMaker == null) 
        {
            return false;
        }
        // If they are making 0 noise, they also can't be heard
        if (noiseMaker.volumeDistance <= 0) 
        {
            return false;
        }
        // If they are making noise, add the volumeDistance in the noisemaker to the hearingDistance of this AI
        float totalDistance = noiseMaker.volumeDistance + hearingDistance;
        // If the distance between our pawn and target is closer than this...
        if (Vector3.Distance(pawn.transform.position, target.transform.position) <= totalDistance) 
        {
            // ... then we can hear the target
            return true;
        }
        else 
        {
            // Otherwise, we are too far away to hear them
            return false;
        }
    }

    public bool IsCanSee(GameObject target)
    {
        // Find the vector from the agent to the target
        Vector3 agentToTargetVector = target.transform.position - transform.position;
        // Find the angle between the direction our agent is facing (forward in local space) and the vector to the target.
        float angleToTarget = Vector3.Angle(agentToTargetVector, pawn.transform.forward);
        // if that angle is less than our field of view
        if (angleToTarget < fieldOfView) 
        {
            return true;
        }
        else 
        {
            return false;
        }
    }

    public void CheckRaycast()
    {
        var ray= new Ray(pawn.transform.position, pawn.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            lastHit = hit.transform.gameObject;
            collision = hit.point;
        }
    }

    public bool IsInLineOfSight(GameObject target)
    {
        if (target == lastHit)
        {
            return true;
        }
        else{
            return false;
        }
        
        //Ray agentRay = new Ray(transform.position, transform.forward);
        //Debug.DrawRay(agentRay.origin, agentRay.direction * 10);
        //if (Physics.Raycast(ray, out RaycastHit hitData))
        //{
            //Debug.Log(hit.collider.gameObject.name + " was hit!")
        //}
    }
}

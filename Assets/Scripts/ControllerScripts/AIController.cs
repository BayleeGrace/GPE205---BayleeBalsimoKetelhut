using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AIController : Controller
{
    #region Variables;
    // An enum is less taxing on the system like an int, but gives the string to other programmers to understand.
    // Each str is being stored as a numeric value (Guard = 1, Chase = 2, etc.)
    public enum AIState { Idle, Chase, Patrol, Attack, Flee, Hide, Enraged };

    // State variables
    public AIState currentState;
    private float lastStateChangeTime; // Variable for tracking how long it takes for states to change
    public GameObject target; // Stores the target that the AIController will be seeking

    // Patrol variables
    public PatrolWaypoint[] patrolWaypoints;
    public float waypointStopDistance;
    private int currentWaypoint = 0;

    public float hearingDistance; // Variable to hold hearing

    public float fieldOfView; // Variable to hold the AI's field of view value
    public float maxViewDistance; // Variable to determine how far the enemy can see

    public GameObject lastHit; // Tracks what the AI is seeing in-game
    public Vector3 collision = Vector3.zero; // ???
    #endregion Variables;
    
    public override void Start()
    {
        patrolWaypoints = FindObjectsOfType<PatrolWaypoint>();
        
        if(GameManager.instance != null)
        {
            // Register this enemy with the List in the Game Manager
            GameManager.instance.enemies.Add(this);
        }
        
        TargetPlayerOne();
        currentState = AIState.Idle;
        ChangeState(AIState.Idle);
        base.Start();

    }

    public override void Update()
    {
        ProcessInputs();
        CheckRaycast(); // Tracks what the AI is "seeing" at all times
        base.Update();
    }

    public override void ProcessInputs() // this is overriding the Controller "ProcessInputs()"
    {
        
        #region State Transitions;
        switch(currentState) // Switch changes the enum based on the current state given
        {
            case AIState.Idle:
                // Do work for the Idle state
                DoIdleState();
                currentState = AIState.Idle;
                if (IsCanHear(target) || IsCanSee(target))
                {
                    ChangeState(AIState.Chase);
                }
                //Check for any transitions
                break; // break; is important because it will only execute the "Idle" state before executing the other states.

            case AIState.Patrol:
                DoPatrolState();
                currentState = AIState.Patrol;
                // Check for transitions
                if (IsCanSee(target) || IsCanHear(target))
                {
                    ChangeState(AIState.Chase);
                }
                break;

            case AIState.Chase:
                DoChaseState();
                currentState = AIState.Chase;
                // Check for transitions
                if (IsInLineOfSight(target) && IsCanSee(target) && IsDistanceLessThan(target, hearingDistance))
                {
                    ChangeState(AIState.Attack);
                }
                break;

            case AIState.Attack:
                DoAttackState();
                // Check for transitions
                if(!IsInLineOfSight(target))
                {
                    ChangeState(AIState.Chase);
                }
                if(!IsDistanceLessThan(target, hearingDistance))
                {
                    ChangeState(AIState.Patrol);
                }
                break;
        }
        #endregion State Transitions;

    }

    /// <summary> ChangeState()
    /// Changes the current state based on what the new state that is given
    /// </summary>
    public virtual void ChangeState(AIState newState)
    {
        currentState = newState;
        lastStateChangeTime = Time.time; // Tracks how long it takes to change states when ChangeState is called
    }

    #region Idle functions;

    public void DoIdleState() // This is a function that runs the IDLE state, not the action of being idle
    {
        
    }

    #endregion Idle functions;
    
    #region Seek functions;
    
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
        if (targetTransform != null)
        {
            // Seek the position of our target transform
            Seek(targetTransform.gameObject);
        }
    }

    public void Seek(Vector3 targetPosition)
    {
        if (targetPosition != null)
        {
            // RotateTowards the Funciton
            pawn.RotateTowards(targetPosition);
            // Move Forward
            pawn.MoveForward();
        }
    }
    #endregion Seek functions;

    #region Patrol functions;

    public virtual void DoPatrolState()
    {
        
        PatrolWaypoint currentWaypoint = 
        
        if(!IsHasTarget())
        {
            if (patrolWaypoints != null)
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

    #endregion Patrol functions;

    #region Chase functions;

    public virtual void DoChaseState()
    {
        // Seek our target
        if (target != null)
        {
        Seek(target);
        }
    }

    #endregion Chase functions;

    #region Attack functions;

    public virtual void DoAttackState()
    {
        Debug.Log(pawn.name + "Is Attacking...");
    }
    
    #endregion Attack functions;

    #region Flee functions;

    public virtual void DoFleeState()
    {

    }

    #endregion Flee functions;

    #region Hide functions;

    public virtual void DoHideState()
    {

    }

    #endregion Hide functions;

    #region Enraged functions;

    public virtual void DoEnragedState()
    {

    }

    #endregion Enraged functions;

    #region AI senses;
    
    /// <summary> IsCanHear()
    /// Grabs the target's NoiseMaker (if it exists) and returns if the target is being heard
    /// </summary>
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

    /// <summary> IsCanSee()
    /// Grabs the target's position, finds the vector between this object's position and the target, and checks if that angle difference is within the FOV
    /// </summary>
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

    /// <summary> CheckRaycast()
    /// Creates a Raycast at the AI's and checks what it is colliding with
    /// </summary>
    public void CheckRaycast()
    {
        var ray = new Ray(pawn.transform.position, pawn.transform.forward);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100))
        {
            lastHit = hit.transform.gameObject;
            collision = hit.point;
        }
    }

    /// <summary> IsInLineOfSight()
    /// Determines if the TARGET is in the exact line of sight of the enemy AI
    /// </summary>
    public bool IsInLineOfSight(GameObject target)
    {
        if (target == lastHit)
        {
            return true;
        }
        else{
            return false;
        }
        
        /*Ray agentRay = new Ray(transform.position, transform.forward);
        Debug.DrawRay(agentRay.origin, agentRay.direction * 10);
        if (Physics.Raycast(ray, out RaycastHit hitData))
        {
            //Debug.Log(hit.collider.gameObject.name + " was hit!")
        }*/
    }

    #endregion AI senses;

    /// <summary> IsDistanceLessThan()
    /// Compare distance between this object's position and target game object's position
    /// </summary>
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

    /// <summary> TargetPlayerOne()
    /// Take the first player in the list of "players" within the Game Manager, and Target that player (if that player exists)
    /// </summary>
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

    /// <summary> IsHasTarget()
    /// Tracks if the AI's target exists
    /// </summary>
    protected bool IsHasTarget()
    {
        // return true, we have target
        return (target != null);
        // false, we dont have a target
    }
    
    // Removes this object from the enemies array in the Game Manager
    public void OnDestroy()
    {
        if (GameManager.instance.enemies != null)
        {
            // Deregister with the Game Manager
            GameManager.instance.enemies.Remove(this);
        }
    }
}

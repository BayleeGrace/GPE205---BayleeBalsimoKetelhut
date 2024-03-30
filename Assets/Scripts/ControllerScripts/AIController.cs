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
    public GameObject targetPlayer; // Stores the targetPlayer that the AIController will be seeking

    // Patrol variables
    //public Transform[] patrolWaypoints;
    public List<Transform> patrolWaypoints;
    public float waypointStopDistance;
    //private int currentWaypoint = 0;
    public Transform nearestWaypoint;
    private bool firstWaypointWasReached = false;

    public float hearingDistance; // Variable to hold hearing

    public float fieldOfView; // Variable to hold the AI's field of view value
    public float maxViewDistance; // Variable to determine how far the enemy can see

    public GameObject lastHit; // Tracks what the AI is seeing in-game
    public Vector3 collision = Vector3.zero; // ???
    #endregion Variables;
    
    public override void Start()
    {
        PatrolWaypoint[] foundPatrolWaypoints = FindObjectsOfType<PatrolWaypoint>();
        foreach (var patrolWaypoint in foundPatrolWaypoints)
        {
            patrolWaypoints.Add(patrolWaypoint.transform);
        }
        
        if(GameManager.instance != null)
        {
            // Register this enemy with the List in the Game Manager
            GameManager.instance.enemies.Add(this);
        }
        
        currentState = AIState.Patrol;
        ChangeState(AIState.Patrol);
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
        if(IsHasTarget() == true)
        {
            #region State Transitions;
            switch(currentState) // Switch changes the enum based on the current state given
            {
                case AIState.Idle:
                    // Do work for the Idle state
                    DoIdleState();
                    currentState = AIState.Idle;
                    if (IsCanHear(targetPlayer) || IsCanSee(targetPlayer) || IsCanHear(targetPlayer) && IsCanSee(targetPlayer))
                    {
                        ChangeState(AIState.Chase);
                    }
                    //Check for any transitions
                    break; // break; is important because it will only execute the "Idle" state before executing the other states.

                case AIState.Patrol:
                    DoPatrolState();
                    currentState = AIState.Patrol;
                    // Check for transitions
                    if (IsCanHear(targetPlayer) || IsCanSee(targetPlayer) || (IsCanHear(targetPlayer) && IsCanSee(targetPlayer)))
                    {
                        ChangeState(AIState.Chase);
                    }
                    break;

                case AIState.Chase:
                    DoChaseState();
                    currentState = AIState.Chase;
                    // Check for transitions
                    if (targetPlayer == null || !IsDistanceLessThan(targetPlayer, hearingDistance))
                    {
                        ChangeState(AIState.Patrol);
                    }
                    break;
            }
            #endregion State Transitions;
        }
        else if ((GameManager.instance.players.Count > 0) || IsHasTarget() == false)
        {
            if (GameManager.instance.players[0] != null)
            {
                TargetPlayerOne();
            }
            else if (GameManager.instance.players[1] != null)
            {
                TargetPlayerTwo();
            }
        }
        else
        {
            ChangeState(AIState.Patrol);
        }
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
    
    public void Seek(GameObject targetPlayer)
    {
        // Chase a GAMEOBJECT's position
        if (targetPlayer != null)
        {
            currentState = AIState.Chase;
            // Rotate towards the targetPlayer
            pawn.RotateTowards(targetPlayer.transform.position);
            // Move Forward towards the targetPlayer
            pawn.MoveForward();
        }
        else
        {
            Seek(nearestWaypoint);
            ChangeState(AIState.Patrol);
        }
    }

    public void Seek(Transform targetPlayerTransform)
    {
        if (targetPlayerTransform != null)
        {
            // Seek the position of our targetPlayer transform
            pawn.RotateTowards(targetPlayerTransform.position);
            // Move Forward
            pawn.MoveForward();
        }
    }

    public void Seek(Vector3 targetPlayerPosition)
    {
        if (targetPlayerPosition != null)
        {
            // RotateTowards the Funciton
            pawn.RotateTowards(targetPlayerPosition);
            // Move Forward
            pawn.MoveForward();
        }
    }
    #endregion Seek functions;

    #region Patrol functions;

    public virtual void DoPatrolState()
    {
        if (patrolWaypoints != null)
        {
            FindNearestPatrolWaypoint();
            if (IsDistanceLessThan(nearestWaypoint.gameObject, (waypointStopDistance)))
            {
                // Then seek that waypoint
                Seek(nearestWaypoint);
                if (Vector3.Distance(pawn.transform.position, nearestWaypoint.position) <= waypointStopDistance)
                {
                    firstWaypointWasReached = true;
                    RestartPatrol();
                }
            }
            else if ((!IsDistanceLessThan(nearestWaypoint.gameObject, waypointStopDistance)) || firstWaypointWasReached == true)
            {
                Transform nextWaypoint = nearestWaypoint.GetComponent<PatrolWaypoint>().nextWaypoint.transform;
                nearestWaypoint = nextWaypoint;

                Seek(nearestWaypoint);
                RestartPatrol();
            }
            /*else
            {
                RestartPatrol();
            }*/
        }
    }
    

    // Create a function to track what PatrolWaypoint is the closest to the current AIController
    public void FindNearestPatrolWaypoint()
    {
        if (firstWaypointWasReached == false)
        {
            // Assume the first waypoint in the array is the nearest
            nearestWaypoint = patrolWaypoints[0].transform;

            float distanceToNearestWaypoint = Vector3.Distance(nearestWaypoint.position, pawn.transform.position);

            // Compare the current waypoint's Vector distance to all other waypoint distances in the list
            foreach (var patrolWaypoint in patrolWaypoints)
            {
                if (patrolWaypoint.gameObject != nearestWaypoint.gameObject)
                {
                    // Find the distance between the AI and each patrolWaypoint
                    float distanceToNewWaypoint = Vector3.Distance(patrolWaypoint.position, pawn.transform.position);
                    if (distanceToNewWaypoint < distanceToNearestWaypoint)
                    {
                        distanceToNewWaypoint = distanceToNearestWaypoint;
                        nearestWaypoint = patrolWaypoint.transform;
                        //Debug.Log("Nearest waypoint for " + pawn + " changed to " + nearestWaypoint);
                    }
                }
            }
        }
        else if (firstWaypointWasReached == true)
        {
            float distanceToNearestWaypoint = Vector3.Distance(nearestWaypoint.position, pawn.transform.position);

            foreach (var patrolWaypoint in patrolWaypoints)
            {
                Transform nextWaypoint = nearestWaypoint.GetComponent<PatrolWaypoint>().nextWaypoint.transform;

                float distanceToNewWaypoint = Vector3.Distance(nextWaypoint.position, pawn.transform.position);
                    if (distanceToNewWaypoint < distanceToNearestWaypoint)
                    {
                        distanceToNewWaypoint = distanceToNearestWaypoint;
                        nearestWaypoint = nextWaypoint;
                    }
            }
        }
    }

    protected void RestartPatrol()
    {
        // Reset the current array index back to 0
        FindNearestPatrolWaypoint();
        nearestWaypoint = patrolWaypoints[0];
    }

    #endregion Patrol functions;

    #region Chase functions;

    public virtual void DoChaseState()
    {
        // Seek our targetPlayer
        if (targetPlayer != null)
        {
        Seek(targetPlayer);
        }
    }

    #endregion Chase functions;

    #region Attack functions;

    public virtual void DoAttackState()
    {
        if (targetPlayer != null)
        {
            // Chase
            Seek(targetPlayer);
            // Shoot
            pawn.Shoot();
        }
        else if (targetPlayer == null)
        {
            ChangeState(AIState.Patrol);
        }
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
    /// Grabs the targetPlayer's NoiseMaker (if it exists) and returns if the targetPlayer is being heard
    /// </summary>
    public bool IsCanHear(GameObject targetPlayer)
    {
            // Get the targetPlayer's NoiseMaker
            NoiseMaker noiseMaker = targetPlayer.GetComponent<NoiseMaker>();
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
            // If the distance between our pawn and targetPlayer is closer than this...
            if (Vector3.Distance(pawn.transform.position, targetPlayer.transform.position) <= totalDistance) 
            {
                // ... then we can hear the targetPlayer
                return true;
            }
            else 
            {
                // Otherwise, we are too far away to hear them
                return false;
            }
    }

    /// <summary> IsCanSee()
    /// Grabs the targetPlayer's position, finds the vector between this object's position and the targetPlayer, and checks if that angle difference is within the FOV
    /// </summary>
    public bool IsCanSee(GameObject targetPlayer)
    {
        // Find the vector from the agent to the targetPlayer
        Vector3 agentTotargetPlayerVector = targetPlayer.transform.position - transform.position;
        // Find the angle between the direction our agent is facing (forward in local space) and the vector to the targetPlayer.
        float angleTotargetPlayer = Vector3.Angle(agentTotargetPlayerVector, pawn.transform.forward);
        // if that angle is less than our field of view
        if (angleTotargetPlayer < fieldOfView) 
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
    /// Determines if the targetPlayer is in the exact line of sight of the enemy AI
    /// </summary>
    public bool IsInLineOfSight(GameObject targetPlayer)
    {
        if (targetPlayer == lastHit)
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
    /// Compare distance between this object's position and targetPlayer game object's position
    /// </summary>
    protected bool IsDistanceLessThan(GameObject targetPlayer, float distance)
    {
        // Compare transform distance of two pawns, the owner of this component and targetPlayer pawn
        if (Vector3.Distance(pawn.transform.position, targetPlayer.transform.position) < distance)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    /// <summary> targetPlayerOne()
    /// Take the first player in the list of "players" within the Game Manager, and targetPlayer that player (if that player exists)
    /// </summary>
    public void TargetPlayerOne()
    {
        // If the GameManager exists
        if(GameManager.instance != null)
        {
            if (GameManager.instance.players.Count > 0)
            {
                targetPlayer = GameManager.instance.players[0].pawn.gameObject;
            }
        }
    }

    public void TargetPlayerTwo()
    {
        // If the GameManager exists
        if(GameManager.instance != null)
        {
            if (GameManager.instance.players.Count > 0)
            {
                targetPlayer = GameManager.instance.players[1].pawn.gameObject;
            }
        }
    }

    /*public void FindAnotherTargetPlayer()
    {
        if(targetPlayer == null)
        {
            if (GameManager.instance.players.Count > 0)
            {
                targetPlayer = GameManager.instance.players[0].pawn.gameObject;
            }
        }
    }*/

    /// <summary> IsHastargetPlayer()
    /// Tracks if the AI's targetPlayer exists
    /// </summary>
    protected bool IsHasTarget()
    {
        // return true, we have targetPlayer
        return (targetPlayer != null);
        // false, we dont have a targetPlayer
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

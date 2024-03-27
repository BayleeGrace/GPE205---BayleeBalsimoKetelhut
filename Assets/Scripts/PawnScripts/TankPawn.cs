using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankPawn : Pawn
{
    public Shooter shooter;

    // Variable for our shell prefab
    public GameObject tankShellPrefab;
    // Variable for our firing force
    public float fireForce;
    // Variable for our damage done
    public float damageDone;
    // Variable for how long our bullets survive if they don't collide
    public float lifespan;
    // Variabe to track when the next event will happen
    public float nextEventTime;
    [HideInInspector]
    public float timerDelay;

    public bool isPlayer;
    
    // Start is called before the first frame update
    // Since we inherit from Pawn, we can remove Start and Update fx's. To be safe I will be telling it to run from the parent anyways.
    public override void Start()
    {
        base.Start();
        
        float secondsPerShot;
        if (fireRate <= 0)
        {
            secondsPerShot = Mathf.Infinity;
        }
        else
        {
            secondsPerShot = 1 / fireRate;
        }
        timerDelay = secondsPerShot;
        nextEventTime = Time.time + timerDelay;
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        //CheckFireRate();
        if ((mover.isMoving && isPlayer) == true)
        {
            mover.PlayRotateSound();
        }
    }

    public override void MoveForward()
    {
        mover.Move(transform.forward, moveSpeed);
        mover.isMoving = true;
    }

    public override void MoveBackward()
    {
        mover.Move(transform.forward, -moveSpeed);
        mover.isMoving = true;
    }

    public override void RotateClockwise()
    {
        mover.Rotate(turnSpeed);
        mover.isMoving = true;
    }

    public override void RotateCounterClockwise()
    {
        mover.Rotate(-turnSpeed);
        mover.isMoving = true;
    }

    public override void Shoot()
    {
        if (Time.time >= nextEventTime)
        {
            shooter.Shoot(tankShellPrefab, fireForce, damageDone, lifespan);
            nextEventTime = Time.time + timerDelay;
        }
    }

    public override void RotateTowards(Vector3 targetPosition)
    {
        // find the vector to the target
        Vector3 vectorToTarget = targetPosition - transform.position;
        // find the rotation to look down that vector
        Quaternion targetRotation = Quaternion.LookRotation(vectorToTarget, Vector3.up);
        // Rotate closer to that vector, but not more than turn speed allows in a single frame
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
    }

    public override void MakeNoise()
    {
        if(noiseMaker != null)
        {
            noiseMaker.volumeDistance = noiseMakerVolume;
        }
    }

    public override void StopNoise()
    {
        if(noiseMaker != null)
        {
            noiseMaker.volumeDistance = 0;
            mover.isMoving = false;
        }
    }

}

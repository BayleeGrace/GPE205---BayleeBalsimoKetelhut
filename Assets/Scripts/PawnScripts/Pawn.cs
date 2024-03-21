using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Since this is a component, must identify as abstract (see below)
public abstract class Pawn : MonoBehaviour
{
    public float moveSpeed; // Variable for move speed, public so designers can change
    public float turnSpeed; // Variable for turn speed, public so designers can change
    public Mover mover; // We need a variable for our Pawn to hold our Mover component
    public float fireRate;
    public NoiseMaker noiseMaker;
    public float noiseMakerVolume;

    public virtual void Start() // Use virtual in case we might override Start in our child functions
    {
        mover = GetComponent<Mover>();
        noiseMaker = GetComponent<NoiseMaker>();
    }

    // Abstract makes it so that this function is abstract and cannot be used with this code (we use this bc it's a component)
    public abstract void MoveForward();
    public abstract void MoveBackward();
    public abstract void RotateClockwise();
    public abstract void RotateCounterClockwise();
    public abstract void Shoot();
    public abstract void RotateTowards(Vector3 targetPosition);
    public abstract void MakeNoise();
    public abstract void StopNoise();

}

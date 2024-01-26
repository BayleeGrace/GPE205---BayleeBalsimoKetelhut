using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Since this is a component, must identify as abstract (see below)
public abstract class Pawn : MonoBehaviour
{
    // Variable for move speed, public so designers can change
    public float moveSpeed;
    // Variable for turn speed, public so designers can change
    public float turnSpeed;
    // We need a variable for our Pawn to hold our Mover component
    public Mover mover;

    // Start is called BEFORE the first frame update
    // Use virtual in case we might override Start in our child functions
    public virtual void Start()
    {
        mover = GetComponent<Mover>();
    }

    // Update is called ONCE per FRAME
    // Use virtual in case we might override Start in our child functions
    public virtual void Update()
    {
        
    }

    // Abstract makes it so that this function is abstract and cannot be used with this code (we use this bc it's a component)
    public abstract void MoveForward();
    public abstract void MoveBackward();
    public abstract void RotateClockwise();
    public abstract void RotateCounterClockwise();

}

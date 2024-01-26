using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{

    public KeyCode moveForwardKey;
    public KeyCode moveBackwardKey;
    public KeyCode rotateClockwiseKey;
    public KeyCode rotateCounterClockwiseKey;

    // Start is called before the first frame update
    public override void Start()
    {
        // Run the Start() fx from the parent (base) class
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        // Process your keyboard inputs
        // Where does the ProcessInputs() fx come from?
        ProcessInputs();

        // Run the Update() fx from the parent class
        base.Update();
    }

    public void ProcessInputs() // I'm not sure why we do this?
    {
        // Create a conditional that checks every frame (above) to see if the player has pressed X key within controller
        if (Input.GetKey(moveForwardKey))
        {
            pawn.MoveForward();
        }

        // If moveForwardKey is not pressed, check if it's moveBackwardKey...etc.
        if (Input.GetKey(moveBackwardKey))
        {
            pawn.MoveBackward();
        }

        if (Input.GetKey(rotateClockwiseKey))
        {
            pawn.RotateClockwise();
        }

        if (Input.GetKey(rotateCounterClockwiseKey))
        {
            pawn.RotateCounterClockwise();
        }
    }

}

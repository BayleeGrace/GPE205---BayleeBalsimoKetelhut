using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerController : Controller
{
    public KeyCode moveForwardKey;
    public KeyCode moveBackwardKey;
    public KeyCode rotateClockwiseKey;
    public KeyCode rotateCounterClockwiseKey;
    private Mover mover;

    // variable for the shoot function
    public KeyCode shootKey;
    // Variable to hold the pause game fx
    public KeyCode pauseKey;
    [HideInInspector] public GameObject playerCamera;
    //public int scoreToAdd;
    public int playerScore;

    // Start is called before the first frame update
    public void Awake()
    {
        // Check if we have a Game Manager // If the instance doesn't exist yet...
        if(GameManager.instance != null)
        {
            // Register this player with the List in the Game Manager
            GameManager.instance.players.Add(this);
        }
        // Run the Start() fx from the parent (base) class
        base.Start();
    }

    // Update is called once per frame
    public override void Update()
    {
        // Process your keyboard inputs
        ProcessInputs();

        // Run the Update() fx from the parent class
    }

    public override void ProcessInputs() // I'm not sure why we do this?
    {
        // Create a conditional that checks every frame (above) to see if the player has pressed X key within controller
        if (Input.GetKey(moveForwardKey))
        {
            pawn.MoveForward();
            pawn.MakeNoise();
        }

        // If moveForwardKey is not pressed, check if it's moveBackwardKey...etc.
        if (Input.GetKey(moveBackwardKey))
        {
            pawn.MoveBackward();
            pawn.MakeNoise();
        }

        if (Input.GetKey(rotateClockwiseKey))
        {
            pawn.RotateClockwise();
            pawn.MakeNoise();
        }

        if (Input.GetKey(rotateCounterClockwiseKey))
        {
            pawn.RotateCounterClockwise();
            pawn.MakeNoise();
        }
        
        if (Input.GetKeyDown(shootKey))
        {
            pawn.Shoot();
        }

        if (!Input.GetKey(moveForwardKey) && !Input.GetKey(moveBackwardKey) && !Input.GetKey(rotateClockwiseKey) && !Input.GetKey(rotateCounterClockwiseKey) && !Input.GetKeyDown(shootKey))
        {
            pawn.StopNoise();
        }

        if (Input.GetKeyDown(pauseKey))
        {
            GameManager.instance.ActivatePauseMenuScreen();
        }
    }

    // Create a function that tells the Game Manager when this player has died, thus removing it from the players List
    // OnDestroy() is not a custom function in Unity C#. We don't have to write a separate command to call this function.
    public void OnDestroy()
    {
        // Instance tracking the destroyed player
        if (GameManager.instance.players != null)
        {
            GameManager.instance.players.Remove(this);
        }
    }

    public override void AddToScore(int scoreToAdd)
    {
        playerScore = playerScore + scoreToAdd;
        Debug.Log("New score for " + pawn + " is " + playerScore);
    }

}

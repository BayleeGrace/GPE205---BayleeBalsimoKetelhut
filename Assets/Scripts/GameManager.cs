using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Variable to reference the player controller and the tank pawn
    public GameObject playerControllerPrefab;
    public GameObject tankPawnPrefab;

    // Variable to hold the player spawn location
    public Transform playerSpawnTransform;

    // Variable to reference the GameManager
    public static GameManager instance;

    // Creates a LIST of players, even if the game is going to be single player
    public List<PlayerController> players;
    
    // Create a game object that holds this game manager
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        // If there is a game object that holds the game instance already, destroy it
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        SpawnPlayer();
    }

    // Spawn the Player Controller at x, y, x, with no rotation
    public void SpawnPlayer()
    {
        GameObject newPlayerObj = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity) as GameObject; // Quaternion.identity has something to do with the rotation
        GameObject newPawnObj = Instantiate(tankPawnPrefab, playerSpawnTransform.position, Quaternion.identity) as GameObject;
        
        // Get the Player Controller component and the Pawn component
        Controller newPlayerController = newPlayerObj.GetComponent<Controller>();
        Pawn newPlayerPawn = newPawnObj.GetComponent<Pawn>();

        newPawnObj.AddComponent<NoiseMaker>();
        newPlayerPawn.noiseMaker = newPawnObj.GetComponent<NoiseMaker>();
        newPlayerPawn.noiseMakerVolume = 3;

        // Hook them up!
        newPlayerController.pawn = newPlayerPawn;

    }

}

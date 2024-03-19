using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables;
    public GameObject playerControllerPrefab; // Variable to reference the player controller and the tank pawn
    public GameObject tankPawnPrefab; // Variable to store the player's tankPawn
    public GameObject playerSpawnTransform; // Variable to hold the player spawn location
    public bool isPlayerSpawned = false; // Boolean to check if the player was spawned, may need to be changed later to allow for multiplayer
    public GameObject cameraPrefab;
    public CameraController cameraControllerPrefab;
    public GameObject[] enemyControllerPrefabs; // Variable to reference the AI controllers and their pawns
    //public GameObject enemyPawnPrefab; **Not used d/t controller being contained within enemy prefab, may need to be used later!**
    public PawnSpawnPoint currentSpawnPoint; // Variable to hold the current spawn location
    public MapGenerator mapGenerator; // Variable to reference the Map Generator
    public static GameManager instance; // Variable to reference the GameManager
    public List<PlayerController> players; // Creates a LIST of players, even if the game is going to be single player
    public List<AIController> enemies; // Creates a LIST of enemies based on how many AIControllers that were spawned in the scene
    #endregion Variables;
    
    private void Awake()
    {
        // Create a game object that holds this game manager
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Don't destroy the first GameManeged created in the scene
        }
        // If there is a game object that holds the game instance already, destroy it
        else
        {
            Destroy(gameObject); 
        }

        if (mapGenerator != null)
        {
            mapGenerator.GenerateMap();
        }
    }
    
    private void Start()
    {
        // Grab all waypoints in the scene that were generated and store it in a local array: spawnPoints
        PawnSpawnPoint[] spawnPoints = FindObjectsOfType<PawnSpawnPoint>();

        // Create a variable "spawnPoint" in the spawnPoints array, and for each spawnpoint in that array...
        foreach (var spawnPoint in spawnPoints)
        {
            // set the current spawn point iteration to the current spawn point that the enemy will spawn at
            currentSpawnPoint = spawnPoint;

            // if that spawn point is not marked as a player spawn,
            if (currentSpawnPoint.isPlayerSpawn == true && isPlayerSpawned == false)
            {
                // Grab a random spawn point for the player to spawn at
                PawnSpawnPoint randomPlayerSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                
                // If that spawn point is a player spawner
                if (randomPlayerSpawnPoint.isPlayerSpawn == true)
                {
                    // spawn the player at that random spawn point
                    SpawnPlayer(randomPlayerSpawnPoint);
                    SpawnPlayerCamera();
                }
            }
            // if the current spawn point is not a player spawn, then spawn an enemy
            else if (currentSpawnPoint.isPlayerSpawn == false)
            {
                // spawn the enemies at every other spawn point in the array
                SpawnEnemy();
            }
        }
    }

    // Spawn the Player Controller at x, y, x, with no rotation
    public void SpawnPlayer(PawnSpawnPoint playerSpawn)
    {
        // New Player/Pawn Obj's are being contained as local variables in this function.
        GameObject newPlayerObj = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity) as GameObject; // Quaternion.identity has something to do with the rotation
        GameObject newPawnObj = Instantiate(tankPawnPrefab, playerSpawn.transform.position, Quaternion.identity) as GameObject;
        
        // Get the Player Controller component and the Pawn component
        Controller newPlayerController = newPlayerObj.GetComponent<Controller>();
        Pawn newPlayerPawn = newPawnObj.GetComponent<Pawn>();

        // For every player pawn spawned in the game, add a noise maker to it and set the volume to 3
        newPawnObj.AddComponent<NoiseMaker>();
        newPlayerPawn.noiseMaker = newPawnObj.GetComponent<NoiseMaker>();
        newPlayerPawn.noiseMakerVolume = 3;

        // Hook them up!
        newPlayerController.pawn = newPlayerPawn;

        isPlayerSpawned = true;

    }

    public void SpawnPlayerCamera()
    {
        foreach (var player in players)
        {
            GameObject newCamera = Instantiate(cameraPrefab, player.pawn.transform.position, Quaternion.identity) as GameObject;

            cameraPrefab = newCamera;
        }
    }

    public void SpawnEnemy()
    {
        // The enemy prefab has the enemy controller contained within it, so for now we only have to spawn that prefab. May need to be changed later.
        GameObject newEnemyObj = Instantiate(RandomEnemyPrefab(), currentSpawnPoint.transform.position, Quaternion.identity) as GameObject;

        // See above in SpawnPlayer() for information
        AIController newEnemyController = newEnemyObj.GetComponent<AIController>();
        Pawn newEnemyPawn = newEnemyObj.GetComponent<Pawn>();
        
        // Add each enemy spawned to the list of enemies in the Game Manager
        enemies.Add(newEnemyController);
    }

    public GameObject RandomEnemyPrefab()
    {
        // pull random enemy obj's from the allotted array created by designers
        return enemyControllerPrefabs[Random.Range(0, enemyControllerPrefabs.Length)];
    }
    
}

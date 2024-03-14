using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables;
    // Variable to reference the player controller and the tank pawn
    public GameObject playerControllerPrefab;
    public GameObject tankPawnPrefab;
    // Variable to hold the player spawn location
    public GameObject playerSpawnTransform;

    // Variable to reference the AI controllers and their pawns
    public GameObject[] enemyControllerPrefabs;
    //public GameObject enemyPawnPrefab;
    public PawnSpawnPoint currentSpawnPoint; // Variable to hold the current spawn location
    
    public MapGenerator mapGenerator; // Variable to reference the Map Generator

    public static GameManager instance; // Variable to reference the GameManager

    public List<PlayerController> players; // Creates a LIST of players, even if the game is going to be single player
    public List<AIController> enemies;
    #endregion Variables;
    
    private void Awake()
    {
        // Create a game object that holds this game manager
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

        if (mapGenerator != null)
        {
            mapGenerator.GenerateMap();
        }
    }
    
    private void Start()
    {
        // Grab all waypoints in the scene that were generated
        PawnSpawnPoint[] spawnPoints = FindObjectsOfType<PawnSpawnPoint>();

        foreach (var spawnPoint in spawnPoints)
        {
            // set the current spawn point iteration to the current spawn point that the enemy will spawn at
            currentSpawnPoint = spawnPoint; 

            // if that spawn point is not marked as a player spawn,
            if (currentSpawnPoint.isPlayerSpawn==true)
            {
                // spawn the player
                SpawnPlayer();
            }
            else if (currentSpawnPoint.isPlayerSpawn==false)
            {
                // spawn the enemies at every other spawn point in the array
                SpawnEnemy();
            }
        }
    }

    // Spawn the Player Controller at x, y, x, with no rotation
    public void SpawnPlayer()
    {
        GameObject newPlayerObj = Instantiate(playerControllerPrefab, Vector3.zero, Quaternion.identity) as GameObject; // Quaternion.identity has something to do with the rotation
        GameObject newPawnObj = Instantiate(tankPawnPrefab, playerSpawnTransform.transform.position, Quaternion.identity) as GameObject;
        
        // Get the Player Controller component and the Pawn component
        Controller newPlayerController = newPlayerObj.GetComponent<Controller>();
        Pawn newPlayerPawn = newPawnObj.GetComponent<Pawn>();

        newPawnObj.AddComponent<NoiseMaker>();
        newPlayerPawn.noiseMaker = newPawnObj.GetComponent<NoiseMaker>();
        newPlayerPawn.noiseMakerVolume = 3;

        // Hook them up!
        newPlayerController.pawn = newPlayerPawn;

    }

    public void SpawnEnemy()
    {
        GameObject newEnemyObj = Instantiate(RandomEnemyPrefab(), currentSpawnPoint.transform.position, Quaternion.identity) as GameObject;

        AIController newEnemyController = newEnemyObj.GetComponent<AIController>();
        Pawn newEnemyPawn = newEnemyObj.GetComponent<Pawn>();
        
        enemies.Add(newEnemyController);
    }

    public GameObject RandomEnemyPrefab()
    {
        // pull random enemy obj's
        return enemyControllerPrefabs[Random.Range(0, enemyControllerPrefabs.Length)];
    }
    
}

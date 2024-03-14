using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Variable to reference the player controller and the tank pawn
    public GameObject playerControllerPrefab;
    public GameObject tankPawnPrefab;
    // Variable to hold the player spawn location
    public GameObject playerSpawnTransform;

    // Variable to reference the AI controllers and their pawns
    public GameObject[] enemyControllerPrefabs;
    //public GameObject enemyPawnPrefab;
    // Variable to hold enemy spawn locations
    public PawnSpawnPoint[] spawnPoints;

    // Variable to reference the Map Generator
    public MapGenerator mapGenerator;

    // Variable to reference the GameManager
    public static GameManager instance;

    // Creates a LIST of players, even if the game is going to be single player
    public List<PlayerController> players;
    public List<AIController> enemies;
    
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

        if (mapGenerator != null)
        {
            mapGenerator.GenerateMap();
        }

    }
    
    private void Start()
    {

        foreach (var spawnPoints in spawnPoints)
        {
            if (spawnPoints.isPlayerSpawn==true)
            {
                SpawnPlayer();
            }
            else if (spawnPoints.isPlayerSpawn==false)
            {
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
        //GameObject newEnemyObj = Instantiate(enemyControllerPrefabs);
                GameObject newEnemyObj = Instantiate(RandomEnemyPrefab(), Vector3.zero, Quaternion.identity) as GameObject;
                //GameObject newEnemyPawnObj = Instantiate(enemyPawnPrefab, RandomSpawnPoint().transform.position, Quaternion.identity) as GameObject;

                Controller newEnemyController = newEnemyObj.GetComponent<AIController>();
                Pawn newEnemyPawn = newEnemyObj.GetComponent<Pawn>();
        
    }

    public GameObject RandomEnemyPrefab()
    {
        // pull random enemy controllers
        return enemyControllerPrefabs[Random.Range(0, enemyControllerPrefabs.Length)];
    }
    
}

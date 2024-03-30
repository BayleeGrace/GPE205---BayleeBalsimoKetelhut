using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region Variables;
    public GameObject playerControllerPrefab; // Variable to reference the player controller and the tank pawn
    public GameObject playerControllerPrefab2;
    public GameObject tankPawnPrefab; // Variable to store the player's tankPawn
    public GameObject tankPawnPrefabTwo;
    public GameObject playerSpawnTransform; // Variable to hold the player spawn location
    public bool isPlayerSpawned = false; // Boolean to check if the player was spawned, may need to be changed later to allow for multiplayer
    public GameObject[] enemyControllerPrefabs; // Variable to reference the AI controllers and their pawns
    //public GameObject enemyPawnPrefab; **Not used d/t controller being contained within enemy prefab, may need to be used later!**
    [HideInInspector] public PawnSpawnPoint currentSpawnPoint; // Variable to hold the current spawn location 
    private List<Transform> currentPatrolWaypoints;
    public MapGenerator mapGenerator; // Variable to reference the Map Generator
    public static GameManager instance; // Variable to reference the GameManager
    public List<PlayerController> players; // Creates a LIST of players, even if the game is going to be single player
    public List<AIController> enemies; // Creates a LIST of enemies based on how many AIControllers that were spawned in the scene
    public int pendingPlayers = 4;
    [HideInInspector] public bool isMultiplayer = false;
    #endregion Variables;
    
    #region Cameras
    /*public GameObject cameraPrefab;
    //[HideInInspector] public GameObject newCamera;
    public CameraController cameraControllerPrefab;
    private bool camerasAreSpawned = false;
    [HideInInspector] public List<GameObject> playerCameras;*/
    private GameObject playerOneCameraObj;
    private GameObject playerTwoCamera;
    #endregion Cameras

    #region Score
    public int highScore;
    public int[] playerScores;
    //public int player2Score = 0;
    //public int player3Score = 0;
    //public int player4Score = 0;
    #endregion Score

    #region Game States;
    public GameObject TitleScreenStateObject; // Title Screen STATE
    public GameObject MainMenuStateObject; // Main Menu Screen STATE
    public GameObject OptionsScreenStateObject; // Options Screen STATE
    public GameObject CreditsScreenStateObject; // Credits Screen STATE
    public GameObject GameplayStateObject; // Gameplay STATE!
    public GameObject GameOverScreenStateObject; // Game Over Screen STATE
    public GameObject PauseMenuSceenStateObject;
    public GameObject PauseMenuOptionsScreenStateObject;
    private bool gameplayIsDeactivated = true;
    private GameObject currentMap;
    #endregion Game States;
    
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
        ActivateMainMenuScreen();
    }
    
    private void Start()
    {
        
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

        if (isMultiplayer == false)
        {
            // Grab the child camera and assign it to player 2 camera
            playerOneCameraObj = newPawnObj.transform.GetChild(1).gameObject;
            Camera playerOneCamera = playerOneCameraObj.GetComponent<Camera>();
            // Set it to the entrie screen
            playerOneCamera.rect = new Rect(0.0f, 0.0f, 1.0f, 1.0f);
        }

        // Hook them up!
        newPlayerController.pawn = newPlayerPawn;
        newPlayerPawn.controller = newPlayerController;

        isPlayerSpawned = true;
        //SpawnPlayerCameras();
    }

    public void SpawnPlayerTwo(PawnSpawnPoint playerSpawn)
    {
        // New Player/Pawn Obj's are being contained as local variables in this function.
        GameObject newPlayerObj = Instantiate(playerControllerPrefab2, Vector3.zero, Quaternion.identity) as GameObject; // Quaternion.identity has something to do with the rotation
        GameObject newPawnObj = Instantiate(tankPawnPrefabTwo, playerSpawn.transform.position, Quaternion.identity) as GameObject;
        
        // Get the Player Controller component and the Pawn component
        Controller newPlayerController = newPlayerObj.GetComponent<Controller>();
        Pawn newPlayerPawn = newPawnObj.GetComponent<Pawn>();

        // For every player pawn spawned in the game, add a noise maker to it and set the volume to 3
        newPawnObj.AddComponent<NoiseMaker>();
        newPlayerPawn.noiseMaker = newPawnObj.GetComponent<NoiseMaker>();
        newPlayerPawn.noiseMakerVolume = 3;

        // Grab the child camera and assign it to player 2 camera
        //playerTwoCamera = newPawnObj.transform.GetChild(1).gameObject;

        // Hook them up!
        newPlayerController.pawn = newPlayerPawn;
        newPlayerPawn.controller = newPlayerController;

        isPlayerSpawned = true;
    }

    /*public void SpawnPlayerCameras()
    {
        if (camerasAreSpawned == false)
        {
            foreach (var player in players)
            {
                GameObject newCamera = Instantiate(cameraPrefab, player.pawn.transform.position, Quaternion.identity) as GameObject;
                CameraController newCameraController = newCamera.AddComponent<CameraController>();
                newCameraController.playerCamera = newCamera;
                newCameraController.targetPlayer = player.gameObject;
                newCameraController.offset.Set(0,7,-10);
                //cameraControllerPrefab.FindPlayer();
                foreach (var camera in playerCameras)
                {
                    player.pawn.playerCamera = camera;
                    if (playerCameras.Count > players.Count)
                    {
                        Destroy(camera);
                    }
                }
            }
        }
        camerasAreSpawned = true;
    }*/

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

    public void DetermineSpawnPoints()
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
                    SpawnPlayer(randomPlayerSpawnPoint);
                    if (isMultiplayer == true)
                    {
                        randomPlayerSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
                        SpawnPlayerTwo(randomPlayerSpawnPoint);
                    }
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

    public void OnPlayerDeath(Pawn player)
    {
        //Destroy(player.playerCamera);
        //camerasAreSpawned = false;
        if (isMultiplayer == false)
        {
            SpawnPlayer(currentSpawnPoint);
        }
        else if (isMultiplayer == true)
        {
            SpawnPlayerTwo(currentSpawnPoint);
        }
    }

    public GameObject RandomEnemyPrefab()
    {
        // pull random enemy obj's from the allotted array created by designers
        return enemyControllerPrefabs[Random.Range(0, enemyControllerPrefabs.Length)];
    }

    #region Game States;
    // Function to Deactivate all game states (such as Menu, Title, Gameplay, etc.)
    private void DeactivateAllStates()
    {
        // Deactivate all GAME states
        TitleScreenStateObject.SetActive(false);
        MainMenuStateObject.SetActive(false);
        OptionsScreenStateObject.SetActive(false);
        CreditsScreenStateObject.SetActive(false);
        //GameplayStateObject.SetActive(false);
        GameOverScreenStateObject.SetActive(false);
        PauseMenuSceenStateObject.SetActive(false);
        PauseMenuOptionsScreenStateObject.SetActive(false);
    }

    public void DeactiveGameplayState()
    {
        gameplayIsDeactivated = true;
        Destroy(currentMap);
        foreach (var player in players)
        {
            Destroy(player.pawn.gameObject);
            Destroy(player.gameObject);
            isPlayerSpawned = false;
            /*foreach (var camera in playerCameras)
            {
                Destroy(camera);
            }*/
        }

        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                Destroy(enemy.transform.parent.gameObject);
            }
        }
        GameplayStateObject.SetActive(false);
    }

    public void DeactivatePauseMenuState()
    {
        PauseMenuSceenStateObject.SetActive(false);
    }

    #region Game State Activators;
    // Also do the same to activate all states (:

    public void ActivateTitleScreen()
    {
        // Deactivate all states
        DeactivateAllStates();
        DeactiveGameplayState();
        DeactivatePauseMenuState();
        // Activate the title screen
        TitleScreenStateObject.SetActive(true); // Set activate activates that object
    }

    public void ActivateMainMenuScreen()
    {
        // Deactivate all states
        DeactiveGameplayState();
        DeactivateAllStates();
        DeactivatePauseMenuState();
        // Activate the title screen
        MainMenuStateObject.SetActive(true); // Set activate activates that object
        //Debug.Log("Main menu opened");
    }

    public void ActivateOptionsScreen()
    {
        // Deactivate all states
        //DeactivateAllStates();
        // Activate the title screen
        OptionsScreenStateObject.SetActive(true); // Set activate activates that object
    }

    public void ActivateCreditsScreen()
    {
        // Deactivate all states
        DeactivateAllStates();
        DeactiveGameplayState();
        // Activate the title screen
        CreditsScreenStateObject.SetActive(true); // Set activate activates that object
    }

    public void ActivateGameplay()
    {
        // Deactivate all states
        DeactivateAllStates();
        DeactivatePauseMenuState();
        // Activate the title screen

        // Generate NEW map if the Map Generator exists
        if (mapGenerator != null)
        {
            if (gameplayIsDeactivated == true)
            {
                GameplayStateObject.SetActive(true); // Set activate activates that object
                mapGenerator.SetMap();
                currentMap = mapGenerator.newGeneratedMapGameObject;
                DetermineSpawnPoints();
                //camerasAreSpawned = false;
                ResetScores();
                ResetLives();
                gameplayIsDeactivated = false;
            }
        }
    }

    public void ActivateGameOverScreen()
    {
        // Deactivate all states
        DeactivateAllStates();
        DeactiveGameplayState();
        // Activate the title screen
        GameOverScreenStateObject.SetActive(true); // Set activate activates that object
        // TODO: Set highScore and other scores in results
        CompareScoreValues();
    }

    public void ActivatePauseMenuScreen()
    {
        // Deactivate all states
        DeactivateAllStates();
        //GameplayStateObject.SetActive(false);
        // Activate the pause menu screen
        PauseMenuSceenStateObject.SetActive(true);
    }

    public void ActivatePauseMenuOptionsScreen()
    {
        // Deactivate all states
        DeactivateAllStates();
        DeactivatePauseMenuState();
        // Activate the pause menu screen
        PauseMenuOptionsScreenStateObject.SetActive(true);
    }

    #endregion Game State Activators;

    #endregion Game State;

    #region Score
    // TODO Create a function that resets the score for all players
    public void ResetScores()
    {
        foreach (var player in players)
        {
            player.playerScore = 0;
        }
    }

    // TODO Create a function that compares all score values in the scene
    public void CompareScoreValues()
    {
        int currentScore = playerScores[0];
        foreach (var scoreInt in playerScores)
        {
            if (scoreInt > 0)
            {
                if (currentScore < scoreInt)
                {
                    highScore = scoreInt;
                }
                else if (currentScore > scoreInt)
                {
                    highScore = currentScore;
                }
            }
        }
        Debug.Log("High score is " + highScore);
    }

    #endregion Score

    #region Lives

    public void ResetLives()
    {
        // TODO create a function that resets the lives variable for each player
        foreach (var player in players)
        {
            player.pawn.currentLives = player.pawn.maxLives;
        }
    }

    #endregion Lives
}

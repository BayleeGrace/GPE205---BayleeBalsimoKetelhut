using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapGenerator : MonoBehaviour
{

    #region Variables
    public GameObject[] gridPrefab;
    public int rows;
    public int cols;
    public float roomWidth = 50.0f;
    public float roomHeight = 50.0f;
    private Room[,] mapGrid;

    public List<PawnSpawnPoint> spawnPoints;

    #endregion Variables
    
    public void Start()
    {
        PawnSpawnPoint[] spawnPoint = FindObjectsOfType<PawnSpawnPoint>();
        foreach (var spawn in spawnPoint)
        {
            spawnPoints.Add(spawn);
        }
    }
    
    public void GenerateMap()
    {
        mapGrid = new Room[cols, rows];

        //Spawn room tiles
        // Randomly select room tiles, one grid coordinate at a time

        #region Spawn room tiles
        // for each grid row...
        for (int currentRow = 0; currentRow < rows; currentRow++)
        {
            // for each column in that row...
            for (int currentCol = 0; currentCol < cols; currentCol++)
            {
                // find the location
                float xPosition = roomWidth * currentCol;
                float zPosition = roomHeight * currentRow;
                Vector3 newPosition = new Vector3(xPosition, 0, zPosition);

                // create a room tile at that location
                GameObject tempRoomObj = Instantiate(RandomRoomPrefab(), newPosition, Quaternion.identity) as GameObject; // typecasting the new room, "cast is technically redundant, not needed"

                // set its parent
                tempRoomObj.transform.parent = transform; // Sets the MapGenerator as the parent of these new rooms

                // give it a meaningful name
                tempRoomObj.name = "Room_" + currentCol + "," + currentRow;

                // get the room.cs component from it
                Room tempRoom = tempRoomObj.GetComponent<Room>();

                // save it to the mapGrid array
                mapGrid[currentCol, currentRow] = tempRoom;

                #region Open/Close doors
                // Open interior door and close external doors

                // Check if North/South doors in the rows are exterior, if they are then close them
                if (currentRow == 0)
                {
                    tempRoom.doorNorth.SetActive(false);
                }
                else if (currentRow == rows - 1)
                {
                    tempRoom.doorSouth.SetActive(false);
                }
                else
                {
                    tempRoom.doorNorth.SetActive(false);
                    tempRoom.doorSouth.SetActive(false);
                }

                // Check if East/West doors in the cols are exterior, if they are then close them
                if (currentCol == 0)
                {
                    tempRoom.doorEast.SetActive(false);
                }
                else if (currentCol == cols - 1)
                {
                    tempRoom.doorWest.SetActive(false);
                }
                else
                {
                    tempRoom.doorEast.SetActive(false);
                    tempRoom.doorWest.SetActive(false);
                }

                #endregion Open/Close doors

            }
        }
        #endregion Spawn room tiles

    }

    public GameObject RandomRoomPrefab()
    {
        // pull random room objects
        return gridPrefab[Random.Range(0, gridPrefab.Length)];
    }

}

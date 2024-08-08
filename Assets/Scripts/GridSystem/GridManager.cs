using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class GridManager : MonoBehaviour
{
    public int roomSize = 8;
    public int maxRooms = 4;
    public float cellSize = 1.0f;
    public GameObject roomPrefab;
    public Transform leftWall;
    public Transform rightWall;
    public Transform leftWallAux;
    public Transform rightWallAux;

    public int gridSizeX = 8;
    public int gridSizeZ = 8;

    private Dictionary<Vector2Int, GameObject> roomFloors;
    public GameObject house;

    public Dictionary<Vector3, IPlaceableObject> furnitureObjects;
    public Dictionary<Vector3, IPlaceableObject> carpetObjects;

    private Vector3 houseOffset;
    private SaveManager saveManager;

    public int scenario = 1;

    private bool isRDUnlocked = false;
    private bool isLUUnlocked = false;

    void Start()
    {
        roomFloors = new Dictionary<Vector2Int, GameObject>();
        houseOffset = house.transform.position;
        InitializeGrid();
    }

    public void SetSaveManager(SaveManager saveManager)
    {
       this.saveManager = saveManager;
    }
    void InitializeGrid()
    {
        if (furnitureObjects == null)
            furnitureObjects = new Dictionary<Vector3, IPlaceableObject>();
        if (carpetObjects == null)
            carpetObjects = new Dictionary<Vector3, IPlaceableObject>();
    }

    public void InitializeDefaultScenario()
    {
        roomFloors = new Dictionary<Vector2Int, GameObject>();
        houseOffset = house.transform.position;
        InitializeGrid();

        AddRoom("L-D");
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && !isLUUnlocked) AddRoom("L-U");
        if (Input.GetKeyDown(KeyCode.Alpha2) && !isRDUnlocked) AddRoom("R-D");
        if (Input.GetKeyDown(KeyCode.Alpha3)) AddRoom("R-U");

        if (Input.GetKeyDown(KeyCode.M)) Debug.Log(carpetObjects.Count);
    }

    void CreateRoom(Vector2Int roomPosition, string roomName)
    {
        Vector3 position = new Vector3(roomPosition.x * roomSize, 0, roomPosition.y * roomSize) + houseOffset;
        GameObject newRoom = Instantiate(roomPrefab, position, Quaternion.identity, house.transform);
        newRoom.name = $"Room-{roomName}";

        roomFloors.Add(roomPosition, newRoom);
    }

    internal void InitializeFromSaveData(int currentScenario)
    {
        roomFloors = new Dictionary<Vector2Int, GameObject>();
        houseOffset = house.transform.position;
        InitializeGrid();

        switch (currentScenario)
        {
            case 1:
                AddRoom("L-D");
                break;
            case 2:
                AddRoom("L-D");
                AddRoom("R-D");
                break;
            case 3:
                AddRoom("L-D");
                AddRoom("L-U");
                break;
            case 4:
                AddRoom("L-D");
                AddRoom("R-D");
                AddRoom("L-U");
                break;
            case 5:
                AddRoom("L-D");
                AddRoom("R-D");
                AddRoom("L-U");
                AddRoom("R-U");
                break;
        }
    }

    public Dictionary<Vector3, IPlaceableObject> GetObjectDictionary(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.Furniture:
                return furnitureObjects;
            case ObjectType.Carpet:
                return carpetObjects;
            default:
                return null;
        }
    }

    void AddRoom(string room)
    {
        switch (room)
        {
            case "L-D":
                CreateRoom(Vector2Int.zero, "L-D");
                scenario = 1;
                gridSizeX = 8;
                gridSizeZ = 8;
                break;
            case "L-U":
                CreateRoom(new Vector2Int(0, 1), "L-U"); // L-U room
                if (isRDUnlocked)
                {
                    gridSizeX = 16;
                    gridSizeZ = 16;
                    scenario = 4;
                    saveManager.UpdateScenario(scenario);
                }
                else
                {
                    gridSizeX = 8;
                    gridSizeZ = 16;
                    scenario = 3;
                    saveManager.UpdateScenario(scenario);
                }
                isLUUnlocked = true;
                break;
            case "R-D":
                CreateRoom(new Vector2Int(1, 0), "R-D"); // R-D room
                if (!isLUUnlocked)
                {
                    gridSizeX = 16;
                    gridSizeZ = 8;
                    scenario = 2;
                    saveManager.UpdateScenario(scenario);
                }
                else
                {
                    gridSizeX = 16;
                    gridSizeZ = 16;
                    scenario = 4;
                    saveManager.UpdateScenario(scenario);
                }
                isRDUnlocked = true;
                break;
            case "R-U":
                if (isLUUnlocked && isRDUnlocked)
                {
                    CreateRoom(new Vector2Int(1, 1), "R-U"); // R-U room
                    scenario = 5;
                    gridSizeX = 16;
                    gridSizeZ = 16;
                    saveManager.UpdateScenario(scenario);
                }
                break;
        }
        ConfigureWalls(scenario);
    }


    void ConfigureWalls(int scenario)
    {
        // Make sure all walls are active
        leftWall.gameObject.SetActive(true);
        rightWall.gameObject.SetActive(true);
        leftWallAux.gameObject.SetActive(true);
        rightWallAux.gameObject.SetActive(true);

        switch (scenario)
        {
            case 1:
                // Scenario 1: Only L-D room
                ConfigureWall(leftWall, new Vector3(0, 2, 4) + houseOffset);
                ConfigureWall(rightWall, new Vector3(4, 2, 0) + houseOffset);
                leftWallAux.gameObject.SetActive(false);
                rightWallAux.gameObject.SetActive(false);
                break;

            case 2:
                // Scenario 2: L-D + R-D rooms
                ConfigureWall(leftWall, new Vector3(0, 2, 4) + houseOffset);
                ConfigureWall(rightWallAux, new Vector3(8, 2, 4) + houseOffset);
                ConfigureWall(rightWall, new Vector3(12, 2, 0) + houseOffset);
                leftWallAux.gameObject.SetActive(false);
                break;

            case 3:
                // Scenario 3: L-D + L-U rooms
                ConfigureWall(leftWall, new Vector3(0, 2, 12) + houseOffset);
                ConfigureWall(rightWall, new Vector3(4, 2, 0) + houseOffset);
                ConfigureWall(rightWallAux, new Vector3(4, 2, 8) + houseOffset);
                rightWallAux.gameObject.SetActive(false);
                break;

            case 4:
                // Scenario 5: L-D + L-U + R-D
                ConfigureWall(leftWall, new Vector3(0, 2, 12) + houseOffset);
                ConfigureWall(rightWallAux, new Vector3(8, 2, 4) + houseOffset);
                ConfigureWall(rightWall, new Vector3(12, 2, 0) + houseOffset);
                ConfigureWall(leftWallAux, new Vector3(4, 2, 8) + houseOffset);
                break;

            case 5:
                // Scenario 4: L-D + L-U + R-D + R-U rooms
                ConfigureWall(leftWall, new Vector3(0, 2, 12) + houseOffset);
                ConfigureWall(leftWallAux, new Vector3(8, 2, 12) + houseOffset);
                leftWallAux.Rotate(Vector3.up, -90);
                ConfigureWall(rightWall, new Vector3(12, 2, 0) + houseOffset);
                ConfigureWall(rightWallAux, new Vector3(12, 2, 8) + houseOffset);
                rightWallAux.Rotate(Vector3.up, 90);
                break;
        }
    }

    void ConfigureWall(Transform wall, Vector3 position)
    {
        wall.position = position;
    }
}

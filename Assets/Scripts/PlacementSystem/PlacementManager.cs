using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public GridManager gridManager;
    public GameData gameData;
    public GameObject placementIndicatorPrefab;
    private List<GameObject> placementIndicators = new List<GameObject>();

    private float cellSize;
    private List<PlacedItemData> placedItems;

    private SaveManager saveManager;

    public List<PlacedItemData> GetPlacedItems()
    {
        return placedItems;
    }

    void Start()
    {
        InitializePlacementIndicators();
        cellSize = gridManager.cellSize;

        if (placedItems == null)
            placedItems = new List<PlacedItemData>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log(placedItems.Count);
        }
    }

    public void SetSaveManager(SaveManager saveManager)
    {
        this.saveManager = saveManager;
    }

    private void InitializePlacementIndicators()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject indicator = Instantiate(placementIndicatorPrefab);
            indicator.SetActive(false);
            placementIndicators.Add(indicator);
        }
    }

    public bool CanPlaceObject(IPlaceableObject obj, Vector3 position)
    {
        return obj.CanPlace(gridManager, position);
    }

    public void PlaceObject(IPlaceableObject obj, Vector3 position)
    {
        if (CanPlaceObject(obj, position))
        {
            obj.Place(gridManager, position);
            MarkCellsOccupied(obj);

            PlacedItemData placedItemData = new PlacedItemData
            {
                ownerId = obj.GetOwnerId(),
                objectType = (int)obj.GetObjectType(),
                itemId = obj.GetId(),
                posX = (int)position.x,
                posZ = (int)position.z,
                rotation = obj.Orientation
            };
            AddPlacedItem(placedItemData);
        }

        SavePlacedItems();
    }

    public void MoveObject(IPlaceableObject obj, Vector3 newPosition)
    {
        var objectDict = gridManager.GetObjectDictionary(obj.GetObjectType());

        // Remove old occupied cells
        foreach (var cell in obj.GetOccupiedCells(obj.Position))
        {
            objectDict.Remove(cell);
        }

        // Place in the new position
        obj.Move(newPosition);
        if (CanPlaceObject(obj, newPosition))
        {
            UpdatePlacedItemData(obj);
            MarkCellsOccupied(obj);
            SavePlacedItems();
        }
        else
        {
            obj.Move(obj.Position); // Move back to the original position if placement is not possible
        }
    }

    public void RotateObject(IPlaceableObject obj)
    {
        obj.Rotate(gridManager);
        UpdatePlacementIndicator(obj, obj.Position);
        UpdatePlacedItemData(obj);
        SavePlacedItems();
    }

    public void AddToDictionary(IPlaceableObject obj, Vector3 position, Dictionary<Vector3, IPlaceableObject> dict)
    {
        dict[position] = obj;
    }

    public void RemoveFromDictionary(Vector3 position, Dictionary<Vector3, IPlaceableObject> dict)
    {
        if (dict.ContainsKey(position))
        {
            var obj = dict[position];
            RemovePlacedItem(obj.GetPlacedItemId());
            dict.Remove(position);
        }
    }

    public void UpdatePlacementIndicator(IPlaceableObject obj, Vector3 position)
    {
        var occupiedCells = obj.GetOccupiedCells(position);
        bool canPlace = CanPlaceObject(obj, position);

        for (int i = 0; i < placementIndicators.Count; i++)
        {
            if (i < occupiedCells.Count)
            {
                placementIndicators[i].transform.position = occupiedCells[i];
                Vector3 newPos = placementIndicators[i].transform.position;
                newPos.y = 0.02f;
                newPos += new Vector3(cellSize / 2f, 0, cellSize / 2f);
                placementIndicators[i].transform.position = newPos;
                placementIndicators[i].SetActive(true);
                var renderer = placementIndicators[i].GetComponent<Renderer>();
                renderer.material.color = canPlace ? Color.green : Color.red;
            }
            else
            {
                placementIndicators[i].SetActive(false);
            }
        }
    }

    public void HidePlacementIndicators()
    {
        foreach (var indicator in placementIndicators)
        {
            indicator.SetActive(false);
        }
    }

    private void AddPlacedItem(PlacedItemData data)
    {
        placedItems.Add(data);
    }

    private void RemovePlacedItem(int placedItemId)
    {
        if (placedItemId >= 0 && placedItemId < placedItems.Count)
        {
            placedItems[placedItemId] = null; // or a special "removed" marker
        }
    }

    private void UpdatePlacedItemData(IPlaceableObject obj)
    {
        int placedItemId = obj.GetPlacedItemId();
        if (placedItemId >= 0 && placedItemId < placedItems.Count)
        {
            var placedItemData = placedItems[placedItemId];
            placedItemData.posX = (int)obj.Position.x;
            placedItemData.posZ = (int)obj.Position.z;
            placedItemData.rotation = obj.Orientation;
        }
    }

    private void MarkCellsOccupied(IPlaceableObject obj)
    {
        var occupiedCells = obj.GetOccupiedCells(obj.Position);
        var objectDict = gridManager.GetObjectDictionary(obj.GetObjectType());

        foreach (var cell in occupiedCells)
        {
            objectDict[cell] = obj;
        }
    }

    public void SavePlacedItems()
    {
        saveManager.SavePlacedItems(placedItems);
    }

    public void LoadPlacedItems()
    {
        placedItems = saveManager.GetSaveData().placedItems;

        saveManager.SetIsSaveEnabled(false);

        foreach (var placedItemData in placedItems)
        {
            if (placedItemData == null) continue;

            Vector3 position = new Vector3(placedItemData.posX, 0, placedItemData.posZ);

            GameObject prefab = gameData.GetItemById(placedItemData.itemId);
            if (prefab != null)
            {
                GameObject newObj = Instantiate(prefab);
                var placeableObject = newObj.GetComponent<IPlaceableObject>();

                placeableObject.SetOwnerId(placedItemData.ownerId);
                placeableObject.Orientation = placedItemData.rotation;
                placeableObject.Place(gridManager, position);
                placeableObject.RotateBack(placedItemData.rotation);

                newObj.transform.position = position;
            }
        }

        saveManager.SetIsSaveEnabled(true);
    }

    private Dictionary<Vector3, IPlaceableObject> GetDictionaryForObjectType(int objectType)
    {
        return gridManager.GetObjectDictionary((ObjectType)objectType);
    }
}

using System.Collections.Generic;
using UnityEngine;

public class PlacementManager : MonoBehaviour
{
    public GridManager gridManager;
    public GameObject placementIndicatorPrefab;
    private List<GameObject> placementIndicators = new List<GameObject>();

    private float cellsize;

    void Start()
    {
        InitializePlacementIndicators();
        cellsize = gridManager.cellSize;
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
        }
    }

    public void MoveObject(IPlaceableObject obj, Vector3 newPosition)
    {
        Vector3 originalPosition = obj.Position;
        obj.Move(newPosition);
        if (!CanPlaceObject(obj, newPosition))
        {
            obj.Move(originalPosition);
        }
    }

    public void RotateObject(IPlaceableObject obj)
    {
        int originalOrientation = obj.Orientation;
        obj.Rotate(gridManager);
        UpdatePlacementIndicator(obj, obj.Position);

        if (!CanPlaceObject(obj, obj.Position))
        {
            obj.RotateBack(originalOrientation);
        }
    }

    public void AddToDictionary(IPlaceableObject obj, Vector3 position, Dictionary<Vector3, IPlaceableObject> dict)
    {
        dict[position] = obj;
    }

    public void RemoveFromDictionary(Vector3 position, Dictionary<Vector3, IPlaceableObject> dict)
    {
        if (dict.ContainsKey(position))
        {
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
                newPos += new Vector3(cellsize / 2f, 0, cellsize / 2f);
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
}

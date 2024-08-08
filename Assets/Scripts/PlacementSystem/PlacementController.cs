using System.Collections.Generic;
using UnityEngine;

public class PlacementController : MonoBehaviour
{
    public PlacementManager placementManager;
    private GameObject currentObject;
    private IPlaceableObject currentPlaceableObject;

    public void StartPlacingObject(GameObject prefab)
    {
        if (currentObject != null)
        {
            Destroy(currentObject);
        }

        currentObject = Instantiate(prefab, new Vector3(5, 0, 5), Quaternion.identity);
        currentPlaceableObject = currentObject.GetComponent<IPlaceableObject>();

        GameManager.EnterEditMode();
    }

    public void MoveObject()
    {
        if (!GameManager.IsEditMode() || currentObject == null || currentPlaceableObject == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 newPosition = new Vector3(hit.point.x, 0, hit.point.z);
            Vector3 snappedPosition = currentPlaceableObject.SnapToGrid(newPosition);
            currentObject.transform.position = snappedPosition;
            placementManager.UpdatePlacementIndicator(currentPlaceableObject, snappedPosition);
        }
    }

    public void TryPlaceObject()
    {
        if (!GameManager.IsEditMode() || currentObject == null || currentPlaceableObject == null) return;

        Vector3 position = currentPlaceableObject.SnapToGrid(currentObject.transform.position);
        if (placementManager.CanPlaceObject(currentPlaceableObject, position))
        {
            placementManager.PlaceObject(currentPlaceableObject, position);
            ClearCurrentPlacement();
            placementManager.HidePlacementIndicators();
            GameManager.ExitEditMode();
        }
        else
        {
            Debug.Log("Can't place here");
        }
    }

    public void ClearCurrentPlacement()
    {
        if (currentObject != null)
        {
            // Optionally, you can destroy the current object here
            // Destroy(currentObject);
        }
        currentObject = null;
        currentPlaceableObject = null;
    }

    public void RotateObject()
    {
        if (!GameManager.IsEditMode() || currentPlaceableObject == null) return;

        placementManager.RotateObject(currentPlaceableObject);
        placementManager.UpdatePlacementIndicator(currentPlaceableObject, currentPlaceableObject.Position);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Right-click
        {
            RotateObject();
        }
    }
}
using UnityEngine;

public class PlacementController : MonoBehaviour
{
    public PlacementManager placementManager;
    private GameObject currentObject;
    private Furniture currentFurniture;

    public void StartPlacingObject(GameObject prefab)
    {
        if (currentObject != null)
        {
            Destroy(currentObject);
        }

        currentObject = Instantiate(prefab, new Vector3(5, 0.5f, 5), Quaternion.identity);
        currentFurniture = currentObject.GetComponent<Furniture>();
    }

    public void MoveObject()
    {
        if (currentObject == null || currentFurniture == null) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 newPosition = new Vector3(hit.point.x, 0.5f, hit.point.z);
            Vector3 snappedPosition = currentFurniture.SnapToGrid(newPosition);
            currentObject.transform.position = snappedPosition;
            placementManager.UpdatePlacementIndicator(currentFurniture, snappedPosition);
        }
    }

    public void TryPlaceObject()
    {
        if (currentObject == null || currentFurniture == null) return;

        Vector3 position = currentFurniture.SnapToGrid(currentObject.transform.position);
        if (placementManager.CanPlaceObject(currentFurniture, position))
        {
            placementManager.PlaceObject(currentFurniture, position);
            ClearCurrentPlacement();
            placementManager.HidePlacementIndicators();
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
            //Destroy(currentObject);
        }
        currentObject = null;
        currentFurniture = null;
    }
}

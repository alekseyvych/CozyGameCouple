using UnityEngine;

public class TestGridSetup : MonoBehaviour
{
    public GridManager gridManager;
    public TileEditorManager tileEditorManager;
    public PlacementController placementController;
    public GameObject furniturePrefab;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            placementController.StartPlacingObject(furniturePrefab);
        }

        if (placementController != null)
        {
            placementController.MoveObject();
            if (Input.GetMouseButtonDown(0))
            {
                placementController.TryPlaceObject();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            ToggleFloorEditingMode();
        }
    }

    void ToggleFloorEditingMode()
    {
        if (tileEditorManager != null)
        {
            if (tileEditorManager.isEditing)
            {
                tileEditorManager.StopEditing();
            }
            else
            {
                tileEditorManager.StartEditing();
            }
        }
    }
}

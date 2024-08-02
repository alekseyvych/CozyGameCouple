using UnityEngine;

public class TileEditorManager : MonoBehaviour
{
    public GameObject tilePrefab; // 1x1 tile prefab with the new texture
    public Material newTileMaterial; // The material to be applied when editing tiles

    private GameObject currentTile; // The tile object currently being moved
    private Vector3 tilePosition;

    public bool isEditing = false;
    private LayerMask floorLayerMask;

    void Start()
    {
        floorLayerMask = LayerMask.GetMask("Floor");
    }

    void Update()
    {
        if (isEditing && currentTile != null)
        {
            MoveTile();
            if (Input.GetMouseButtonDown(0))
            {
                ApplyTileTexture();
            }
        }
    }

    public void StartEditing()
    {
        if (currentTile == null)
        {
            currentTile = Instantiate(tilePrefab);
            currentTile.GetComponent<Renderer>().material = newTileMaterial;
        }
        isEditing = true;
    }

    public void StopEditing()
    {
        if (currentTile != null)
        {
            Destroy(currentTile);
        }
        isEditing = false;
    }

    void MoveTile()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, floorLayerMask))
        {
            tilePosition = new Vector3(Mathf.Round(hit.point.x), 0.05f, Mathf.Round(hit.point.z));
            currentTile.transform.position = tilePosition;
        }
    }

    void ApplyTileTexture()
    {
        Collider[] colliders = Physics.OverlapBox(tilePosition, new Vector3(0.5f, 0.1f, 0.5f), Quaternion.identity, floorLayerMask);
        foreach (var collider in colliders)
        {
            Renderer renderer = collider.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = newTileMaterial;
            }
        }
    }
}

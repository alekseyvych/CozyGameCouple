using System.Collections.Generic;
using UnityEngine;

public class Carpet : MonoBehaviour, IPlaceableObject
{
    public Sprite[] Sprites;
    [SerializeField]
    private Vector3 carpetSize = new Vector3(1, 1, 1);

    public Vector3 Position { get; private set; }
    public Vector3 Size { get; private set; }
    public int Orientation { get; set; }
    public ObjectType Type { get; private set; }
    public List<Vector3> OccupiedCells { get; private set; }

    private float cellSize = 1.0f;

    public int id;
    public int price;
    public string objectName;
    public Sprite previewSprite;
    public int OwnerId;
    public int placedItemId;

    void Start()
    {
        Initialize(ObjectType.Carpet, carpetSize);
    }

    public void Initialize(ObjectType type, Vector3 size)
    {
        Type = type;
        Size = size;
        OccupiedCells = new List<Vector3>();
        Orientation = 0;
    }

    public Vector3 SnapToGrid(Vector3 position)
    {
        float x = Mathf.Round(position.x / cellSize) * cellSize;
        float z = Mathf.Round(position.z / cellSize) * cellSize;
        return new Vector3(x, position.y, z);
    }

    public bool CanPlace(GridManager gridManager, Vector3 position)
    {
        var occupiedCells = GetOccupiedCells(position);
        foreach (var cell in occupiedCells)
        {
            if (cell.x < 0 || cell.x >= gridManager.gridSizeX ||
                cell.z < 0 || cell.z >= gridManager.gridSizeZ ||
                gridManager.carpetObjects.ContainsKey(cell))
            {
                return false;
            }
        }
        return true;
    }

    public void Place(GridManager gridManager, Vector3 position)
    {
        Position = position;
        UpdateOccupiedCells();
        foreach (var cell in OccupiedCells)
        {
            gridManager.carpetObjects[cell] = this;
        }
    }

    public void Move(Vector3 newPosition)
    {
        Position = newPosition;
        UpdateOccupiedCells();
    }

    public void Rotate(GridManager gridManager)
    {
        int originalOrientation = Orientation;
        Orientation = (Orientation + 1) % Sprites.Length;
        UpdateSprite();
        UpdateOccupiedCells();

        if (!CanPlace(gridManager, Position))
        {
            RotateBack(originalOrientation);
        }
    }

    public void RotateBack(int originalOrientation)
    {
        Debug.Log($"Rotating back to: {originalOrientation}");
        Orientation = originalOrientation;
        Debug.Log($"Rotating to: {Orientation}");
        UpdateSprite();
        UpdateOccupiedCells();
    }

    public void UpdateSprite()
    {
        Debug.Log($"Updating sprite to: {Orientation}");
        var spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = Sprites[Orientation];
        Debug.Log($"Sprite updated to: {Sprites[Orientation].texture.name}");
    }

    public List<Vector3> GetOccupiedCells(Vector3 position)
    {
        var occupiedCells = new List<Vector3>();
        Vector3 size = carpetSize;

        if (Orientation == 1 || Orientation == 3) // 90 or 270 degrees
        {
            size = new Vector3(Size.z, Size.y, Size.x);
        }

        for (int x = 0; x < size.x; x++)
        {
            for (int y = 0; y < size.y; y++)
            {
                for (int z = 0; z < size.z; z++)
                {
                    Vector3 offset = new Vector3(x, y, z);
                    occupiedCells.Add(position + offset);
                }
            }
        }
        return occupiedCells;
    }

    protected void UpdateOccupiedCells()
    {
        OccupiedCells = GetOccupiedCells(Position);
    }

    // Interface methods implementation
    public int GetId() => id;
    public int GetOwnerId() => OwnerId;
    public void SetOwnerId(int id) => OwnerId = id;
    public int GetPlacedItemId() => placedItemId;
    public void SetPlacedItemId(int placedItemId) => this.placedItemId = placedItemId;
    public ObjectType GetObjectType() => ObjectType.Carpet;
    public int GetPrice() => price;
    public string GetName() => objectName;
    public Sprite GetPreviewSprite() => previewSprite;
}

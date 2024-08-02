using System.Collections.Generic;
using UnityEngine;

public class Carpet : MonoBehaviour, IPlaceableObject
{
    public Sprite[] Sprites;
    [SerializeField]
    private Vector3 carpetSize = new Vector3(1, 1, 1);

    public Vector3 Position { get; private set; }
    public Vector3 Size { get; private set; }
    public int Orientation { get; private set; }
    public ObjectType Type { get; private set; }
    public List<Vector3> OccupiedCells { get; private set; }

    [SerializeField]
    public int Price { get; private set; }  // Added Price for shop functionality

    public int price;
    private float cellSize = 1.0f;

    [SerializeField]
    public Sprite previewSprite;

    void Start()
    {
        Initialize(ObjectType.Carpet, carpetSize, 100); // Assuming a default price of 100 for now
        UpdateSprite();
    }

    public void Initialize(ObjectType type, Vector3 size, int price)
    {
        Type = type;
        Size = size;
        Price = price;  // Initialize price
        OccupiedCells = new List<Vector3>();
        Orientation = 0; // Default orientation
    }

    public bool CanPlace(GridManager gridManager, Vector3 position)
    {
        var occupiedCells = GetOccupiedCells(position);
        foreach (var cell in occupiedCells)
        {
            if (cell.x < 0 || cell.x >= gridManager.gridSizeX ||
                cell.z < 0 || cell.z >= gridManager.gridSizeZ)
            {
                return false;
            }

            if (gridManager.scenario == 4)
            {
                if (cell.z > gridManager.gridSizeZ / 2 - 1 && cell.x > gridManager.gridSizeX / 2 - 1)
                {
                    return false;
                }
            }

            if (gridManager.floorObjects.ContainsKey(cell))
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
        var dict = gridManager.floorObjects;
        foreach (var cell in OccupiedCells)
        {
            dict[cell] = this;
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
        Orientation = (Orientation + 1) % 4; // Rotate to the next orientation
        UpdateOccupiedCells();
        UpdateSprite();

        if (!CanPlace(gridManager, Position))
        {
            RotateBack(originalOrientation);
        }
    }

    public void RotateBack(int originalOrientation)
    {
        Orientation = originalOrientation;
        UpdateOccupiedCells();
        UpdateSprite();
    }

    public List<Vector3> GetOccupiedCells(Vector3 position)
    {
        var occupiedCells = new List<Vector3>();
        Vector3 size = Size;

        // Adjust size based on current orientation
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

    protected bool IsValidPosition(GridManager gridManager, Vector3 position, Dictionary<Vector3, IPlaceableObject> dict)
    {
        if (position.x < 0 || position.x >= gridManager.gridSizeX ||
            position.z < 0 || position.z >= gridManager.gridSizeZ)
        {
            return false;
        }

        if (dict.ContainsKey(position))
        {
            return false;
        }

        return true;
    }

    public void UpdateSprite()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && Sprites.Length > 0)
        {
            spriteRenderer.sprite = Sprites[Orientation];
        }
    }

    public Vector3 SnapToGrid(Vector3 position)
    {
        float x = Mathf.Round(position.x / cellSize) * cellSize;
        float z = Mathf.Round(position.z / cellSize) * cellSize;
        return new Vector3(x, position.y, z);
    }

    // Additional methods related to shop and inventory can be added here if needed
}

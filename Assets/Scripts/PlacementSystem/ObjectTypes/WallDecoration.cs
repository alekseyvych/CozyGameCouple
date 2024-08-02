using System.Collections.Generic;
using UnityEngine;

public class WallDecoration : MonoBehaviour//, IPlaceableObject
{/*
    public Sprite[] Sprites; // Array to hold the sprites for different rotations

    [SerializeField]
    private Vector3 wallDecorationSize = new Vector3(1, 1, 0.1f);

    public Vector3 Position { get; private set; }
    public Vector3 Size { get; private set; }
    public Quaternion Rotation { get; private set; }
    public ObjectType Type { get; private set; }
    public List<Vector3> OccupiedCells { get; private set; }

    void Start()
    {
        Initialize(ObjectType.WallDecoration, wallDecorationSize);
        UpdateSprite();
    }

    private void Initialize(ObjectType type, Vector3 size)
    {
        Type = type;
        Size = size;
        OccupiedCells = new List<Vector3>();
        Rotation = Quaternion.Euler(0, 0, 0);
    }

    public bool CanPlace(GridManager gridManager, Vector3 position)
    {
        var newOccupiedCells = GetOccupiedCells(position, Rotation);
        foreach (var cell in newOccupiedCells)
        {
            if (gridManager.leftWallObjects.ContainsKey(cell) || gridManager.rightWallObjects.ContainsKey(cell))
            {
                return false;
            }
        }
        return true;
    }

    public void Place(GridManager gridManager, Vector3 position)
    {

    }

    public void Move(Vector3 newPosition)
    {
        Position = newPosition;
        UpdateOccupiedCells();
    }

    public void Rotate(GridManager gridManager)
    {
        // Wall decorations do not rotate
    }

    public void RotateBack(Quaternion originalRotation)
    {
        Rotation = originalRotation;
        UpdateOccupiedCells();
    }

    public void UpdateSprite()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null && Sprites.Length > 0)
        {
            int rotationIndex = Mathf.RoundToInt(Rotation.eulerAngles.y / 90) % Sprites.Length;
            spriteRenderer.sprite = Sprites[rotationIndex];
        }
    }

    private List<Vector3> GetOccupiedCells(Vector3 position, Quaternion rotation)
    {
        var occupiedCells = new List<Vector3>();
        for (int x = 0; x < Size.x; x++)
        {
            for (int y = 0; y < Size.y; y++)
            {
                for (int z = 0; z < Size.z; z++)
                {
                    Vector3 offset = new Vector3(x, y, z);
                    Vector3 rotatedOffset = RotatePointAroundPivot(offset, Vector3.zero, rotation.eulerAngles);
                    occupiedCells.Add(position + rotatedOffset);
                }
            }
        }
        return occupiedCells;
    }

    private Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
    {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }

    private void UpdateOccupiedCells()
    {
        OccupiedCells = GetOccupiedCells(Position, Rotation);
    }

    private bool IsValidPosition(GridManager gridManager, Vector3 position)
    {
        if (position.x < 0 || position.x >= gridManager.gridSizeX || position.z < 0 || position.z >= gridManager.gridSizeZ || position.y < 0 || position.y >= gridManager.gridSizeY)
        {
            return false;
        }

        if (gridManager.leftWallObjects.ContainsKey(position) || gridManager.rightWallObjects.ContainsKey(position))
        {
            return false;
        }

        return true;
    }*/
}

using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    Furniture,
    Carpet,
    WallDecoration,
    FloorDecoration
}

public interface IPlaceableObject
{
    Vector3 Position { get; }
    Vector3 Size { get; }
    int Orientation { get; }
    ObjectType Type { get; }
    List<Vector3> OccupiedCells { get; }
    int Price { get; }

    bool CanPlace(GridManager gridManager, Vector3 position);
    void Place(GridManager gridManager, Vector3 position);
    void Move(Vector3 newPosition);
    void Rotate(GridManager gridManager);
    void RotateBack(int originalOrientation); // Updated to use orientation
    void UpdateSprite();
    List<Vector3> GetOccupiedCells(Vector3 position); // Removed rotation parameter
}

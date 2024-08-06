using UnityEngine;
using System.Collections.Generic;

public enum ObjectType
{
    Furniture,
    Carpet,
    Wall,
    Floor
}

public interface IPlaceableObject
{
    Vector3 Position { get; }
    Vector3 Size { get; }
    public int Orientation { get; set;}
    ObjectType Type { get; }
    List<Vector3> OccupiedCells { get; }
    bool CanPlace(GridManager gridManager, Vector3 position);
    void Place(GridManager gridManager, Vector3 position);
    void Move(Vector3 newPosition);
    void Rotate(GridManager gridManager);
    void RotateBack(int originalOrientation);
    void UpdateSprite();
    int GetId();
    int GetOwnerId();
    void SetOwnerId(int id);
    int GetPlacedItemId();
    void SetPlacedItemId(int id);
    ObjectType GetObjectType();
    int GetPrice();
    string GetName();
    Sprite GetPreviewSprite();
    Vector3 SnapToGrid(Vector3 position);
    List<Vector3> GetOccupiedCells(Vector3 position);
}

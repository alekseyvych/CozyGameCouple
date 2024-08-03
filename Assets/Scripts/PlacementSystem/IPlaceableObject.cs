using System.Collections.Generic;
using UnityEngine;

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
    int Orientation { get; }
    ObjectType Type { get; }
    List<Vector3> OccupiedCells { get; }
    bool CanPlace(GridManager gridManager, Vector3 position);
    void Place(GridManager gridManager, Vector3 position);
    void Move(Vector3 newPosition);
    void Rotate(GridManager gridManager);
    void RotateBack(int originalOrientation);
    void UpdateSprite();
    public int GetId();
    public ObjectType GetObjectType();
    public int GetPrice();
    public string GetName();
    public Sprite GetPreviewSprite();

    List<Vector3> GetOccupiedCells(Vector3 position);
}

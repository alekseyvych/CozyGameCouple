using System.Collections.Generic;

[System.Serializable]
public class PlacedItemData
{
    public int itemId;
    public int ownerId;
    public int rotation;
    public int gridId;
    public int objectType; // 0 -> furniture, 1 -> carpet, 2 -> wall, 3 -> floor
    public int posX;
    public int posZ;

    public override string ToString()
    {
        return $"PlacedItemData: [ItemId: {itemId}, OwnerId: {ownerId}, Rotation: {rotation}, " +
               $"GridId: {gridId}, ObjectType: {objectType}, Position: ({posX}, {posZ})]";
    }
}

[System.Serializable]
public class SaveData
{
    public int money;
    public SerializableDictionary<int, int> furnitureItemCounts = new SerializableDictionary<int, int>();
    public SerializableDictionary<int, int> carpetItemCounts = new SerializableDictionary<int, int>();
    public SerializableDictionary<int, int> wallItemCounts = new SerializableDictionary<int, int>();
    public SerializableDictionary<int, int> floorItemCounts = new SerializableDictionary<int, int>();
    public List<PlacedItemData> placedItems = new List<PlacedItemData>();
    public int currentScenario;

    public override string ToString()
    {
        string result = $"Money: {money}\nCurrent Scenario: {currentScenario}\n";
        result += "Furniture Item Counts:\n";
        foreach (var item in furnitureItemCounts)
        {
            result += $"- ID: {item.Key}, Count: {item.Value}\n";
        }
        result += "Carpet Item Counts:\n";
        foreach (var item in carpetItemCounts)
        {
            result += $"- ID: {item.Key}, Count: {item.Value}\n";
        }
        result += "Wall Item Counts:\n";
        foreach (var item in wallItemCounts)
        {
            result += $"- ID: {item.Key}, Count: {item.Value}\n";
        }
        result += "Floor Item Counts:\n";
        foreach (var item in floorItemCounts)
        {
            result += $"- ID: {item.Key}, Count: {item.Value}\n";
        }
        result += "Placed Items:\n";
        foreach (var placedItem in placedItems)
        {
            result += $"- ItemID: {placedItem.itemId}, OwnerID: {placedItem.ownerId}, Rotation: {placedItem.rotation}, GridID: {placedItem.gridId}, Position: ({placedItem.posX}, {placedItem.posZ})\n";
        }
        return result;
    }
}

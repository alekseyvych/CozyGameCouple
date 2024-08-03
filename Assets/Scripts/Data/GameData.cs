using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    public List<GameObject> furnitureItems;
    public List<GameObject> carpetItems;
    public List<GameObject> wallItems;
    public List<GameObject> floorItems;

    // Separate dictionaries for item counts based on type
    public Dictionary<int, int> furnitureItemCounts = new Dictionary<int, int>();
    public Dictionary<int, int> carpetItemCounts = new Dictionary<int, int>();
    public Dictionary<int, int> wallItemCounts = new Dictionary<int, int>();
    public Dictionary<int, int> floorItemCounts = new Dictionary<int, int>();

    private SaveDataManager saveDataManager;

    public void Initialize(SaveDataManager saveDataManager)
    {
        this.saveDataManager = saveDataManager;
        LoadInventoryData(saveDataManager.GetSaveData());
    }

    public void AddItem(int itemID, ObjectType type)
    {
        var itemCounts = GetItemCountDictionary(type);

        if (itemCounts.ContainsKey(itemID))
            itemCounts[itemID]++;
        else
            itemCounts[itemID] = 1;

        saveDataManager.UpdateInventory(MergeItemCounts());
    }

    public void RemoveItem(int itemID, ObjectType type)
    {
        var itemCounts = GetItemCountDictionary(type);

        if (itemCounts.ContainsKey(itemID) && itemCounts[itemID] > 0)
        {
            itemCounts[itemID]--;
            saveDataManager.UpdateInventory(MergeItemCounts());
        }
    }

    public int GetItemCount(int itemID, ObjectType type)
    {
        var itemCounts = GetItemCountDictionary(type);

        if (itemCounts.ContainsKey(itemID))
            return itemCounts[itemID];

        return 0;
    }

    public void LoadInventoryData(SaveData saveData)
    {
        // Clear existing data
        furnitureItemCounts.Clear();
        carpetItemCounts.Clear();
        wallItemCounts.Clear();
        floorItemCounts.Clear();

        // Load data into the appropriate dictionaries
        foreach (var item in saveData.inventoryItems)
        {
            if (furnitureItems.Exists(x => x.GetComponent<IPlaceableObject>().GetId() == item.Key))
                furnitureItemCounts[item.Key] = item.Value;
            else if (carpetItems.Exists(x => x.GetComponent<IPlaceableObject>().GetId() == item.Key))
                carpetItemCounts[item.Key] = item.Value;
            else if (wallItems.Exists(x => x.GetComponent<IPlaceableObject>().GetId() == item.Key))
                wallItemCounts[item.Key] = item.Value;
            else if (floorItems.Exists(x => x.GetComponent<IPlaceableObject>().GetId() == item.Key))
                floorItemCounts[item.Key] = item.Value;
        }
    }

    private Dictionary<int, int> GetItemCountDictionary(ObjectType type)
    {
        switch (type)
        {
            case ObjectType.Furniture:
                return furnitureItemCounts;
            case ObjectType.Carpet:
                return carpetItemCounts;
            case ObjectType.Wall:
                return wallItemCounts;
            case ObjectType.Floor:
                return floorItemCounts;
            default:
                return null;
        }
    }

    public List<GameObject> GetCategoryItemsInInventory(ObjectType category)
    {
        List<GameObject> items = new List<GameObject>();
        
        switch (category)
        {
            case ObjectType.Furniture:
                foreach (var item in furnitureItemCounts)
                {
                    var itemObject = furnitureItems.Find(x => x.GetComponent<IPlaceableObject>().GetId() == item.Key);
                    if (itemObject != null)
                        items.Add(itemObject);
                }
                break;
            case ObjectType.Carpet:
                foreach (var item in carpetItemCounts)
                {
                    var itemObject = carpetItems.Find(x => x.GetComponent<IPlaceableObject>().GetId() == item.Key);
                    if (itemObject != null)
                        items.Add(itemObject);
                }
                break;
            case ObjectType.Wall:
                foreach (var item in wallItemCounts)
                {
                    var itemObject = wallItems.Find(x => x.GetComponent<IPlaceableObject>().GetId() == item.Key);
                    if (itemObject != null)
                        items.Add(itemObject);
                }
                break;

            case ObjectType.Floor:
                foreach (var item in floorItemCounts)
                {
                    var itemObject = floorItems.Find(x => x.GetComponent<IPlaceableObject>().GetId() == item.Key);
                    if (itemObject != null)
                        items.Add(itemObject);
                }
                break;
        }
        return items;
    }

    private Dictionary<int, int> MergeItemCounts()
    {
        var mergedItemCounts = new Dictionary<int, int>();

        foreach (var item in furnitureItemCounts)
            mergedItemCounts[item.Key] = item.Value;

        foreach (var item in carpetItemCounts)
            mergedItemCounts[item.Key] = item.Value;

        foreach (var item in wallItemCounts)
            mergedItemCounts[item.Key] = item.Value;

        foreach (var item in floorItemCounts)
            mergedItemCounts[item.Key] = item.Value;

        return mergedItemCounts;
    }
}

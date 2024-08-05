using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameData", menuName = "ScriptableObjects/GameData", order = 1)]
public class GameData : ScriptableObject
{
    public List<GameObject> furnitureItems;
    public List<GameObject> carpetItems;
    public List<GameObject> wallItems;
    public List<GameObject> floorItems;

    public SerializableDictionary<int, int> furnitureItemCounts = new SerializableDictionary<int, int>();
    public SerializableDictionary<int, int> carpetItemCounts = new SerializableDictionary<int, int>();
    public SerializableDictionary<int, int> wallItemCounts = new SerializableDictionary<int, int>();
    public SerializableDictionary<int, int> floorItemCounts = new SerializableDictionary<int, int>();

    private SaveManager saveManager;
    public void Initialize(SaveManager saveManager)
    {
        this.saveManager = saveManager;
        LoadInventoryData(saveManager.GetSaveData());
    }

    public void AddItem(int itemID, ObjectType type)
    {
        var itemCounts = GetItemCountDictionary(type);

        if (itemCounts.ContainsKey(itemID))
            itemCounts[itemID]++;
        else
            itemCounts[itemID] = 1;

        saveManager.SaveInventoryChange(type, itemID, itemCounts[itemID]);
    }

    public void RemoveItem(int itemID, ObjectType type)
    {
        var itemCounts = GetItemCountDictionary(type);

        if (itemCounts.ContainsKey(itemID) && itemCounts[itemID] > 0)
        {
            itemCounts[itemID]--;
            saveManager.SaveInventoryChange(type, itemID, itemCounts[itemID]);
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
        furnitureItemCounts = saveData.furnitureItemCounts;
        carpetItemCounts = saveData.carpetItemCounts;
        wallItemCounts = saveData.wallItemCounts;
        floorItemCounts = saveData.floorItemCounts;
    }

    private SerializableDictionary<int, int> GetItemCountDictionary(ObjectType type)
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

    internal void SetSaveManager(SaveManager saveManager)
    {
        this.saveManager = saveManager;
    }

}



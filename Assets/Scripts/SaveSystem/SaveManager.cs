using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using System;

public class SaveManager
{
    private SaveData saveData;
    public bool IsSaveEnabled = true;

    public void SetIsSaveEnabled(bool value)
    {
        IsSaveEnabled = value;
    }

    public SaveData StartSession()
    {
        saveData = SaveSystem.LoadGame();

        if (saveData == null)
        {
            return null;
        }
        else
        {
            return saveData;
        }
    }

    public SaveManager()
    {
    }

    public void SetSaveData(SaveData saveData)
    {
        this.saveData = saveData;
    }

    public SaveData GetSaveData()
    {
        return saveData;
    }

    public void UpdateInventory(Dictionary<int, int> furnitureItemCounts,
                                Dictionary<int, int> carpetItemCounts,
                                Dictionary<int, int> wallItemCounts,
                                Dictionary<int, int> floorItemCounts)
    {
        saveData.furnitureItemCounts.Clear();
        saveData.carpetItemCounts.Clear();
        saveData.wallItemCounts.Clear();
        saveData.floorItemCounts.Clear();

        foreach (var item in furnitureItemCounts)
        {
            saveData.furnitureItemCounts.Add(item.Key, item.Value);
        }

        foreach (var item in carpetItemCounts)
        {
            saveData.carpetItemCounts.Add(item.Key, item.Value);
        }

        foreach (var item in wallItemCounts)
        {
            saveData.wallItemCounts.Add(item.Key, item.Value);
        }

        foreach (var item in floorItemCounts)
        {
            saveData.floorItemCounts.Add(item.Key, item.Value);
        }

        SaveSystem.SaveGame(saveData);
    }

    public void UpdateMoney(int newAmount)
    {
        saveData.money = newAmount;
        SaveSystem.SaveGame(saveData);
    }

    public void UpdateScenario(int scenario)
    {
        saveData.currentScenario = scenario;
        SaveSystem.SaveGame(saveData);
    }

    public void SaveAll(GameData gameData, MoneyManager moneyManager, GridManager gridManager, PlacementManager placementManager)
    {
        saveData.money = moneyManager.currentMoney;
        saveData.furnitureItemCounts = gameData.furnitureItemCounts;
        saveData.carpetItemCounts = gameData.carpetItemCounts;
        saveData.wallItemCounts = gameData.wallItemCounts;
        saveData.floorItemCounts = gameData.floorItemCounts;
        saveData.currentScenario = gridManager.scenario;
        saveData.placedItems = placementManager.GetPlacedItems();

        SaveSystem.SaveGame(saveData);
    }

    public void SaveInventoryChange(ObjectType itemType, int itemId, int count)
    {
        SerializableDictionary<int, int> targetDict = null;

        switch (itemType)
        {
            case ObjectType.Furniture:
                targetDict = saveData.furnitureItemCounts;
                break;
            case ObjectType.Carpet:
                targetDict = saveData.carpetItemCounts;
                break;
            case ObjectType.Wall:
                targetDict = saveData.wallItemCounts;
                break;
            case ObjectType.Floor:
                targetDict = saveData.floorItemCounts;
                break;
        }

        if (targetDict != null)
        {
            if (count > 0)
            {
                targetDict[itemId] = count;
            }
            else
            {
                targetDict.Remove(itemId);
            }

            SaveSystem.SaveGame(saveData);
        }
    }

    public void SavePlacedItems(List<PlacedItemData> placedItems)
    {
        if (IsSaveEnabled == false)
        {
            return;
        }
        saveData.placedItems = placedItems;
        SaveSystem.SaveGame(saveData);
    }
}

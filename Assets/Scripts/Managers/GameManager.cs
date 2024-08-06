using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameData gameData;

    private SaveManager saveManager;
    public MoneyManager moneyManager;
    public InventoryManager inventoryManager;
    public ShopManager shopManager;
    public GridManager gridManager;
    public PlacementManager placementManager;

    public static int playerId;

    private static bool isEditMode;


    void Start()
    {            
        saveManager = new SaveManager();

        SaveData saveData = saveManager.StartSession();

        InitializeSaveManagers(saveManager);

        if (saveData == null)
        {
            InitializeNewGame();
        }
        else
        {
            LoadGame();
        }

    }

    void InitializeNewGame()
    {
        moneyManager.Initialize(10000);
        gridManager.InitializeDefaultScenario();


        SaveData saveData = new SaveData
        {
            money = 10000,
            furnitureItemCounts = new SerializableDictionary<int, int>(),
            carpetItemCounts = new SerializableDictionary<int, int>(),
            wallItemCounts = new SerializableDictionary<int, int>(),
            floorItemCounts = new SerializableDictionary<int, int>(),
            currentScenario = 1,
            placedItems = new List<PlacedItemData>()
        };

        saveManager.SetSaveData(saveData);
        SaveSystem.SaveGame(saveData);
    }

    void LoadGame()
    {
        gameData.Initialize();
        moneyManager.Initialize(saveManager.GetSaveData().money);
        inventoryManager.Initialize(saveManager.GetSaveData());
        gridManager.InitializeFromSaveData(saveManager.GetSaveData().currentScenario);
        placementManager.LoadPlacedItems();
    }

    public static void EnterEditMode()
    {
        isEditMode = true;
    }

    public static void ExitEditMode()
    {
        isEditMode = false;
    }

    public static bool IsEditMode()
    {
        return isEditMode;
    }

    private void InitializeSaveManagers(SaveManager saveManager)
    {
        moneyManager.SetSaveManager(saveManager);
        shopManager.SetSaveManager(saveManager);
        inventoryManager.SetSaveManager(saveManager);
        gameData.SetSaveManager(saveManager);
        gridManager.SetSaveManager(saveManager);
        placementManager.SetSaveManager(saveManager);
    }
}

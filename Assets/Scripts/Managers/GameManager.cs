using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameData gameData;

    private SaveManager saveManager;
    public MoneyManager moneyManager;
    public InventoryManager inventoryManager;
    public ShopManager shopManager;
    public GridManager gridManager;

    public static int playerId;

    void Start()
    {            
        saveManager = new SaveManager(gameData);

        SaveData saveData = saveManager.StartSession();

        moneyManager.SetSaveManager(saveManager);
        shopManager.SetSaveManager(saveManager);
        inventoryManager.SetSaveManager(saveManager);
        gameData.SetSaveManager(saveManager);
        gridManager.SetSaveManager(saveManager);

        if (saveData == null)
        {
            Debug.Log("No save data found, initializing new game");
            InitializeNewGame();
        }
        else
        {
            Debug.Log("Save data found, loading game");
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
            currentScenario = 1
        };

        saveManager.SetSaveData(saveData);
        SaveSystem.SaveGame(saveData);
    }

    void LoadGame()
    {
        moneyManager.Initialize(saveManager.GetSaveData().money);
        inventoryManager.Initialize(saveManager.GetSaveData());
        gridManager.InitializeFromSaveData(saveManager.GetSaveData().currentScenario);
    }
}

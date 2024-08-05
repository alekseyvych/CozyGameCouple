public class LoadManager
{
    private SaveData saveData;

    public LoadManager(GameData gameData)
    {
        saveData = SaveSystem.LoadGame();
        if (saveData != null)
        {
            gameData.LoadInventoryData(saveData);
        }
    }

    public bool HasSaveData()
    {
        return saveData != null;
    }

    public void LoadAll(MoneyManager moneyManager, InventoryManager inventoryManager, GridManager gridManager)
    {
        if (saveData == null) return;

        moneyManager.Initialize(saveData.money);
        inventoryManager.Initialize(saveData);
        //gridManager.InitializeFromSaveData(saveData);
    }
}

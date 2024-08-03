using System.Collections.Generic;

public class SaveDataManager
{
    private SaveData saveData;

    public SaveDataManager(GameData gameData)
    {
        saveData = SaveSystem.LoadGame();

        if (saveData == null)
        {
            saveData = new SaveData
            {
                money = 1000,
                inventoryItems = new Dictionary<int, int>(),
                // Initialize other default values as needed
            };

            SaveSystem.SaveGame(saveData); // Save this initial data
        }
        gameData.LoadInventoryData(saveData);
    }

    public void UpdateMoney(int newAmount)
    {
        saveData.money = newAmount;
        SaveSystem.SaveGame(saveData);
    }

    public void UpdateInventory(Dictionary<int, int> updatedItemCounts)
    {
        saveData.inventoryItems = updatedItemCounts;
        SaveSystem.SaveGame(saveData);
    }

    /*public void UpdateGridData(List<GridData> gridData)
    {
        saveData.gridData = gridData;
        SaveSystem.SaveGame(saveData);
    }*/

    public void UpdateCurrentScenario(int scenario)
    {
        saveData.currentScenario = scenario;
        SaveSystem.SaveGame(saveData);
    }

    public SaveData GetSaveData()
    {
        return saveData;
    }
}

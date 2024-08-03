using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameData gameData;
    private SaveDataManager saveDataManager;
    public MoneyManager moneyManager;
    public InventoryManager inventoryManager;

    void Start()
    {
        saveDataManager = new SaveDataManager(gameData);

        moneyManager.Initialize(saveDataManager);
    }
}

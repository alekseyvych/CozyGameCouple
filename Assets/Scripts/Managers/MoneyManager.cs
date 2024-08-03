using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public int currentMoney;
    public TextMeshProUGUI moneyText;
    private SaveDataManager saveDataManager;

    public void Initialize(SaveDataManager saveDataManager)
    {
        this.saveDataManager = saveDataManager;
        currentMoney = saveDataManager.GetSaveData().money;
        UpdateMoneyUI();
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateMoneyUI();
        saveDataManager.UpdateMoney(currentMoney);
    }

    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            UpdateMoneyUI();
            saveDataManager.UpdateMoney(currentMoney);
            return true;
        }
        else
        {
            Debug.Log("Not enough money");
            return false;
        }
    }

    void UpdateMoneyUI()
    {
        moneyText.text = "Money: $" + currentMoney.ToString();
    }
}

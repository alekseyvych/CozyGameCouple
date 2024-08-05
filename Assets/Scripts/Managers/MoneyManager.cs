using System;
using TMPro;
using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public int currentMoney;
    public TextMeshProUGUI moneyText;
    private SaveManager saveManager;

    public void Initialize(int money)
    {
        currentMoney = money;
        UpdateMoneyUI();
    }

    public int GetMoney()
    {
        return currentMoney;
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateMoneyUI();
        saveManager.UpdateMoney(currentMoney);
    }

    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            UpdateMoneyUI();
            saveManager.UpdateMoney(currentMoney);
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

    internal void SetSaveManager(SaveManager saveManager)
    {
        this.saveManager = saveManager;
    }
}

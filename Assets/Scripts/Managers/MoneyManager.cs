using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MoneyManager : MonoBehaviour
{
    public int currentMoney = 1000;
    public TextMeshProUGUI moneyText;

    void Start()
    {
        UpdateMoneyUI();
    }

    public void AddMoney(int amount)
    {
        currentMoney += amount;
        UpdateMoneyUI();
    }

    public bool SpendMoney(int amount)
    {
        if (currentMoney >= amount)
        {
            currentMoney -= amount;
            UpdateMoneyUI();
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

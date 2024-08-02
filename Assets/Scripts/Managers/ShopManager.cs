using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Transform furnitureShopPanel;
    public Transform carpetShopPanel;
    public Transform wallDecorationShopPanel;
    public Transform floorDecorationShopPanel;
    public GameObject shopItemPrefab;
    public InventoryManager inventoryManager;
    public PlacementController placementController;
    public MoneyManager moneyManager;

    public int playerMoney;

    private Dictionary<ObjectType, Transform> shopPanels;

    void Start()
    {
        playerMoney = moneyManager.currentMoney;

        shopPanels = new Dictionary<ObjectType, Transform>
        {
            { ObjectType.Furniture, furnitureShopPanel },
            { ObjectType.Carpet, carpetShopPanel },
            { ObjectType.WallDecoration, wallDecorationShopPanel },
            { ObjectType.FloorDecoration, floorDecorationShopPanel }
        };

        PopulateShop(ObjectType.Furniture, "Prefabs/Shop/Furniture");
        PopulateShop(ObjectType.Carpet, "Prefabs/Shop/Carpet");
        PopulateShop(ObjectType.WallDecoration, "Prefabs/Shop/Wall");
        PopulateShop(ObjectType.FloorDecoration, "Prefabs/Shop/Floor");
    }

    void PopulateShop(ObjectType type, string resourcePath)
    {
        GameObject[] items = Resources.LoadAll<GameObject>(resourcePath);

        foreach (var item in items)
        {
            GameObject shopItemButton = Instantiate(shopItemPrefab, shopPanels[type]);
            shopItemButton.GetComponentInChildren<Text>().text = item.name + " - $" + item.GetComponent<IPlaceableObject>().Price;
            shopItemButton.GetComponent<Button>().onClick.AddListener(() => OnShopItemClicked(item));
        }
    }

    void OnShopItemClicked(GameObject itemPrefab)
    {
        IPlaceableObject placeableObject = itemPrefab.GetComponent<IPlaceableObject>();

        if (playerMoney >= placeableObject.Price)
        {
            playerMoney -= placeableObject.Price;
            ShowPurchaseOptions(itemPrefab);
        }
        else
        {
            Debug.Log("Not enough money to purchase this item.");
        }
    }

    void ShowPurchaseOptions(GameObject itemPrefab)
    {
        // Placeholder for actual UI implementation
        bool moveToInventory = true; // This would be determined by the player's choice

        if (moveToInventory)
        {
            inventoryManager.AddToInventory(itemPrefab);
        }
        else
        {
            placementController.StartPlacingObject(itemPrefab);
        }
    }
}

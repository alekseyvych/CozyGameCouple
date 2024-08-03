using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public GameObject shopUI;
    public GameObject shopButtonPrefab;
    public Transform contentPanel;

    public Button furnitureTabButton;
    public Button carpetTabButton;
    public Button wallTabButton;
    public Button floorTabButton;
    public Button exitButton;
    public Button openShopButton;

    public GameObject storingPanel;
    public Button placeInInventoryButton;
    public Button placeInHouseButton;
    public TextMeshProUGUI storingPanelText;

    public PlacementController placementController;
    public InventoryManager inventoryManager;
    public MoneyManager moneyManager;

    public GameData gameData;

    private Dictionary<string, List<GameObject>> shopItems = new Dictionary<string, List<GameObject>>();
    private GameObject selectedItem;

    void Start()
    {
        furnitureTabButton.onClick.AddListener(() => DisplayCategoryItems("Furniture"));
        carpetTabButton.onClick.AddListener(() => DisplayCategoryItems("Carpet"));
        wallTabButton.onClick.AddListener(() => DisplayCategoryItems("Wall"));
        floorTabButton.onClick.AddListener(() => DisplayCategoryItems("Floor"));
        exitButton.onClick.AddListener(CloseShop);
        openShopButton.onClick.AddListener(OpenShop);

        placeInInventoryButton.onClick.AddListener(() => StoreItem(true));
        placeInHouseButton.onClick.AddListener(() => StoreItem(false));

        shopItems["Furniture"] = gameData.furnitureItems;
        shopItems["Carpet"] = gameData.carpetItems;
        shopItems["Wall"] = gameData.wallItems;
        shopItems["Floor"] =gameData.floorItems;

        DisplayCategoryItems("Furniture");
    }

    void DisplayCategoryItems(string category)
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        foreach (GameObject item in shopItems[category])
        {
            Debug.Log(item.name);
            GameObject newButton = Instantiate(shopButtonPrefab, contentPanel);

            Image itemImage = newButton.transform.GetChild(0).GetChild(0).GetComponent<Image>();
            TextMeshProUGUI nameText = newButton.transform.GetChild(1).GetComponent<TextMeshProUGUI>();
            TextMeshProUGUI priceText = newButton.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();

            var placeable = item.GetComponent<IPlaceableObject>();

            priceText.text = placeable.GetPrice().ToString();
            nameText.text = placeable.GetName();
            itemImage.sprite = placeable.GetPreviewSprite();

            Button buttonComponent = newButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => SelectItem(item));
        }
    }

    void SelectItem(GameObject item)
    {
        var placeable = item.GetComponent<IPlaceableObject>();

        if (moneyManager.SpendMoney(placeable.GetPrice()))
        {
            selectedItem = item;
            storingPanel.SetActive(true);
            storingPanelText.text = $"What would you like to do with {placeable.GetName()}?";
        }
        else
        {
            Debug.Log("Not enough money to buy this item.");
        }
    }

    void StoreItem(bool toInventory)
    {
        if (selectedItem == null) return;

        if (toInventory)
        {
            gameData.AddItem(selectedItem.GetInstanceID(), selectedItem.GetComponent<IPlaceableObject>().GetObjectType());
        }
        else
        {
            placementController.StartPlacingObject(selectedItem.gameObject);
            CloseShop();
        }

        storingPanel.SetActive(false);
        selectedItem = null;
    }

    public void CloseShop()
    {
        shopUI.SetActive(false);
    }

    public void OpenShop()
    {
        shopUI.SetActive(true);
        DisplayCategoryItems("Furniture");
    }
}

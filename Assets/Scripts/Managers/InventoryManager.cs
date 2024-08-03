using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryUI;
    public GameObject inventoryButtonPrefab;
    public Transform contentPanel;

    public Button furnitureTabButton;
    public Button carpetTabButton;
    public Button wallTabButton;
    public Button floorTabButton;
    public Button openInventoryButton;
    public Button exitButton;

    public GameData gameData;
    public PlacementController placementController;

    private GameObject selectedItem;

    private void Start()
    {
        furnitureTabButton.onClick.AddListener(() => DisplayCategoryItems(ObjectType.Furniture));
        carpetTabButton.onClick.AddListener(() => DisplayCategoryItems(ObjectType.Carpet));
        wallTabButton.onClick.AddListener(() => DisplayCategoryItems(ObjectType.Wall));
        floorTabButton.onClick.AddListener(() => DisplayCategoryItems(ObjectType.Floor));
        openInventoryButton.onClick.AddListener(OpenInventory);
        exitButton.onClick.AddListener(CloseInventory);

        DisplayCategoryItems(ObjectType.Furniture);
    }

    void DisplayCategoryItems(ObjectType category)
    {
        foreach (Transform child in contentPanel)
        {
            Destroy(child.gameObject);
        }

        List<GameObject> itemsToDisplay = gameData.GetCategoryItemsInInventory(category);

        foreach (GameObject item in itemsToDisplay)
        {
            GameObject newButton = Instantiate(inventoryButtonPrefab, contentPanel);

            TextMeshProUGUI ownedText = newButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            Image itemImage = newButton.transform.GetChild(1).GetChild(0).GetComponent<Image>();
            TextMeshProUGUI nameText = newButton.transform.GetChild(2).GetComponent<TextMeshProUGUI>();

            var placeable = item.GetComponent<IPlaceableObject>();

            int itemCount = gameData.GetItemCount(placeable.GetId(), category);
            ownedText.text = "Owned: " + itemCount.ToString();
            nameText.text = placeable.GetName();
            itemImage.sprite = placeable.GetPreviewSprite();

            Button buttonComponent = newButton.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => SelectItem(item, category));
        }
    }

    void SelectItem(GameObject item, ObjectType category)
    {
        var placeable = item.GetComponent<IPlaceableObject>();

        if (gameData.GetItemCount(placeable.GetId(), category) > 0)
        {
            selectedItem = item;
            placementController.StartPlacingObject(selectedItem);
            gameData.RemoveItem(placeable.GetId(), category);
            CloseInventory();
        }
        else
        {
            Debug.Log("No items of this type left in inventory.");
        }
    }

    public void CloseInventory()
    {
        selectedItem = null;
        inventoryUI.SetActive(false);
    }

    public void OpenInventory()
    {
        inventoryUI.SetActive(true);
        DisplayCategoryItems(ObjectType.Furniture);
    }
}

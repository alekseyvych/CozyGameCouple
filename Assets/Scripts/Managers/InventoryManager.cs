using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    private List<GameObject> inventoryItems = new List<GameObject>();

    public void AddToInventory(GameObject itemPrefab)
    {
        inventoryItems.Add(itemPrefab);
        Debug.Log(itemPrefab.name + " added to inventory.");
    }

    public void RemoveFromInventory(GameObject itemPrefab)
    {
        if (inventoryItems.Contains(itemPrefab))
        {
            inventoryItems.Remove(itemPrefab);
            Debug.Log(itemPrefab.name + " removed from inventory.");
        }
    }

    public void UseItemFromInventory(GameObject itemPrefab)
    {
        if (inventoryItems.Contains(itemPrefab))
        {
            // Start placing the object using the PlacementController
            PlacementController placementController = FindObjectOfType<PlacementController>();
            placementController.StartPlacingObject(itemPrefab);
            RemoveFromInventory(itemPrefab);
        }
        else
        {
            Debug.Log("Item not in inventory.");
        }
    }

    public void DisplayInventoryItems()
    {
        // Placeholder for UI implementation to display inventory items
        foreach (var item in inventoryItems)
        {
            Debug.Log("Inventory Item: " + item.name);
        }
    }
}

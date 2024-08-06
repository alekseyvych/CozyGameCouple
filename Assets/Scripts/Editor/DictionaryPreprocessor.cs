using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class DictionaryPreprocessor : MonoBehaviour
{
    [MenuItem("Tools/Build Item Dictionary")]
    public static void BuildItemDictionary()
    {
        // Load the GameData asset from the specified path
        GameData gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");
        if (gameData != null)
        {
            SerializableDictionary<int, ItemReference> itemDictionary = new SerializableDictionary<int, ItemReference>();

            // Add items from each category to the dictionary
            AddItemsToDictionary(gameData.furnitureItems, itemDictionary, ObjectType.Furniture);
            AddItemsToDictionary(gameData.carpetItems, itemDictionary, ObjectType.Carpet);
            AddItemsToDictionary(gameData.wallItems, itemDictionary, ObjectType.Wall);
            AddItemsToDictionary(gameData.floorItems, itemDictionary, ObjectType.Floor);

            // Convert the dictionary to JSON and save it to a file
            string json = JsonUtility.ToJson(new ItemDictionaryWrapper(itemDictionary));
            string path = Path.Combine(Application.persistentDataPath, "PrebuiltItemDictionary.json");
            File.WriteAllText(path, json);

            // Refresh the AssetDatabase to ensure changes are applied
            AssetDatabase.Refresh();
            Debug.Log("Item Dictionary built and saved to " + path);
        }
        else
        {
            Debug.LogError("GameData not found in Resources.");
        }
    }

    // Helper method to add items to the dictionary
    private static void AddItemsToDictionary(List<GameObject> items, SerializableDictionary<int, ItemReference> dict, ObjectType objectType)
    {
        for (int i = 0; i < items.Count; i++)
        {
            var placeableObject = items[i].GetComponent<IPlaceableObject>();
            if (placeableObject != null)
            {
                dict.Add(placeableObject.GetId(), new ItemReference(objectType, i));
            }
        }
    }
}
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DuplicateIDChecker : MonoBehaviour
{
    [MenuItem("Tools/Check for Duplicate IDs")]
    public static void CheckForDuplicateIDs()
    {
        GameData gameData = AssetDatabase.LoadAssetAtPath<GameData>("Assets/Data/GameData.asset");
        if (gameData != null)
        {
            Dictionary<int, List<string>> idMap = new Dictionary<int, List<string>>();
            CheckListForDuplicates(gameData.furnitureItems, ObjectType.Furniture, idMap);
            CheckListForDuplicates(gameData.carpetItems, ObjectType.Carpet, idMap);
            CheckListForDuplicates(gameData.wallItems, ObjectType.Wall, idMap);
            CheckListForDuplicates(gameData.floorItems, ObjectType.Floor, idMap);

            bool hasDuplicates = false;

            foreach (var entry in idMap)
            {
                if (entry.Value.Count > 1)
                {
                    Debug.LogError($"Duplicate ID {entry.Key} found in categories: {string.Join(", ", entry.Value)}");
                    hasDuplicates = true;
                }
            }

            if (!hasDuplicates)
            {
                Debug.Log("No duplicate IDs found.");
            }
        }
        else
        {
            Debug.LogError("GameData asset not found.");
        }
    }

    private static void CheckListForDuplicates(List<GameObject> items, ObjectType objectType, Dictionary<int, List<string>> idMap)
    {
        foreach (var item in items)
        {
            var placeableObject = item.GetComponent<IPlaceableObject>();
            if (placeableObject != null)
            {
                int id = placeableObject.GetId();
                if (!idMap.ContainsKey(id))
                {
                    idMap[id] = new List<string>();
                }

                idMap[id].Add(objectType.ToString());
            }
        }
    }
}

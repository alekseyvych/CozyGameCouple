using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public int money;
    public SerializableDictionary<int, int> furnitureItemCounts = new SerializableDictionary<int, int>();
    public SerializableDictionary<int, int> carpetItemCounts = new SerializableDictionary<int, int>();
    public SerializableDictionary<int, int> wallItemCounts = new SerializableDictionary<int, int>();
    public SerializableDictionary<int, int> floorItemCounts = new SerializableDictionary<int, int>();
    public int currentScenario;

    public override string ToString()
    {
        string result = $"Money: {money}\nCurrent Scenario: {currentScenario}\n";
        result += "Furniture Item Counts:\n";
        foreach (var item in furnitureItemCounts)
        {
            result += $"- ID: {item.Key}, Count: {item.Value}\n";
        }
        result += "Carpet Item Counts:\n";
        foreach (var item in carpetItemCounts)
        {
            result += $"- ID: {item.Key}, Count: {item.Value}\n";
        }
        result += "Wall Item Counts:\n";
        foreach (var item in wallItemCounts)
        {
            result += $"- ID: {item.Key}, Count: {item.Value}\n";
        }
        result += "Floor Item Counts:\n";
        foreach (var item in floorItemCounts)
        {
            result += $"- ID: {item.Key}, Count: {item.Value}\n";
        }
        return result;
    }
}

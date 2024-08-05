using System.IO;
using UnityEngine;

public static class SaveSystem
{
    private static string saveFilePath = Application.persistentDataPath + "/gameSaveData.json";

    public static void SaveGame(SaveData saveData)
    {
        string json = JsonUtility.ToJson(saveData);

        File.WriteAllText(saveFilePath, json);
    }

    public static SaveData LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            Debug.Log("Save file found");
            string json = File.ReadAllText(saveFilePath);
            return JsonUtility.FromJson<SaveData>(json);
        }
        else
        {
            Debug.Log("Save file not found");
            return null;
        }
    }
}

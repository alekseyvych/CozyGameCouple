using System.Collections.Generic;

[System.Serializable]
public class ItemData
{
    public int id;
    public string prefabPath;
}

[System.Serializable]
public class ItemDictionary
{
    public List<ItemData> items = new List<ItemData>();

    public Dictionary<int, string> ToDictionary()
    {
        Dictionary<int, string> dict = new Dictionary<int, string>();
        foreach (var item in items)
        {
            dict[item.id] = item.prefabPath;
        }
        return dict;
    }
}
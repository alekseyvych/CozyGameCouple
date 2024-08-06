using System;

[Serializable]
public class ItemDictionaryWrapper
{
    public SerializableDictionary<int, ItemReference> itemDictionary;

    public ItemDictionaryWrapper(SerializableDictionary<int, ItemReference> dict)
    {
        itemDictionary = dict;
    }
}
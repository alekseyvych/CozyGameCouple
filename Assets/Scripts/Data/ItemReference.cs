
using System;

[Serializable]
public class ItemReference
{
    public ObjectType objectType;
    public int index;

    public ItemReference(ObjectType objectType, int index)
    {
        this.objectType = objectType;
        this.index = index;
    }
}
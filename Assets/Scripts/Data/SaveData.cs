using System.Collections.Generic;

[System.Serializable]
public class SaveData
{
    public int money;
    public Dictionary<int, int> inventoryItems = new Dictionary<int, int>();
    //public List<GridData> gridData = new List<GridData>();
    public int currentScenario;
}
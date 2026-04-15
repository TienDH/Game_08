using UnityEngine;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    public string currentMapId = "Map1";

    private Dictionary<string, MapUpgradeData> mapData = new();

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public MapUpgradeData GetData()
    {
        if (!mapData.ContainsKey(currentMapId))
        {
            mapData[currentMapId] = new MapUpgradeData();
        }

        return mapData[currentMapId];
    }

    public int GetRareLevel()
    {
        return GetData().rareLevel;
    }

    public int GetValueLevel()
    {
        return GetData().valueLevel;
    }

    public void UpgradeRare()
    {
        var d = GetData();
        if (d.rareLevel >= 15) return;

        int cost = GetCost(d.rareLevel);
        if (MoneyManager.Instance.money < cost) return;

        MoneyManager.Instance.money -= cost;
        d.rareLevel++;
    }

    public void UpgradeValue()
    {
        var d = GetData();
        if (d.valueLevel >= 15) return;

        int cost = GetCost(d.valueLevel);
        if (MoneyManager.Instance.money < cost) return;

        MoneyManager.Instance.money -= cost;
        d.valueLevel++;
    }

    public int GetCost(int level)
    {
        return Mathf.RoundToInt(50 * Mathf.Pow(1.5f, level));
    }
}

[System.Serializable]
public class MapUpgradeData
{
    public int rareLevel;
    public int valueLevel;
}
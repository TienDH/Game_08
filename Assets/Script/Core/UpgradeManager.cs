using UnityEngine;
using System.Collections.Generic;

public class UpgradeManager : MonoBehaviour
{
    public static UpgradeManager Instance;

    public string currentMapId = "Map1";
    public MapUpgradeConfig[] configs;

    private Dictionary<string, MapUpgradeData> mapData = new();

    public System.Action onUpgradeChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public MapUpgradeData GetData()
    {
        if (!mapData.ContainsKey(currentMapId))
            mapData[currentMapId] = new MapUpgradeData();

        return mapData[currentMapId];
    }

    public MapUpgradeConfig GetConfig()
    {
        foreach (var c in configs)
        {
            if (c.mapId == currentMapId)
                return c;
        }

        return null;
    }

    public int GetRareLevel() => GetData().rareLevel;
    public int GetValueLevel() => GetData().valueLevel;

    public void UpgradeRare()
    {
        var d = GetData();
        if (d.rareLevel >= 15) return;

        int cost = GetCost(d.rareLevel);
        if (MoneyManager.Instance.money < cost) return;

        MoneyManager.Instance.SpendMoney(cost);
        d.rareLevel++;

        onUpgradeChanged?.Invoke();
    }

    public void UpgradeValue()
    {
        var d = GetData();
        if (d.valueLevel >= 15) return;

        int cost = GetCost(d.valueLevel);
        if (MoneyManager.Instance.money < cost) return;

        MoneyManager.Instance.SpendMoney(cost);
        d.valueLevel++;

        onUpgradeChanged?.Invoke();
    }

    public int GetCost(int level)
    {
        var config = GetConfig();
        if (config == null) return 0;

        return Mathf.RoundToInt(
            config.baseCost * Mathf.Pow(config.costMultiplier, level)
        );
    }
}

[System.Serializable]
public class MapUpgradeData
{
    public int rareLevel;
    public int valueLevel;
}
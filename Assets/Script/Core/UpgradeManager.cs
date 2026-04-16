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

    public MapUpgradeConfig GetConfig()
    {
        foreach (var c in configs)
        {
            if (c.mapId == currentMapId)
                return c;
        }

        Debug.LogError("❌ Không tìm thấy config map: " + currentMapId);
        return null;
    }

    public MapUpgradeData GetData()
    {
        if (!mapData.ContainsKey(currentMapId))
            mapData[currentMapId] = new MapUpgradeData();

        return mapData[currentMapId];
    }

    // ===== GET =====

    public int GetRareLevel() => GetData().rareLevel;
    public int GetValueLevel() => GetData().valueLevel;
    public int GetSpawnLevel() => GetData().spawnLevel;

    // ===== COST =====

    public int GetCost(int level)
    {
        var config = GetConfig();
        if (config == null) return 999999;

        return Mathf.RoundToInt(
            config.baseCost * Mathf.Pow(config.costMultiplier, level)
        );
    }

    // ===== UPGRADE =====

    public void UpgradeRare()
    {
        var d = GetData();
        var c = GetConfig();

        if (d.rareLevel >= c.rareMaxLevel) return;

        int cost = GetCost(d.rareLevel);
        if (MoneyManager.Instance.money < cost) return;

        MoneyManager.Instance.SpendMoney(cost);
        d.rareLevel++;

        onUpgradeChanged?.Invoke();
    }

    public void UpgradeValue()
    {
        var d = GetData();
        var c = GetConfig();

        if (d.valueLevel >= c.valueMaxLevel) return;

        int cost = GetCost(d.valueLevel);
        if (MoneyManager.Instance.money < cost) return;

        MoneyManager.Instance.SpendMoney(cost);
        d.valueLevel++;

        onUpgradeChanged?.Invoke();
    }

    public void UpgradeSpawn()
    {
        var d = GetData();
        var c = GetConfig();

        if (d.spawnLevel >= c.spawnMaxLevel) return;

        int cost = GetCost(d.spawnLevel);
        if (MoneyManager.Instance.money < cost) return;

        MoneyManager.Instance.SpendMoney(cost);
        d.spawnLevel++;

        onUpgradeChanged?.Invoke();
    }
    [System.Serializable]
    public class MapUpgradeData
    {
        public int rareLevel;
        public int valueLevel;
        public int spawnLevel;
    }
}
using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    public string mapId = "Map1";

    [Header("Prefab")]
    public GameObject itemPrefab;
    public ItemData[] allItemDatas;

    [Header("Spawn")]
    public float spawnRate = 1.2f;
    public int baseMaxItems = 40; // 🔥 đổi tên cho rõ

    [Header("Area")]
    public Transform[] spawnAreas;
    public Vector2 areaSize = new Vector2(6f, 4f);

    [Header("Visual")]
    public Vector2 scaleRange = new Vector2(0.85f, 1.25f);
    public float minRotation = -25f;
    public float maxRotation = 25f;

    private int currentCount = 0;

    private void Start()
    {
        if (itemPrefab == null || allItemDatas.Length == 0)
        {
            Debug.LogError("Spawner thiếu dữ liệu!");
            return;
        }

        InvokeRepeating(nameof(SpawnItem), 1f, spawnRate);
    }

    private void SpawnItem()
    {
        if (spawnAreas.Length == 0) return;

        // 🔥 check max item theo upgrade
        if (currentCount >= GetMaxItems())
            return;

        Transform area = spawnAreas[Random.Range(0, spawnAreas.Length)];

        Vector3 pos = area.position + new Vector3(
            Random.Range(-areaSize.x, areaSize.x),
            Random.Range(-areaSize.y, areaSize.y),
            0
        );

        ItemData data = GetSpawnItem();

        GameObject obj = Instantiate(itemPrefab, pos, Quaternion.identity);

        // GÁN DATA
        var drag = obj.GetComponent<DragItem>();
        if (drag != null)
            drag.data = data;

        var life = obj.GetComponent<ItemLife>();
        if (life != null)
            life.data = data;

        // sprite
        var sr = obj.GetComponent<SpriteRenderer>();
        if (sr != null && data.icon != null)
            sr.sprite = data.icon;

        // transform
        float scale = Random.Range(scaleRange.x, scaleRange.y);
        obj.transform.localScale = Vector3.one * scale;

        obj.transform.rotation = Quaternion.Euler(
            0, 0, Random.Range(minRotation, maxRotation)
        );

        currentCount++;

        if (life != null)
            life.onDespawn += () => currentCount--;
    }

    // =========================
    // 🔥 MAX ITEM (SPAWN UPGRADE)
    // =========================

    int GetMaxItems()
    {
        if (UpgradeManager.Instance == null)
            return baseMaxItems;

        var config = UpgradeManager.Instance.GetConfig();
        int level = UpgradeManager.Instance.GetSpawnLevel();

        if (config == null)
            return baseMaxItems;

        return baseMaxItems + level * config.spawnIncreasePerLevel;
    }

    // =========================
    // 🔥 RARITY SYSTEM
    // =========================

    ItemData GetSpawnItem()
    {
        if (UpgradeManager.Instance == null)
            return GetRandomItem();

        int rareLevel = UpgradeManager.Instance.GetRareLevel();
        var config = UpgradeManager.Instance.GetConfig();

        float bonus = 1 + rareLevel * (
            config != null ? config.rareBonusPerLevel : 0.25f
        );

        List<ItemData> pool = new();

        foreach (var item in allItemDatas)
        {
            int weight = GetBaseWeight(item.rarity);

            // buff item hiếm
            if (item.rarity >= Rarity.Rare)
                weight = Mathf.RoundToInt(weight * bonus);

            for (int i = 0; i < weight; i++)
                pool.Add(item);
        }

        if (pool.Count == 0)
            return GetRandomItem();

        return pool[Random.Range(0, pool.Count)];
    }

    int GetBaseWeight(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common: return 50;
            case Rarity.Uncommon: return 30;
            case Rarity.Rare: return 15;
            case Rarity.Epic: return 4;
            case Rarity.Legendary: return 1;
        }

        return 1;
    }

    ItemData GetRandomItem()
    {
        return allItemDatas[Random.Range(0, allItemDatas.Length)];
    }
}
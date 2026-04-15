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
    public int maxItems = 40;

    [Header("Area")]
    public Transform[] spawnAreas;
    public Vector2 areaSize = new Vector2(6f, 4f);

    [Header("Visual")]
    public Vector2 scaleRange = new Vector2(0.85f, 1.25f);
    public float minRotation = -25f;
    public float maxRotation = 25f;

    [Header("Rarity")]
    public List<RarityWeight> baseWeights;

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
        if (currentCount >= maxItems || spawnAreas.Length == 0) return;

        Transform area = spawnAreas[Random.Range(0, spawnAreas.Length)];

        Vector3 pos = area.position + new Vector3(
            Random.Range(-areaSize.x, areaSize.x),
            Random.Range(-areaSize.y, areaSize.y),
            0
        );

        ItemData data = GetSpawnItem();

        GameObject obj = Instantiate(itemPrefab, pos, Quaternion.identity);

        // 🔥 GÁN DATA
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
        obj.transform.rotation = Quaternion.Euler(0, 0, Random.Range(minRotation, maxRotation));

        currentCount++;

        if (life != null)
            life.onDespawn += () => currentCount--;
    }

    // ===== RARITY SYSTEM =====

    ItemData GetSpawnItem()
    {
        var weights = GetFinalWeights();
        var rarity = RollRarity(weights);
        return GetItemByRarity(rarity);
    }

    List<RarityWeight> GetFinalWeights()
    {
        int level = UpgradeManager.Instance.GetRareLevel();

        List<RarityWeight> result = new();

        foreach (var w in baseWeights)
        {
            float value = w.weight;

            if (w.rarity == Rarity.Rare ||
                w.rarity == Rarity.Epic ||
                w.rarity == Rarity.Legendary)
            {
                value *= (1 + level * 0.25f);
            }

            result.Add(new RarityWeight
            {
                rarity = w.rarity,
                weight = value
            });
        }

        return result;
    }

    Rarity RollRarity(List<RarityWeight> weights)
    {
        float total = 0;
        foreach (var w in weights)
            total += w.weight;

        float rand = Random.value * total;

        float sum = 0;
        foreach (var w in weights)
        {
            sum += w.weight;
            if (rand <= sum)
                return w.rarity;
        }

        return Rarity.Common;
    }

    ItemData GetItemByRarity(Rarity rarity)
    {
        List<ItemData> list = new();

        foreach (var item in allItemDatas)
        {
            if (item.rarity == rarity)
                list.Add(item);
        }

        if (list.Count == 0)
            return allItemDatas[Random.Range(0, allItemDatas.Length)];

        return list[Random.Range(0, list.Count)];
    }
}
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
        obj.transform.rotation = Quaternion.Euler(0, 0, Random.Range(minRotation, maxRotation));

        currentCount++;

        if (life != null)
            life.onDespawn += () => currentCount--;
    }

    // ===== RARITY SYSTEM FINAL =====

    ItemData GetSpawnItem()
    {
        int rareLevel = UpgradeManager.Instance.GetRareLevel();
        var config = UpgradeManager.Instance.GetConfig();
        float bonus = 1 + rareLevel * (config != null ? config.rareBonusPerLevel : 0.25f);

        List<ItemData> pool = new();

        foreach (var item in allItemDatas)
        {
            int weight = 1;

            switch (item.rarity)
            {
                case Rarity.Common: weight = 50; break;
                case Rarity.Uncommon: weight = 30; break;
                case Rarity.Rare: weight = 15; break;
                case Rarity.Epic: weight = 4; break;
                case Rarity.Legendary: weight = 1; break;
            }

            if (item.rarity >= Rarity.Rare)
                weight = Mathf.RoundToInt(weight * bonus);

            for (int i = 0; i < weight; i++)
                pool.Add(item);
        }

        return pool[Random.Range(0, pool.Count)];
    }
}
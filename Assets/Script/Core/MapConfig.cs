using UnityEngine;

[CreateAssetMenu(menuName = "Game/MapUpgradeConfig")]
public class MapUpgradeConfig : ScriptableObject
{
    public string mapId;

    [Header("Rare (Max 10)")]
    public int rareMaxLevel = 10;
    public float rareBonusPerLevel = 0.25f;

    [Header("Value (Max 10)")]
    public int valueMaxLevel = 10;
    public float valueBonusPerLevel = 0.2f;

    [Header("Max Spawn (Max 5)")]
    public int spawnMaxLevel = 5;
    public int spawnIncreasePerLevel = 5;

    [Header("Cost")]
    public int baseCost = 50;
    public float costMultiplier = 1.5f;
}
using UnityEngine;

[CreateAssetMenu(menuName = "Game/MapUpgradeConfig")]
public class MapUpgradeConfig : ScriptableObject
{
    public string mapId;

    [Header("Cost")]
    public float baseCost = 50;
    public float costMultiplier = 1.5f;

    [Header("Rare Upgrade")]
    public float rareBonusPerLevel = 0.25f;

    [Header("Value Upgrade")]
    public float valueBonusPerLevel = 0.2f;
}
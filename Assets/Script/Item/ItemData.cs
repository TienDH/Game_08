using UnityEngine;

[CreateAssetMenu(menuName = "Game/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName = "Trash";
    public Sprite icon;
    public int baseValue = 10;

    public ItemType type;
    public float riskChance = 0f;

    public float lifeTime = 15f;
    public float blinkTime = 4f;
    public Rarity rarity;
    [TextArea]
    public string description;
}

public enum ItemType
{
    Kimloai,
    nhua,
    linhkien,
    Giay,
    Khac
}
public enum Rarity
{
    Common,     // 1 sao
    Uncommon,   // 2
    Rare,       // 3
    Epic,       // 4
    Legendary   // 5
}
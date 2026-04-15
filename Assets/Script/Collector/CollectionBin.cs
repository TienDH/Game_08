using UnityEngine;

public class CollectionBin : MonoBehaviour
{
    public ItemType acceptType;

    public bool TryCollect(ItemData data)
    {
        if (data == null)
        {
            Debug.Log("❌ data NULL");
            return false;
        }

        if (data.type != acceptType)
        {
            Debug.Log("❌ Sai loại");
            return false;
        }

        int level = UpgradeManager.Instance.GetValueLevel();

        float multiplier = 1f + level * 0.25f;

        int finalValue = Mathf.RoundToInt(data.baseValue * multiplier);

        MoneyManager.Instance.AddMoney(finalValue);

        Debug.Log($"💰 +{finalValue}");

        return true;
    }
}
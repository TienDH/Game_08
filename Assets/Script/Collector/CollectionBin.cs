using UnityEngine;

public class CollectionBin : MonoBehaviour
{
    public ItemType acceptType;

    public bool TryCollect(ItemData data)
    {
        if (data == null) return false;

        if (data.type != acceptType)
            return false;

        if (UpgradeManager.Instance == null)
            return false;

        int valueLevel = UpgradeManager.Instance.GetValueLevel();
        var config = UpgradeManager.Instance.GetConfig();

        // 🔥 FIX NULL
        float bonus = (config != null) ? config.valueBonusPerLevel : 0.2f;

        float multiplier = 1 + valueLevel * bonus;

        int finalMoney = Mathf.RoundToInt(data.baseValue * multiplier);

        if (MoneyManager.Instance != null)
            MoneyManager.Instance.AddMoney(finalMoney);

        return true;
    }
}
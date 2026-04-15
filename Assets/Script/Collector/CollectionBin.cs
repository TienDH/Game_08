using UnityEngine;

public class CollectionBin : MonoBehaviour
{
    public ItemType acceptType;

    public bool TryCollect(ItemData data)
    {
        if (data == null) return false;

        if (data.type != acceptType)
            return false;

        int valueLevel = UpgradeManager.Instance.GetValueLevel();
        var config = UpgradeManager.Instance.GetConfig();

        float multiplier = 1 + valueLevel * (config != null ? config.valueBonusPerLevel : 0.2f);

        int finalMoney = Mathf.RoundToInt(data.baseValue * multiplier);

        MoneyManager.Instance.AddMoney(finalMoney);

        return true;
    }
}
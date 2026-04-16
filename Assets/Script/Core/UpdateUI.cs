using UnityEngine;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    public TextMeshProUGUI rareText;
    public TextMeshProUGUI valueText;
    public TextMeshProUGUI spawnText;

    public TextMeshProUGUI rareCost;
    public TextMeshProUGUI valueCost;
    public TextMeshProUGUI spawnCost;

    private void Start()
    {
        Refresh();

        UpgradeManager.Instance.onUpgradeChanged += Refresh;
        MoneyManager.Instance.onMoneyChanged += Refresh;
    }

    public void UpgradeRare() => UpgradeManager.Instance.UpgradeRare();
    public void UpgradeValue() => UpgradeManager.Instance.UpgradeValue();
    public void UpgradeSpawn() => UpgradeManager.Instance.UpgradeSpawn();

    void Refresh()
    {
        var u = UpgradeManager.Instance;
        var c = u.GetConfig();

        int r = u.GetRareLevel();
        int v = u.GetValueLevel();
        int s = u.GetSpawnLevel();

        rareText.text = $"Tỉ lệ Lv {r}/{c.rareMaxLevel}";
        valueText.text = $"Giá trị Lv {v}/{c.valueMaxLevel}";
        spawnText.text = $"Spawn Lv {s}/{c.spawnMaxLevel}";

        rareCost.text = GetCostText(r, c.rareMaxLevel);
        valueCost.text = GetCostText(v, c.valueMaxLevel);
        spawnCost.text = GetCostText(s, c.spawnMaxLevel);
    }

    string GetCostText(int level, int max)
    {
        if (level >= max) return "MAX";

        int cost = UpgradeManager.Instance.GetCost(level);
        return cost + "$";
    }
}
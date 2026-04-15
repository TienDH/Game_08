using UnityEngine;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    public TextMeshProUGUI rareText;
    public TextMeshProUGUI valueText;

    public TextMeshProUGUI rareCostText;
    public TextMeshProUGUI valueCostText;

    private void Start()
    {
        Refresh();

        UpgradeManager.Instance.onUpgradeChanged += Refresh;
        MoneyManager.Instance.onMoneyChanged += Refresh;
    }

    public void UpgradeRare()
    {
        UpgradeManager.Instance.UpgradeRare();
    }

    public void UpgradeValue()
    {
        UpgradeManager.Instance.UpgradeValue();
    }

    void Refresh()
    {
        int r = UpgradeManager.Instance.GetRareLevel();
        int v = UpgradeManager.Instance.GetValueLevel();

        rareText.text = $"Tỉ lệ Lv {r}";
        valueText.text = $"Giá trị Lv {v}";

        rareCostText.text = GetCostText(r);
        valueCostText.text = GetCostText(v);
    }

    string GetCostText(int level)
    {
        if (level >= 15) return "MAX";

        int cost = UpgradeManager.Instance.GetCost(level);
        return cost + "$";
    }
}
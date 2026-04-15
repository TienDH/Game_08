using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    public TextMeshProUGUI moneyText;

    private void Start()
    {
        UpdateUI();
        MoneyManager.Instance.onMoneyChanged += UpdateUI;
    }

    void UpdateUI()
    {
        moneyText.text = MoneyManager.Instance.money.ToString() + "$";
    }
}
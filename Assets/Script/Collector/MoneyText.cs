using TMPro;
using UnityEngine;

public class MoneyUI : MonoBehaviour
{
    public static MoneyUI Instance;

    public TextMeshProUGUI moneyText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (MoneyManager.Instance == null) return;

        int money = MoneyManager.Instance.money;
        moneyText.text = "$" + money.ToString();
    }
}
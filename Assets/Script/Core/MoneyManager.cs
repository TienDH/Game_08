using UnityEngine;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    public int money = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void AddMoney(int amount)
    {
        money += amount;
        Debug.Log("💰 Money: " + money);

        // 🔥 update UI
        if (MoneyUI.Instance != null)
            MoneyUI.Instance.Refresh();
    }
}
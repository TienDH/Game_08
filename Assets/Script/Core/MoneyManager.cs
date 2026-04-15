using UnityEngine;
using System;

public class MoneyManager : MonoBehaviour
{
    public static MoneyManager Instance;

    public int money = 0;

    public Action onMoneyChanged;

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
        onMoneyChanged?.Invoke();
    }

    public void SpendMoney(int amount)
    {
        money -= amount;
        onMoneyChanged?.Invoke();
    }
}
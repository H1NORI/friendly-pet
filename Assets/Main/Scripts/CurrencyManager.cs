using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    public int coins = 0;

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadCurrency();
    }

    public bool CanAfford(int amount) => coins >= amount;

    public void Spend(int amount)
    {
        coins -= amount;
        SaveCurrency();
        UIController.Instance.UpdateCoinsText();
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        SaveCurrency();
        UIController.Instance.UpdateCoinsText();
    }

    private void SaveCurrency()
    {
        PlayerPrefs.SetInt("Coins", coins);
    }

    private void LoadCurrency()
    {
        coins = PlayerPrefs.GetInt("Coins", 0);
    }
}



using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void BuyFood(FoodItem item)
    {
        if (CurrencyManager.Instance.CanAfford(item.price) && !InventoryManager.Instance.IsFoodOwned(item))
        {
            CurrencyManager.Instance.Spend(item.price);
            InventoryManager.Instance.AddFood(item);
            UIController.Instance.ReloadFoodInventoryUI();
            UIController.Instance.ReloadShopFoodUI();
        }
    }

    public void BuyActivity(ActivityItem item)
    {
        if (CurrencyManager.Instance.CanAfford(item.price) && !InventoryManager.Instance.IsActivityOwned(item))
        {
            CurrencyManager.Instance.Spend(item.price);
            InventoryManager.Instance.AddActivity(item);
            UIController.Instance.ReloadActivityInventoryUI();
            UIController.Instance.ReloadShopActivityUI();
        }
    }
}


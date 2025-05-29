using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    public List<FoodItem> ownedFood = new();
    public List<ActivityItem> ownedActivities = new();

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadInventory();
    }

    public void AddFood(FoodItem item)
    {
        if (IsFoodOwned(item))
        {
            return;
        }
        ownedFood.Add(item);
        SaveInventory();
    }

    public void AddActivity(ActivityItem item)
    {
        if (IsActivityOwned(item))
        {
            return;
        }
        ownedActivities.Add(item);
        SaveInventory();
    }

    public bool IsFoodOwned(FoodItem item)
    {
        return ownedFood.Find(f => f.id == item.id) != null;
    }

    public bool IsActivityOwned(ActivityItem item)
    {
        return ownedActivities.Find(a => a.id == item.id) != null;
    }

    private void SaveInventory()
    {
        PlayerPrefs.SetString("FoodInventory", JsonUtility.ToJson(new FoodInventoryWrapper { items = ownedFood }));
        PlayerPrefs.SetString("ActivityInventory", JsonUtility.ToJson(new ActivityInventoryWrapper { items = ownedActivities }));
    }

    private void LoadInventory()
    {
        string foodData = PlayerPrefs.GetString("FoodInventory", "");
        string activityData = PlayerPrefs.GetString("ActivityInventory", "");

        if (!string.IsNullOrEmpty(foodData))
            ownedFood = JsonUtility.FromJson<FoodInventoryWrapper>(foodData).items;

        if (!string.IsNullOrEmpty(activityData))
            ownedActivities = JsonUtility.FromJson<ActivityInventoryWrapper>(activityData).items;
    }

    [System.Serializable]
    private class FoodInventoryWrapper { public List<FoodItem> items = new(); }

    [System.Serializable]
    private class ActivityInventoryWrapper { public List<ActivityItem> items = new(); }
}



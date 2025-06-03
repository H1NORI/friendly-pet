using UnityEngine;

public enum ActivityType
{
    PettingActivity,
    BrushActivity,
    BallActivity,
    None
}

[System.Serializable]
public class ActivityItem
{
    public int id;
    public string name;
    public string descripion;
    public int price;
    public int minCoins;
    public int maxCoins;
    public Sprite icon;
    public ActivityType activity;
}

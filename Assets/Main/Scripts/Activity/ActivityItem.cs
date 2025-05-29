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
    public Sprite icon;
    public ActivityType activity;
}

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
    public string name;
    public Sprite icon;
    public ActivityType activity;
}

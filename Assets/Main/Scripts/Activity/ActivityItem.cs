using UnityEngine;

[System.Serializable]
public class ActivityItem
{
    public string name;
    public Sprite icon;
    public enum Activity
    {
        PettingActivity,
        BrushActivity,
        None
    };

    public Activity activity;
}
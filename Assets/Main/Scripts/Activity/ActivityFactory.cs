using System;
using System.Collections.Generic;

public static class ActivityFactory
{
    private static Dictionary<ActivityType, Func<IPetActivity>> activityMap = new()
    {
        { ActivityType.PettingActivity, () => new PettingActivity() },
        { ActivityType.BrushActivity, () => new BrushActivity() },
        { ActivityType.None, () => null },
    };

    public static IPetActivity GetActivity(ActivityType type)
    {
        if (activityMap.TryGetValue(type, out var constructor))
        {
            return constructor();
        }

        return null;
    }
}

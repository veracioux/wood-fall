using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAnalytics : MonoBehaviour
{
    public static void disableAnalytics()
    {
        UnityEngine.Analytics.Analytics.enabled = false;
        UnityEngine.Analytics.Analytics.deviceStatsEnabled = false;
        UnityEngine.Analytics.Analytics.initializeOnStartup = false;
        UnityEngine.Analytics.Analytics.limitUserTracking = false;
        UnityEngine.Analytics.PerformanceReporting.enabled = false;
    }
}

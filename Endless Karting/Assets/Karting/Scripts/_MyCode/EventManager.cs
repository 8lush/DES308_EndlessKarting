using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Abertay.Analytics;
using GameAnalyticsSDK;

public class EventManager : MonoBehaviour
{
    public delegate void Game();
    public static event Game StartTrack;
    public static event Game EndTrack;
    public static event Game SpawnNextTrack;
    public static event Game NextThreshold;

    private void Start()
    {
        GameAnalyticsSDK.GameAnalytics.Initialize();
    }

    public static void EventStartTrack()
    {
        StartTrack?.Invoke();
        AnalyticsManager.GetGAInstance.SendProgressionEvent(GameAnalyticsSDK.GAProgressionStatus.Start, "Track started");
    }

    public static void EventEndTrack()
    {
        EndTrack?.Invoke();
    }

    public static void EventSpawnNextTrack()
    {
        SpawnNextTrack?.Invoke();
    }

    public static void EventNextThreshold()
    {
        NextThreshold?.Invoke();
    }

    public static void TrackComplete(int score)
    {
        AnalyticsManager.GetGAInstance.SendProgressionEvent(GameAnalyticsSDK.GAProgressionStatus.Complete, "Track completed", score);
        SceneManager.LoadScene("LoseScene");
    }
}

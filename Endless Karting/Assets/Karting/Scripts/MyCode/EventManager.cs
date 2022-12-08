using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Abertay.Analytics;

public class EventManager : MonoBehaviour
{
    public delegate void Game();
    public static event Game StartTrack;
    public static event Game EndTrack;
    public static event Game SpawnNextTrack;
    public static event Game NextThreshold;

    public static void EventStartTrack()
    {
        StartTrack?.Invoke();
        AnalyticsManager.GetGAInstance.SendProgressionEvent(GameAnalyticsSDK.GAProgressionStatus.Start, "Test Track Start");
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

    public static void TrackComplete(float score)
    {
        AnalyticsManager.GetGAInstance.SendProgressionEvent(GameAnalyticsSDK.GAProgressionStatus.Complete, "Test Track Complete", "Score: " + score);
        SceneManager.LoadScene("LoseScene");
    }
}

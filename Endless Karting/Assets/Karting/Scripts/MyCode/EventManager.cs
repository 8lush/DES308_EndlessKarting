using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void Game();
    public static event Game StartTrack;
    public static event Game SpawnNextTrack;

    public static void EventStartTrack()
    {
        StartTrack?.Invoke();
    }

    public static void EventSpawnNextTrack()
    {
        SpawnNextTrack?.Invoke();
    }
}

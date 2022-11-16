using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EventManager : MonoBehaviour
{
    public delegate void Game();
    public static event Game StartTrack;
    public static event Game EndTrack;
    public static event Game SpawnNextTrack;

    public static void EventStartTrack()
    {
        StartTrack?.Invoke();
    }

    public static void EventEndTrack()
    {
        EndTrack?.Invoke();
        SceneManager.LoadScene("LoseScene");
    }

    public static void EventSpawnNextTrack()
    {
        SpawnNextTrack?.Invoke();
    }
}

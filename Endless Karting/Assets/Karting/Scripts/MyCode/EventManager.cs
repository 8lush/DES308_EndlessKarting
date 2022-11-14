using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void Game();
    public static event Game StartTrack;
    public static event Game TestTrackEvent;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void ActivateStartTrack()
    {
        StartTrack?.Invoke();
    }

    public static void TestTrack()
    {
        TestTrackEvent?.Invoke();
    }
}

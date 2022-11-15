using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessGameMode : MonoBehaviour
{
    [Header("Track Prefabs")]
    public GameObject straightTrack;
    public GameObject medTurnTrack;
    public GameObject uTurnTrack;
    public GameObject updownTrack;

    [Header("Track Start")]
    public GameObject startingIsland;
    private Vector3 trackSpawnLocation = new Vector3(0, 0, 0);
    private Vector3 trackPositionOffset = new Vector3(0, 0, 0);
    private Vector3 trackRotationOffset = new Vector3(0, 0, 0);

    [Header("Track Spawning Timings")]
    public float trackSpawnDelay = 0.5f;
    public float trackDestroyDelay = 5f;
    public float trackInstantiationFrequency = 2f;

    [Header("Track min/max")]
    public int minNumberofTrackForward = 2;
    public int maxNumberofTrackForward = 5;

    private int trackIntDirection = 0;

    private int numofTrackForward = 0;
    private bool special = false;


    void OnEnable()
    {
        EventManager.StartTrack += InitialTrack;
        EventManager.SpawnNextTrack += NextTrack;
    }

    void OnDisable()
    {
        EventManager.StartTrack -= InitialTrack;
        EventManager.SpawnNextTrack -= NextTrack;
    }

    private void Start()
    {
        trackSpawnLocation = startingIsland.transform.position + new Vector3(0, 0, 5);
    }

    void InitialTrack()
    {
        TrackForward();
        TrackForward();
        TrackForward();
    }

    void NextTrack()
    {
        switch((numofTrackForward, special))
        {
            case (0, false):
                TrackTurn();
                special = !special;
                numofTrackForward = Random.Range(minNumberofTrackForward, maxNumberofTrackForward + 1);
                break;
            case (0, true):
                TrackSpecial();
                special = !special;
                numofTrackForward = Random.Range(minNumberofTrackForward, maxNumberofTrackForward + 1);
                break;
            default:
                TrackForward();
                numofTrackForward--;
                break;
        }
    }

    // Spawns a straight track
    private void TrackForward()
    {
        GameObject trackForward = Instantiate(straightTrack, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset));

        Destroy(trackForward, trackDestroyDelay);

        AddTrackForwardOffset();
    }

    // Spawns a left or right turn track
    private void TrackTurn()
    {
        GameObject trackTurn;

        switch (Random.Range(0, 2))
        {
            case 0:
                AddTrackTurnOffset();
                trackTurn = Instantiate(medTurnTrack, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset + new Vector3(0, -90, 0)));
                trackRotationOffset = trackRotationOffset + new Vector3(0, 90, 0);
                trackIntDirection++;
                NormalizeTrackDirection();
                break;
            case 1:
                trackTurn = Instantiate(medTurnTrack, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset));
                trackRotationOffset = trackRotationOffset + new Vector3(0, -90, 0);
                trackIntDirection--;
                NormalizeTrackDirection();
                AddTrackTurnOffset();
                break;
            default:
                trackTurn = Instantiate(medTurnTrack, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset));
                trackRotationOffset = trackRotationOffset + new Vector3(0, 90, 0);
                trackIntDirection++;
                NormalizeTrackDirection();
                break;
        }        

        Destroy(trackTurn, trackDestroyDelay);
        
    }

    // Spawns one of the special tracks
    private void TrackSpecial()
    {
        GameObject trackSpecial;

        switch (Random.Range(0, 4))
        {
            case 0:
                trackSpecial = Instantiate(uTurnTrack, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset));
                trackRotationOffset = trackRotationOffset + new Vector3(0, 180, 0);
                AddTrackTurnOffset();
                trackIntDirection++;
                NormalizeTrackDirection();
                AddTrackTurnOffset();
                trackIntDirection++;
                NormalizeTrackDirection();
                break;
            case 1:            
                trackIntDirection--;
                NormalizeTrackDirection();
                AddTrackTurnOffset();
                trackIntDirection--;
                NormalizeTrackDirection();
                AddTrackTurnOffset();
                trackSpecial = Instantiate(uTurnTrack, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset));
                trackRotationOffset = trackRotationOffset + new Vector3(0, 180, 0);
                break;
            case 2:
                trackSpecial = Instantiate(updownTrack, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset + new Vector3(0, 180, 0)));
                trackPositionOffset = trackPositionOffset + new Vector3(0, 4, 0);
                AddTrackForwardOffset();
                AddTrackForwardOffset();
                break;
            case 3:
                AddTrackForwardOffset();
                AddTrackForwardOffset();
                trackPositionOffset = trackPositionOffset + new Vector3(0, -4, 0);
                trackSpecial = Instantiate(updownTrack, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset));
                break;
            default:
                trackSpecial = Instantiate(uTurnTrack, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset));
                trackRotationOffset = trackRotationOffset + new Vector3(0, 180, 0);
                AddTrackTurnOffset();
                trackIntDirection++;
                NormalizeTrackDirection();
                AddTrackTurnOffset();
                trackIntDirection++;
                NormalizeTrackDirection();
                break;
        }

        Destroy(trackSpecial, trackDestroyDelay);

    }

    // Keeps track of position offsets
    private void AddTrackForwardOffset()
    {
        switch (trackIntDirection)
        {
            case 0:
                trackPositionOffset = trackPositionOffset + new Vector3(0, 0, 10);
                break;
            case 1:
                trackPositionOffset = trackPositionOffset + new Vector3(10, 0, 0);
                break;
            case 2:
                trackPositionOffset = trackPositionOffset + new Vector3(0, 0, -10);
                break;
            case 3:
                trackPositionOffset = trackPositionOffset + new Vector3(-10, 0, 0);
                break;
            default:
                trackPositionOffset = trackPositionOffset + new Vector3(0, 0, 10);
                break;
        }
    }

    // Keeps track of position offsets
    private void AddTrackTurnOffset()
    {
        switch (trackIntDirection)
        {
            case 0:
                trackPositionOffset = trackPositionOffset + new Vector3(10, 0, 10);
                break;
            case 1:
                trackPositionOffset = trackPositionOffset + new Vector3(10, 0, -10);
                break;
            case 2:
                trackPositionOffset = trackPositionOffset + new Vector3(-10, 0, -10);
                break;
            case 3:
                trackPositionOffset = trackPositionOffset + new Vector3(-10, 0, 10);
                break;
            default:
                trackPositionOffset = trackPositionOffset + new Vector3(10, 0, 10);
                break;
        }
    }

    // Keeps track of track direction
    private void NormalizeTrackDirection()
    {
        switch (trackIntDirection)
        {
            case 4:
                trackIntDirection = 0;
                break;
            case -1:
                trackIntDirection = 3;
                break;
            default:
                break;
        }
    }
}

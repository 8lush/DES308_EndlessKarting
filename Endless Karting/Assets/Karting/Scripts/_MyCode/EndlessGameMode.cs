using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessGameMode : MonoBehaviour
{
    [Header("Track Prefabs")]
    public GameObject straightTrack;
    public GameObject trapLeftTrack;
    public GameObject trapRightTrack;
    public GameObject medTurnTrack;
    public GameObject largeTurnTrack;
    public GameObject uTurnTrack;
    public GameObject updownTrack;
    public GameObject scurveRightTrack;
    public GameObject scurveLeftTrack;

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
    private int numofTrackForward = 0;
    private int trapCooldownCurrent = 2;
    private int trapCooldownBase = 1;

    private int currentThreshold = 0;

    private int trackIntDirection = 0;
    
    private bool special = false;

    [SerializeField]
    private List<trackInfo> listTurnTracks = new List<trackInfo>();

    [SerializeField]
    private List<trackInfo> listSpecialTracks = new List<trackInfo>();

    [System.Serializable]
    public struct trackInfo
    {
        public int trackIndex;
    }

    void OnEnable()
    {
        EventManager.StartTrack += InitialTrack;
        EventManager.SpawnNextTrack += NextTrack;
        EventManager.NextThreshold += NextThreshold;
    }

    void OnDisable()
    {
        EventManager.StartTrack -= InitialTrack;
        EventManager.SpawnNextTrack -= NextTrack;
        EventManager.NextThreshold -= NextThreshold;
    }

    private void Start()
    {
        trackSpawnLocation = startingIsland.transform.position + new Vector3(0, 0, 5);
    }

    private void NextThreshold()
    {
        currentThreshold++;
        trackDestroyDelay -= 0.1f;

        NextTrack();
        minNumberofTrackForward++;
        maxNumberofTrackForward++;

        switch (currentThreshold)
        {
            case 1:
                listSpecialTracks.Add(new trackInfo { trackIndex = 1 });
                listSpecialTracks.Add(new trackInfo { trackIndex = 2 });
                listSpecialTracks.Add(new trackInfo { trackIndex = 5 });
                listSpecialTracks.Add(new trackInfo { trackIndex = 6 });
                break;
            case 2:
                listSpecialTracks.Remove(new trackInfo { trackIndex = 0 });

                listTurnTracks.Add(new trackInfo { trackIndex = 2 });
                listTurnTracks.Add(new trackInfo { trackIndex = 3 });

                //trapCooldownBase++;
                break;
            case 3:
                listSpecialTracks.Add(new trackInfo { trackIndex = 3 });
                listSpecialTracks.Add(new trackInfo { trackIndex = 4 });

                listTurnTracks.Remove(new trackInfo { trackIndex = 0 });
                listTurnTracks.Remove(new trackInfo { trackIndex = 1 });

                listSpecialTracks.Remove(new trackInfo { trackIndex = 5 });
                listSpecialTracks.Remove(new trackInfo { trackIndex = 6 });

                trapCooldownBase++;
                break;
            default:
                break;
        }
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
                numofTrackForward = UnityEngine.Random.Range(minNumberofTrackForward, maxNumberofTrackForward + 1);
                break;
            case (0, true):
                TrackSpecial();
                special = !special;
                numofTrackForward = UnityEngine.Random.Range(minNumberofTrackForward, maxNumberofTrackForward + 1);
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
        GameObject trackForward;

        if (trapCooldownCurrent > 0)
        {
            trackForward = Instantiate(straightTrack, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset));

            trapCooldownCurrent--;
        }

        else
        {
            trapCooldownCurrent = trapCooldownBase;

            switch (UnityEngine.Random.Range(0, 2))
            {
                case 0:
                    trackForward = Instantiate(trapLeftTrack, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset));
                    break;
                case 1:
                    trackForward = Instantiate(trapRightTrack, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset));
                    break;
                default:
                    trackForward = Instantiate(trapRightTrack, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset));
                    break;
            }
        }

        Destroy(trackForward, trackDestroyDelay);

        AddTrackForwardOffset();
    }

    // Spawns a left or right turn track
    private void TrackTurn()
    {
        GameObject trackTurn;

        int i = listTurnTracks[UnityEngine.Random.Range(0, listTurnTracks.Count)].trackIndex;

        switch (i)
        {
            case 0:
                AddTrackTurnOffset();
                trackTurn = Instantiate(medTurnTrack, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset + new Vector3(0, -90, 0)));
                trackRotationOffset = trackRotationOffset + new Vector3(0, 90, 0);
                NormalizeTrackDirection(1);
                break;
            case 1:
                trackTurn = Instantiate(medTurnTrack, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset));
                trackRotationOffset = trackRotationOffset + new Vector3(0, -90, 0);
                NormalizeTrackDirection(-1);
                AddTrackTurnOffset();
                break;
            case 2:
                AddTrackTurnOffset();
                AddTrackTurnOffset();
                trackTurn = Instantiate(largeTurnTrack, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset + new Vector3(0, -90, 0)));
                trackRotationOffset = trackRotationOffset + new Vector3(0, 90, 0);
                NormalizeTrackDirection(1);
                break;
            case 3:
                trackTurn = Instantiate(largeTurnTrack, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset));
                trackRotationOffset = trackRotationOffset + new Vector3(0, -90, 0);
                NormalizeTrackDirection(-1);
                AddTrackTurnOffset();
                AddTrackTurnOffset();
                break;
            default:
                AddTrackTurnOffset();
                trackTurn = Instantiate(medTurnTrack, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset + new Vector3(0, -90, 0)));
                trackRotationOffset = trackRotationOffset + new Vector3(0, 90, 0);
                NormalizeTrackDirection(1);
                break;
        }        

        Destroy(trackTurn, trackDestroyDelay);
        
    }

    // Spawns one of the special tracks
    private void TrackSpecial()
    {
        GameObject trackSpecial;

        int i = listSpecialTracks[UnityEngine.Random.Range(0, listSpecialTracks.Count)].trackIndex;

        switch (i)
        {
            case 0:
                TrackTurn();
                trackSpecial = null;
                break;
            case 1:
                trackSpecial = Instantiate(uTurnTrack, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset));
                trackRotationOffset = trackRotationOffset + new Vector3(0, 180, 0);
                AddTrackTurnOffset();
                NormalizeTrackDirection(1);
                AddTrackTurnOffset();
                NormalizeTrackDirection(1);
                break;
            case 2:
                NormalizeTrackDirection(-1);
                AddTrackTurnOffset();
                NormalizeTrackDirection(-1);
                AddTrackTurnOffset();
                trackSpecial = Instantiate(uTurnTrack, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset));
                trackRotationOffset = trackRotationOffset + new Vector3(0, 180, 0);
                break;
            case 3:
                trackSpecial = Instantiate(updownTrack, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset + new Vector3(0, 180, 0)));
                trackPositionOffset = trackPositionOffset + new Vector3(0, 4, 0);
                AddTrackForwardOffset();
                AddTrackForwardOffset();
                break;
            case 4:
                AddTrackForwardOffset();
                AddTrackForwardOffset();
                trackPositionOffset = trackPositionOffset + new Vector3(0, -4, 0);
                trackSpecial = Instantiate(updownTrack, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset));
                break;
            case 5:
                trackSpecial = Instantiate(scurveRightTrack, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset));
                AddTrackForwardOffset();
                AddTrackTurnOffset();
                break;
            case 6:
                trackSpecial = Instantiate(scurveLeftTrack, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset));
                AddTrackForwardOffset();
                NormalizeTrackDirection(-1);
                AddTrackTurnOffset();
                NormalizeTrackDirection(1);
                break;
            default:
                TrackTurn();
                trackSpecial = null; 
                break;
        }

        Destroy(trackSpecial, trackDestroyDelay);

        trapCooldownCurrent = trapCooldownBase;
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
    private void NormalizeTrackDirection(int trackTurn)
    {
        bool positive = trackTurn > 0;
        trackTurn = Math.Abs(trackTurn);

        for (int i = trackTurn; i > 0; i--)
        {
            if (positive)
            {
                trackIntDirection++;
            }
            else
            {
                trackIntDirection--;
            }

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
}

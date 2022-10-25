using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessGameMode : MonoBehaviour
{
    public GameObject straightTrack;
    public GameObject medTurnTrack;
    public GameObject specialTrack1;

    Vector3 trackSpawnLocation = new Vector3(140, 15f, -50);
    Vector3 trackPositionOffset = new Vector3(0, 0, 0);
    Vector3 trackRotationOffset = new Vector3(0, 0, 0);

    public float trackDestroyDelay = 5f;
    public float trackInstantiationFrequency = 2f;

    public string trackDirection = "Front";
    private int trackIntDirection = 0;


    void Start()
    {
        InvokeRepeating("BuildRoad", 1.0f, 3.5f);
    }

    private void BuildRoad()
    {
        StartCoroutine(StraightTrackPieces());  
    }

    private IEnumerator StraightTrackPieces()
    {
        for (int numberofStraightTracks = 0; numberofStraightTracks < 3; numberofStraightTracks++)
        {
            TrackForward();
            yield return new WaitForSeconds(0.3f);
        }
        TrackTurn();
        TrackSpecial();
    }

    private void TrackForward()
    {
        GameObject trackForward = Instantiate(straightTrack, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset));

        Destroy(trackForward, trackDestroyDelay);

        AddTrackForwardOffset();
    }

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

    private void TrackSpecial()
    {
        GameObject trackSpecial;

        switch (Random.Range(0, 2))
        {
            case 0:
                trackSpecial = Instantiate(specialTrack1, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset));
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
                trackSpecial = Instantiate(specialTrack1, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset));
                trackRotationOffset = trackRotationOffset + new Vector3(0, 180, 0);
                break;
            default:
                trackSpecial = Instantiate(specialTrack1, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset));
                trackRotationOffset = trackRotationOffset + new Vector3(0, 90, 0);
                trackIntDirection++;
                NormalizeTrackDirection();
                break;
        }

        Destroy(trackSpecial, trackDestroyDelay);

    }

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

    private void AddTrackTurnOffset()
    {
        switch (trackIntDirection)
        {
            case 0:
                trackDirection = "Front";
                trackPositionOffset = trackPositionOffset + new Vector3(10, 0, 10);
                break;
            case 1:
                trackDirection = "Right";
                trackPositionOffset = trackPositionOffset + new Vector3(10, 0, -10);
                break;
            case 2:
                trackDirection = "Back";
                trackPositionOffset = trackPositionOffset + new Vector3(-10, 0, -10);
                break;
            case 3:
                trackDirection = "Left";
                trackPositionOffset = trackPositionOffset + new Vector3(-10, 0, 10);
                break;
            default:
                trackDirection = "Left";
                trackPositionOffset = trackPositionOffset + new Vector3(-10, 0, 10);
                break;
        }
    }

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

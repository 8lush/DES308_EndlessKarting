using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessGameMode : MonoBehaviour
{
    public GameObject straightTrack;
    public GameObject medTurnTrack;
    public GameObject uTurnTrack;
    public GameObject updownTrack;

    public Vector3 trackSpawnLocation = new Vector3(100, 50f, -90);
    Vector3 trackPositionOffset = new Vector3(0, 0, 0);
    Vector3 trackRotationOffset = new Vector3(0, 0, 0);

    public float trackSpawnDelay = 0.5f;
    public float trackDestroyDelay = 5f;
    public float trackInstantiationFrequency = 2f;

    public int minNumberofTrackForward = 2;
    public int maxNumberofTrackForward = 5;

    private int trackIntDirection = 0;

    private int b;
    private bool special = false;


    void Start()
    {
        InvokeRepeating("BuildRoad", 1.0f, trackInstantiationFrequency);
    }

    private void BuildRoad()
    {
        StartCoroutine(StraightTrackPieces());  
    }

    private IEnumerator StraightTrackPieces()
    {
        b = Random.Range(minNumberofTrackForward, maxNumberofTrackForward + 1);
        for (int i = 0; i < b; i++)
        {
            TrackForward();
            yield return new WaitForSeconds(trackSpawnDelay);
        }
        if (special)
        {
            TrackSpecial();
            special = !special;
        }
        else 
        {
            TrackTurn();
            special = !special;
        }      
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessGameMode : MonoBehaviour
{
    public GameObject straightTrack;
    public GameObject medTurnTrack;

    Vector3 trackSpawnLocation = new Vector3(140, 15f, -50);
    Vector3 trackPositionOffset = new Vector3(0, 0, 0);
    Vector3 trackRotationOffset = new Vector3(0, 0, 0);

    public float roadDestroyTimer = 1.0f;

    public string trackDirection = "Front";


    void Start()
    {
        InvokeRepeating("BuildRoad", 1.0f, 2.0f);
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
            yield return new WaitForSeconds(0.1f);
        }
        TrackTurn();
    }

    private void TrackForward()
    {
        GameObject road = Instantiate(straightTrack, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset));

        Destroy(road, roadDestroyTimer);

        switch (trackDirection)
        {
            case "Right":
                trackPositionOffset = trackPositionOffset + new Vector3(10, 0, 0);
                break;
            case "Left":
                trackPositionOffset = trackPositionOffset + new Vector3(-10, 0, 0);
                break;
            case "Back":
                trackPositionOffset = trackPositionOffset + new Vector3(0, 0, -10);
                break;
            case "Front":
                trackPositionOffset = trackPositionOffset + new Vector3(0, 0, 10);
                break;
            default:
                trackPositionOffset = trackPositionOffset + new Vector3(0, 0, 10);
                break;
        }
    }

    private void TrackTurn()
    {
        GameObject road = Instantiate(medTurnTrack, trackSpawnLocation + trackPositionOffset, Quaternion.Euler(trackRotationOffset));

        Destroy(road, roadDestroyTimer);

        trackRotationOffset = trackRotationOffset + new Vector3(0, -90, 0);

        switch (trackDirection)
        {
            case "Right":
                trackDirection = "Front";
                trackPositionOffset = trackPositionOffset + new Vector3(10, 0, 10);
                break;
            case "Left":
                trackDirection = "Back";
                trackPositionOffset = trackPositionOffset + new Vector3(-10, 0, -10);
                break;
            case "Back":
                trackDirection = "Right";
                trackPositionOffset = trackPositionOffset + new Vector3(10, 0, -10);
                break;
            case "Front":
                trackDirection = "Left";
                trackPositionOffset = trackPositionOffset + new Vector3(-10, 0, 10);
                break;
            default:
                trackDirection = "Left";
                trackPositionOffset = trackPositionOffset + new Vector3(-10, 0, 10);
                break;
        }
    }
}

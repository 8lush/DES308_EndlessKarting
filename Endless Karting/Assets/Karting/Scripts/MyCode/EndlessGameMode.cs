using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessGameMode : MonoBehaviour
{
    public GameObject road;

    Vector3 trackSpawnLocation = new Vector3(140, 2f, -50);
    Vector3 trackOffset = new Vector3(0, 0, 0);

    public float roadDestroyTimer = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("BuildRoad", 1.0f, 3.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void BuildRoad()
    {
        StartCoroutine(StraightTrackPieces());
    }

    private IEnumerator StraightTrackPieces()
    {
        for (int numberofStraightTracks = 0; numberofStraightTracks < 3; numberofStraightTracks++)
        {
            InstantiateTrackPiece();
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void InstantiateTrackPiece()
    {
        GameObject dRoad = Instantiate(road, trackSpawnLocation + trackOffset, Quaternion.identity);

        Destroy(dRoad, roadDestroyTimer);

        trackOffset = trackOffset + new Vector3(0, 0, 10);
    }
}

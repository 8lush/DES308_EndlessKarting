using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class TrackDeletionManager : MonoBehaviour
{
    public List<GameObject> trackPieces = new List<GameObject>();

    private List<float> deleteTimers = new List<float>();

    [Header("Track Destroy Variables")]
    [SerializeField] private float speed = 1f;
    [SerializeField] private float trackLifespan = 4f;
    [SerializeField] private float trackDown = 1f;

    private bool initialSpawn = true;

    [SerializeField] private int yellowTracks = 3;
    [SerializeField] private int redTracks = 2;
    private int colourTracks;

    // Working variables
    private int trackIndex = 0;
    private float deleteTimer;

    public float maxQueueCount = 15;
    private bool maxQueueExceeded = false;

    GameObject trackPiece;

    private void Start()
    {
        trackDown = trackLifespan / 100 * trackDown;
    }

    void FixedUpdate()
    {
        if (trackPieces.Count != 0)
        {
            bool first = true;

            colourTracks = yellowTracks + redTracks;

            if(trackPieces.Count < colourTracks)
            {
                colourTracks = trackPieces.Count - 1;
            }

            for (int i = 0; i < colourTracks; i++)
            {
                switch (first)
                {
                    case true:
                        trackIndex = trackPieces.IndexOf(trackPieces.First());
                        //deleteTimer = deleteTimers[trackIndex];
                        break;
                    case false:
                        trackIndex = trackPieces.IndexOf(trackPieces.GetNextInCycle(trackPieces[trackIndex]));
                        break;
                }

                trackPiece = trackPieces[trackIndex];

                maxQueueExceeded = maxQueueCount <= trackPieces.Count;

                switch (maxQueueExceeded, first)
                {
                    case (true, true):
                        deleteTimers[trackIndex] -= Time.deltaTime * 5f;
                        break;
                    case (false, true):
                        deleteTimers[trackIndex] -= Time.deltaTime;
                        break;
                    default:
                        break;
                }


                if (deleteTimers[trackIndex] <= 0f && first)
                {
                    Destroy(trackPiece);
                    trackPieces.RemoveAt(trackIndex);

                    deleteTimers.RemoveAt(trackIndex);
                }   
                else if (deleteTimers[trackIndex] <= trackDown && first)
                {
                    // Move object downwards
                    trackPiece.transform.position += Vector3.down * speed * Time.deltaTime;
                }
                else if (i < redTracks)
                {
                    trackPiece.GetComponent<Renderer>().material.color = Color.red;
                }
                else if (trackPieces.Count < colourTracks)
                {
                    break;
                }
                else if (i <= (redTracks + yellowTracks))
                {
                    trackPiece.GetComponent<Renderer>().material.color = Color.yellow;
                }

                first = false;
            }
        }
        
    }

    public void AddToList(GameObject trackReference, float extraLifespan)
    {
        trackPieces.Add(trackReference);

        extraLifespan = trackLifespan / 100 * extraLifespan;

        if (initialSpawn)
        {
            initialSpawn = false;

            extraLifespan = 3.5f;
        }

        int index = trackPieces.IndexOf(trackReference);
        deleteTimers.Insert(index, trackLifespan + extraLifespan);
    }

}

using Codice.CM.Common;
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
    [SerializeField] private float trackRed = 2f;
    [SerializeField] private float trackYellow = 2f;
    [SerializeField] private float trackDown = 1f;

    // Working variables
    private int trackIndex = 0;
    private float deleteTime;

    public float maxQueueCount = 15;
    private bool maxQueueExceeded = false;

    GameObject trackPiece;


    void Update()
    {

        if (trackPieces.Count != 0)
        {
            bool first = false;

            for (int i = 0; i < trackPieces.Count; i++)
            {
                switch (first)
                {
                    case true:
                        trackIndex = trackPieces.IndexOf(trackPieces.First());
                        first = false;
                        break;
                    case false:
                        trackIndex = trackPieces.IndexOf(trackPieces.GetNextInCycle(trackPieces[trackIndex]));
                        break;
                }

                trackPiece = trackPieces[trackIndex];
                deleteTime = deleteTimers[trackIndex];

                maxQueueExceeded = maxQueueCount <= trackPieces.Count;

                switch (maxQueueExceeded)
                {
                    case true:
                        deleteTimers[trackIndex] -= Time.deltaTime * 3f;
                        break;
                    case false:
                        deleteTimers[trackIndex] -= Time.deltaTime;
                        break;
                }


                if (deleteTime <= 0f)
                {
                    Destroy(trackPiece);
                    trackPieces.RemoveAt(trackIndex);

                    deleteTimers.RemoveAt(trackIndex);
                }
                else if (deleteTime <= trackDown)
                {
                    // Move object downwards
                    trackPiece.transform.position += Vector3.down * speed * Time.deltaTime;
                }
                else if (deleteTime <= trackRed)
                {
                    trackPiece.GetComponent<Renderer>().material.color = Color.red;
                }
                else if (deleteTime <= trackYellow)
                {
                    trackPiece.GetComponent<Renderer>().material.color = Color.yellow;
                }
            }
        }
        
    }

    public void AddToList(GameObject trackReference)
    {
        trackPieces.Add(trackReference);

        int index = trackPieces.IndexOf(trackReference);
        deleteTimers.Insert(index, trackLifespan);
    }

}

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
    [SerializeField] private float trackDown = 1f;
    [SerializeField] private float trackDestroyDelay = 1f;

    // Working variables
    private float _deleteDelay1 = 2f;
    private float _deleteDelay2 = 1f;

    private int trackIndex = 0;
    private float deleteTime;

    public float maxQueueCount = 15;

    // Tracking variables
    private int deletedTracks = 0;
    private bool deleting = false;

    GameObject trackPiece;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
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
                        //trackIndex = trackPieces.IndexOf(trackPiece);
                        trackIndex = trackPieces.IndexOf(trackPieces.First());
                        first = false;
                        break;
                    case false:
                        trackIndex = trackPieces.IndexOf(trackPieces.GetNextInCycle(trackPieces[trackIndex]));
                        break;
                }

                trackPiece = trackPieces[trackIndex];

                deleteTime = deleteTimers[trackIndex];
                deleteTimers[trackIndex] -= Time.deltaTime;

                if (deleteTime <= 0f)
                {
                    Destroy(trackPiece);
                    deletedTracks++;
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
            }

            /*
            if (trackPieces.Count > maxQueueCount)
            {
                _deleteDelay1 = 0;
                _deleteDelay2 = 0;
            }

            if (!deleting)
            {
                deleting = true;
                trackPiece = trackPieces.First();
            }

            // Turn object red
            trackPiece.GetComponent<Renderer>().material.color = Color.red;

            _deleteDelay1 -= Time.deltaTime;

            // Start moving object down after deleteDelay1 amount of time
            if (_deleteDelay1 <= 0)
            {
                // Move object downwards
                trackPiece.transform.position += Vector3.down * speed * Time.deltaTime;

                _deleteDelay2 -= Time.deltaTime;
            }

            // Delete object after deleteDelay2 amount of time
            if (_deleteDelay2 <= 0)
            {
                Destroy(trackPiece);
                deletedTracks++;
                trackPieces.Dequeue();

                _deleteDelay1 = deleteDelay1;
                _deleteDelay2 = deleteDelay2;
                deleting = false;
            } */
        }
        
    }

    public void AddToList(GameObject trackReference)
    {
        trackPieces.Add(trackReference);
        Debug.Log("List count " + trackPieces.Count);

        int index = trackPieces.IndexOf(trackReference);
        deleteTimers.Insert(index, trackLifespan);
    }

    private IEnumerator DeleteTrack(float time)
    {
        yield return new WaitForSeconds(time);
    }
}

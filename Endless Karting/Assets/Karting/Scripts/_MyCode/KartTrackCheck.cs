using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KartTrackCheck : MonoBehaviour
{

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Track"))
        {
            EventManager.EventSpawnNextTrack();
        }
    }
}

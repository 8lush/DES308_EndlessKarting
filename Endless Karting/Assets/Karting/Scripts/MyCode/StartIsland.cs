using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartIsland : MonoBehaviour
{

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EventManager.EventStartTrack();
            Destroy(gameObject, 1f);
        }
    }

    void DestroySelf()
    {
        
    }
}

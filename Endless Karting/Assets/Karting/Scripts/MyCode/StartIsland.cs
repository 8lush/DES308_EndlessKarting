using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartIsland : MonoBehaviour
{

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            EventManager.ActivateStartTrack();
            DestroySelf();
        }
    }

    void DestroySelf()
    {
        Destroy(gameObject, 1f);
    }
}

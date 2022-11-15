using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartIsland : MonoBehaviour
{
    private bool used = false;

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Player") && !used)
        {
            used = true;
            EventManager.EventStartTrack();
            Destroy(transform.parent.gameObject, 1f);
        }
    }

}

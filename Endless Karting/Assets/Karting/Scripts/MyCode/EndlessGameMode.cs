using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndlessGameMode : MonoBehaviour
{
    public GameObject road;


    // Start is called before the first frame update
    void Start()
    {
        GameObject dRoad = Instantiate(road, new Vector3(160, -0.2f, -50), Quaternion.identity);

        Destroy(dRoad, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}

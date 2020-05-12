using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppearObstacle : MonoBehaviour
{
    [SerializeField] Vector3 moveFactor;

    Vector3 startingPos;
    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Rocket.rocketPos.x >= 21.5f )
        {
            transform.position = startingPos + moveFactor;
        }
    }
}

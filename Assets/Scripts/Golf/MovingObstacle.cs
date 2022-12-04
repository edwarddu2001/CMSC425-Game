using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObstacle : MonoBehaviour
{
    [SerializeField]
    private Vector3 from;
    [SerializeField]
    private Vector3 to;




    // Update is called once per frame
    void Update()
    {
   
        transform.position = Vector3.Lerp(from, to, Mathf.PingPong(Time.time, 1));
    }
}

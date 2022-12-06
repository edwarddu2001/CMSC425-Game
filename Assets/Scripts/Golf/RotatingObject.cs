using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingObject : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        float y = transform.eulerAngles.y;
        float z = transform.eulerAngles.z;
        transform.rotation = Quaternion.Euler((15 * Mathf.Sin(Time.time * 5)), y, z);
    }
}

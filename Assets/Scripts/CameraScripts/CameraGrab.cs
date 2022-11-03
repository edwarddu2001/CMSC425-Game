using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGrab : MonoBehaviour
{
    public GameObject mainCamera; 
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void OnMouseDown()
    {
        mainCamera.transform.parent = gameObject.transform;
        mainCamera.transform.position = gameObject.transform.position;
        mainCamera.transform.rotation = gameObject.transform.rotation;
    }


}

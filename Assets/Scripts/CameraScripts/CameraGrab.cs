using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGrab : MonoBehaviour
{

    private Camera mainCamera; 
    public float fov; 
    void Start(){
        mainCamera = Camera.main;
        mainCamera.fieldOfView = 80;


    }
    
    void OnMouseDown()
    {
        mainCamera.transform.parent = gameObject.transform;
        mainCamera.transform.position = gameObject.transform.position;
        mainCamera.transform.rotation = new Quaternion(0,0,0,0);
    }


}

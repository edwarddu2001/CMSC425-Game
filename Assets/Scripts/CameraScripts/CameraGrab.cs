using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraGrab : MonoBehaviour
{

    private Camera mainCamera; 
    void Start(){
        mainCamera = Camera.main;
    }
    void OnMouseDown()
    {
        mainCamera.transform.position = gameObject.transform.position;
        mainCamera.transform.rotation = gameObject.transform.rotation;
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraHolder : MonoBehaviour
{

    [SerializeField]
    private Transform playerTransform;


    private void Start()
    {
        
    }
    // Update is called once per frame
    void Update()
    {

        transform.position = playerTransform.position;
    }
}

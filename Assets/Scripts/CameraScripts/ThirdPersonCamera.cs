using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [SerializeField]
    private float xrotation;
    [SerializeField]
    private float yrotation;

    public float sensitivity;

    private float maxZoomOut = 10f;
    private float maxZoomIn = -2f;
    private float currZoom = 0;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        cam = gameObject.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.mouseScrollDelta.y < 0 && currZoom + 1f <= maxZoomOut)
        {
            currZoom += 1f;
            Debug.Log("zoomout");
        }
        else if (Input.mouseScrollDelta.y > 0 && currZoom - 1f >= maxZoomIn)
        {
            currZoom -= 1f;
            Debug.Log("zoomin");
        }

        cam.fieldOfView = 60 + currZoom*4;

        xrotation += Input.GetAxis("Mouse X") * sensitivity;
        xrotation %= 360f;
        float yTemp = Input.GetAxis("Mouse Y") * sensitivity;
        if(yrotation + yTemp <= 30 && yrotation + yTemp >= -90) { yrotation += yTemp; }
        //yrotation += Input.GetAxis("Mouse Y") * sensitivity;
        transform.parent.transform.localRotation = Quaternion.Euler(-yrotation, xrotation, 0);

        
    }
}

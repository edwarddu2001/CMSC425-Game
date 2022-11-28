using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslatingCamera : MonoBehaviour
{
    public GameObject player;
    public float xOffset, yOffset, zOffset;
    //public float sensitivity = 0.5f;
    // Update is called once per frame
    void Update()
    {
        // reverse camera with Shift + Down Arrow
        if (Input.GetKey(KeyCode.LeftShift)) {
            if (Input.GetKeyDown(KeyCode.DownArrow)){
                zOffset = -1.0f * zOffset;
            }

        }

        

        if(player.GetComponent<ShrinkAbility>().isShrunk) { 
            transform.position = player.transform.position + new Vector3(xOffset, yOffset/2, zOffset/2);
        }
        else {
            transform.position = player.transform.position + new Vector3(xOffset, yOffset,zOffset);
        
        }
        transform.LookAt(player.transform.position);

        // rotate camera with mouse
        float rotateHorizontal = Input.GetAxis("Mouse X");
        float rotateVertical = Input.GetAxis("Mouse Y");
        /* transform.RotateAround(player.transform.position, -Vector3.up, rotateHorizontal * sensitivity); 
        transform.RotateAround(Vector3.zero, transform.right, rotateVertical * sensitivity); 
        */
        //use transform.Rotate(-transform.up * rotateHorizontal * sensitivity) instead if you dont want the camera to rotate around the player
    }
}

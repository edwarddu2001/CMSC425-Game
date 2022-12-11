using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Replaces WASD player movement with a labyrinth puzzle-like system,
 where the map instead rotates in the xz plane with WASD.

 W: z+
 S: z-
 A: x+
 D: x-

 Two things to note.
1. For static parts of the map (floors, walls, pads, etc.), they should all
   be children of a single parent object AT THE CENTER OF THE MAP WITH THIS
   SCRIPT. that way, everything will rotate consistently. For "loose" objects
   including the player itself, do not follow this rule.

2. Loose objects only seem to obey correct collision laws if the of the map is NOT
   set to rotation angles of (0,0,0). To remedy, make one angle start at something
   close, such as 1 or -1.*/

public class LabyrinthRotate : MonoBehaviour
{
    Quaternion originalRot;
    float rotAboutX, rotAboutZ;
    float rotX, rotY, rotZ;
    //To change the max amount the map is allowed to rotate in any direction, change this
    //Moved this here from Update() and made it a float -Zach
    public float maxAngle = 10;
    //Collider collie;

    // Start is called before the first frame update
    void Start()
    {
        originalRot = transform.rotation;
        /*rotAboutX = 0;
        rotAboutZ = 0;*/

        //collie = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentOrientation = transform.eulerAngles;
        rotX = currentOrientation.x;
        rotY = currentOrientation.y;
        rotZ = currentOrientation.z;

        
        


        if (Input.GetKey("d"))
        {
            if (rotZ > 360 - maxAngle - 10 || rotZ < maxAngle)
                {
                //Debug.Log(rotZ);
                transform.rotation = transform.rotation * Quaternion.Euler(Vector3.forward * 0.06f);
                //collie.transform.rotation = transform.rotation * Quaternion.Euler(Vector3.back * 0.02f);
            }
        }
        if (Input.GetKey("a"))
        {
            if (rotZ > 360 - maxAngle || rotZ < maxAngle + 10)
            {
                //Debug.Log(rotZ);
                transform.rotation = transform.rotation * Quaternion.Euler(Vector3.back * 0.06f);
                //collie.transform.rotation = transform.rotation * Quaternion.Euler(Vector3.forward * 0.02f);
            }
        }

        if (Input.GetKey("w"))
        {
            if (rotX > 360 - maxAngle || rotX < maxAngle + 10)
            {
                //Debug.Log(rotX);
                transform.rotation = transform.rotation * Quaternion.Euler(Vector3.left * 0.06f);
                //collie.transform.rotation = transform.rotation * Quaternion.Euler(Vector3.right * 0.02f);
            }
        }
        if (Input.GetKey("s"))
        {
            if (rotX > 360 - maxAngle - 10 || rotX < maxAngle)
            {
                //Debug.Log(rotX);
                transform.rotation = transform.rotation * Quaternion.Euler(Vector3.right * 0.06f);
                //collie.transform.rotation = transform.rotation * Quaternion.Euler(Vector3.left * 0.02f);
            }
        }
    }

    public void returnToOriginalRotation()
    {
        transform.rotation = originalRot;
    }
}

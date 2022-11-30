using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetUpShot : MonoBehaviour
{
    public GameObject hole;
    public GameObject ball;
    private Vector3 oldPathToHole, newPathToHole;

    // Start is called before the first frame update
    void Start()
    {
        oldPathToHole = hole.transform.position - transform.position;
        oldPathToHole.y = 0;
        oldPathToHole = Vector3.Normalize(oldPathToHole);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = ball.transform.position;


        //calculate path to hole
        newPathToHole = hole.transform.position - transform.position;
        newPathToHole.y = 0;
        newPathToHole = Vector3.Normalize(newPathToHole);

        //Debug.Log(pathToHole);
        //Quaternion q = new Quaternion();
        //q.eulerAngles = pathToHole;
        //shotArrow.transform.rotation = q;
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("realigning");
            transform.rotation = transform.rotation * Quaternion.FromToRotation(oldPathToHole, newPathToHole);
            oldPathToHole = newPathToHole;
        }
        /*transform.rotation = transform.rotation * Quaternion.FromToRotation(oldPathToHole, newPathToHole);
        oldPathToHole = newPathToHole;*/
        //shotArrow.transform.rotation = shotArrow.transform.rotation * Quaternion.FromToRotation(oldPathToHole, newPathToHole);

    }
}

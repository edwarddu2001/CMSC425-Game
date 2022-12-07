using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*In the controls menu, this script displays the direction, loft, and power of your shot while you're setting up for
 a new shot. Why make it in a seperate script? Purely for organization reasons. The ControlsUI script is long enough as it is,
 needing many lines to figure out what controls are enabled.*/

public class ShotStatsUI : MonoBehaviour
{
    public Image shotDirection;
    public Image shotLoft;
    //public GameObject shotArrowRepresentation;
    public Slider shotPower;

    private bool isEnabled = true;

    public GameObject playerContainer;
    private GameObject playerBall;
    private GameObject shotArrow;

    private MoveGolf ballMovement;
    //private Vector3 currDirection;
    private float currXZangle;
    private float currSpeed;

    // Start is called before the first frame update
    void Start()
    {
        playerBall = playerContainer.transform.GetChild(0).gameObject;
        shotArrow = playerContainer.transform.GetChild(1).gameObject;

        ballMovement = playerBall.GetComponent<MoveGolf>();

        //set slider stats
        shotPower.minValue = ballMovement.minSpeedFactor;
        shotPower.maxValue = ballMovement.maxSpeedFactor;

    }

    // Update is called once per frame
    void Update()
    {
        if (isEnabled)
        {
            //currDirection = ballMovement.getShotAngle();
            currXZangle = ballMovement.getShotAngle();
            currSpeed = ballMovement.getShotPower();

            shotPower.value = currSpeed;
            //shotArrowRepresentation.transform.rotation = ballMovement.getShotRotation();
            //shotDirection.rectTransform.Rotate(Vector3.forward, Vector3.SignedAngle(Vector3.up, currDirection, Vector3.forward));
            /*Quaternion q = ballMovement.getShotRotation();
            Debug.Log(q);*/

            //we think this works, problem is it's always rotating by that angle...so what to do?
            Quaternion q = Quaternion.Euler(new Vector3(0, 0, currXZangle));
            shotDirection.rectTransform.rotation = q;
            //shotDirection.rectTransform.Rotate(Vector3.forward, currXZangle);
            //shotLoft.rectTransform.rotation = ballMovement.getShotRotation();
        }
    }
}

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
    public TextMeshProUGUI rotDegrees;
    public TextMeshProUGUI loftDegrees;
    //public GameObject shotArrowRepresentation;
    public Slider shotPower;
    public TextMeshProUGUI shotPowerText;

    private bool isEnabled = true;

    public GameObject playerContainer;
    private GameObject playerBall;
    private GameObject shotArrow;

    private MoveGolf ballMovement;
    //private Vector3 currDirection;
    private float currXZangle;
    private float currLoft;
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
            currLoft = ballMovement.getShotLoft();

            //can change with some abilities.
            shotPower.maxValue = ballMovement.maxSpeedFactor;
            shotPower.value = currSpeed;
            //this will ensure speed is always displayed with 2 decimal places.
            float speedVal = ((int)(currSpeed * 100) / 100.0f);
            shotPowerText.SetText("Power: " + speedVal );

            //"Angle" is the angle from straight ahead (Vector3.forward) to our ball's direction, solely in the XZ plane.
            Quaternion q = Quaternion.Euler(new Vector3(0, 0, currXZangle+180));
            shotDirection.rectTransform.rotation = q;

            //"Loft" is the angle from the XZ plane to our direction. unless you have chipshot or ZeroGrav, loft is always 0.
            q = Quaternion.Euler(new Vector3(0, 0, currLoft));
            shotLoft.rectTransform.rotation = q;

            //set text for these values
            if(currXZangle < 0) { rotDegrees.SetText("Angle: " + (180 + (int)currXZangle).ToString() + "W"); }
            else { rotDegrees.SetText("Angle: " + (180 - (int)currXZangle).ToString() + "E"); }
            if (Mathf.Abs(currLoft) < 1) { loftDegrees.SetText("NO LOFT"); }
            else { loftDegrees.SetText("Loft: " + ((int)currLoft).ToString()); }
        }
    }
}

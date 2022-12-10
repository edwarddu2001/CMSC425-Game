using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//controls all things related to movement of the golf ball.
public class MoveGolf : MonoBehaviour
{
    //BIG IDEA: if not inMotion, set up and take your next shot. if in motion, allow the ball to move / jump / etc.
    [SerializeField]
    bool inMotion = true;
    float timeSlowed = 0;

    //public variables that control our shot: how far we can hit it, what we can do with it, etc.
    public float maxSpeedFactor = 5.0f, minSpeedFactor = 0.2f;
    public float stoppingRate = 0.8f;
    public float jumpHeight = 20.0f;
    public float spinRate = 0; public float spinTime = 0.0f;
    public GameObject shotArrow;
    private float speed;
    public float defaultSpeed = 10.0f;
    private Vector3 direction;

    //chipRotation is changed as you angle the shot up/down for a chip shot. 1.0 is the default, a flat shot on the ground.
    //you can rotate it anywhere in the range (0, 100)
    private float chipRotation = 0.0f;
    private float yAxis;

    //controls physics
    private Vector3 playerVelocity;
    private Rigidbody rbody; //new
    private float timeSinceLastShot = 0;

    //controls the arrow mesh, for when we change its appearance
    private MeshFilter shotArrowMF;
    private Mesh shotArrowMesh;
    private Vector3[] oldVerts;
    private MeshRenderer shotArrowMR;

    //keeps score
    private ScorecardScript scorecard;

    //tracks current ability
    private AbObserver2 observer;

    //"labyrinth mode" is a special mode that completely changes movement, when you have the labyrinth ability
    private bool labyrinthMode = false, movingLabyrinth = false;
    private LabyrinthRotate rotatableObjectScript;
    private AbilityPickup2[] abilitiesList;

    //we'd better tell our friendly player what the controls are...
    public ControlsUI controlUI;

    float currSpeed = 1.0f;

    //this keeps track of how many "Ground" labelled objects the ball is colliding with. If 0, we're in midair.
    [SerializeField]
    private int inContactWithGround = 0;

    [SerializeField]
    private AudioSource[] sounds = new AudioSource[3];

    //initialize stuff
    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        rbody.velocity = Vector3.zero;

        shotArrow.SetActive(false);

        //"shotArrow" refers to the 3D arrow jutting out of your ball.
        //to signify rotation or loft, we simply rotate it in the respective direction.
        //to signify a change in power, we'll increase/decrease the size of the thing, and change color
        GameObject shotArrowHead = shotArrow.transform.GetChild(0).gameObject;
        shotArrowMF = shotArrowHead.GetComponent<MeshFilter>();
        shotArrowMesh = shotArrowMF.mesh;
        oldVerts = shotArrowMesh.vertices;
        shotArrowMR = shotArrowHead.GetComponent<MeshRenderer>();

        //set initial color of shotArrow- blue = soft, red = powerful...
        shotArrowMR.material.SetColor("_Color", new Color(1.0f / maxSpeedFactor, 0, (maxSpeedFactor - 1.0f) / maxSpeedFactor, 1.0f));

        //find observer
        observer = GetComponent<AbObserver2>();

        //the so-called "global" scoring system
        scorecard = transform.parent.GetComponent<ScorecardScript>();

    }

    //expand or shrink the shot arrow, to represent a stronger or weaker shot
    void resizeShotArrow(float scale)
    {
        Vector3[] newVerts = shotArrowMesh.vertices;
        //don't need to modify triangles or normals for this simple operation.
        Color newcol = new Color(0, 0, 0);

        for (var i = 0; i < newVerts.Length; i++)
        {
            newVerts[i].x = oldVerts[i].x * scale;

            newVerts[i].x -= (scale - 1.0f) * 5.0f;

            newcol = new Color(scale / maxSpeedFactor, 0, (maxSpeedFactor - scale) / maxSpeedFactor, 1.0f);
        }

        shotArrowMesh.vertices = newVerts;
        shotArrowMR.material.SetColor("_Color", newcol);
    }


    //unfortunately, it only makes sense to run most code in update since too much can be happening all at once.
    void Update()
    {
        //all abilities have normal golf ball movement, EXCEPT the labyrinth ability in labyrinth mode.
        if (!movingLabyrinth)
        {
            /*for brevity's sake, "almost stopped moving" = not moving.
            We decide the shot is over when the ball has nearly stopped moving for at least 2 seconds.*/
            bool movingFast = (rbody.velocity.magnitude > 0.2f);
            if (!movingFast)
            {
                if (timeSlowed > 2.0f)
                {
                    if (inMotion == true)
                    {
                        inMotion = false;
                        /*problem: the player might lose chipshot or zero-grav, and be unable to change their loft again.
                         * solution: reset loft on every shot. loft is pretty risky to use most of the time anyway*/
                        Quaternion q = shotArrow.transform.rotation;
                        shotArrow.transform.rotation = new Quaternion(0, q.y, 0, q.w);
                        chipRotation = 0;
                        
                        reportChangeInState(false, observer.ability);
                    }

                }
                else
                {
                    //if the ball is slow, but it hasn't been the threshold of 2 seconds yet, just keep counting
                    timeSlowed += Time.deltaTime;
                }
            }
            else
            {
                timeSlowed = 0.0f;
                inMotion = true;
            }

            //NOT IN MOTION: draw the arrow and line up your shot
            if (!inMotion)
            {
                if (!labyrinthMode)
                {
                    ShotSetup();
                }
                else
                {
                    LabyrinthSetup();
                }
            }


            //IN MOTION: the ball is now moving. allow for jumping
            else
            {
                if (!labyrinthMode)
                {
                    moveNormally();
                }
            }
        }
        //when we are actively in labyrinth mode, call that method there.
        else
        {
            moveLabyrinth();
        }


    }


    //movement with the labyrinth ability is completely different
    //it's done by rotating the map, however, the unity physics engine has a tough time with this...
    //it will incorrectly assume the rigidbody is staying still, seemingly ignoring the collisions happening.
    //so we add little love taps to the ball to keep it moving in the direction of rotation.
    void moveLabyrinth()
    {
        inMotion = true;

        if (Input.GetKey(KeyCode.A))
        {
            //rbody.AddForce(Vector3.right * 0.01f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            //rbody.AddForce(Vector3.left * 0.01f);
        }
        if (Input.GetKey(KeyCode.W))
        {
            //rbody.AddForce(Vector3.up * 0.01f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            //rbody.AddForce(Vector3.back * 0.01f);
        }

        //end your "labyrinth rotation", go back to normal.
        if (Input.GetKey(KeyCode.F))
        {
            toggleAllAbilities(true);

            labyrinthMode = false;
            movingLabyrinth = false;
            rotatableObjectScript.enabled = false;

            timeSlowed = 0;
            reportLabyrinthChangeInState();
            reportChangeInState(true, observer.ability);
        }
    }

    /*call this method if you have to respawn and you're moving with labyrinth at the moment.
    it's necessary to reset movement back to normal, AND return the map to its original rotation.
    otherwise the player can mess up the map permanently by rotating things poorly*/
    public void respawnWithLabyrinth()
    {
        //TODO: REPLACE WITH AN ACTION!
        toggleAllAbilities(true);

        rotatableObjectScript.returnToOriginalRotation();
        rotatableObjectScript.enabled = false;
        labyrinthMode = false;
        movingLabyrinth = false;

        inMotion = true;
        reportLabyrinthChangeInState();
        reportChangeInState(false, observer.ability);
        rbody.AddForce(Vector3.down, ForceMode.Impulse);
    }

    //used by any labyrinthAbility script, to set the current object of rotation.
    public void setRotatableObjects(GameObject courseContainer, GameObject abList)
    {
        rotatableObjectScript = courseContainer.GetComponent<LabyrinthRotate>();
        abilitiesList = abList.GetComponentsInChildren<AbilityPickup2>();
    }

    //TODO: REPLACE WITH AN ACTION!
    private void toggleAllAbilities(bool enable)
    {
        if (enable)
        {
            for (var i = 0; i < abilitiesList.Length; i++)
            {
                abilitiesList[i].enablePickup();
            }
        }
        else
        {
            for (var i = 0; i < abilitiesList.Length; i++)
            {
                abilitiesList[i].disablePickup();
            }
        }

    }

    //setting up shots is different in labyrinth mode, because not only can you take a normal shot, you can also
    //take a "labyrinth shot" by using your ability and rotating the map.
    void LabyrinthSetup()
    {
        //toggles Labyrinth mode if you have the labyrinth ability (to turning it OFF). next frame,
        //ShotSetup() called instead
        if (Input.GetKeyDown(KeyCode.F) && observer.ability.GetAbilityName() == "Labyrinth")
        {
            labyrinthMode = false;
            shotArrow.SetActive(true);
            reportLabyrinthChangeInState();
            reportChangeInState(false, observer.ability);
            Debug.Log("Labyrinth mode now " + labyrinthMode);
        }


        //when the player "shoots" the ball, just allow them to start rotating the labyrinth object.
        //and keep score!
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            scorecard.takeShot();


            //disable all abilities
            toggleAllAbilities(false);

            rotatableObjectScript.enabled = true;
            movingLabyrinth = true;
            reportLabyrinthChangeInState();
            reportChangeInState(true, observer.ability);
        }
    }


    //SETUP: for the next shot. activate the shot arrow, control its direction/power with WASD.
    void ShotSetup()
    {

        timeSinceLastShot = 0;
        //basic
        if (!shotArrow.activeInHierarchy)
        {
            shotArrow.SetActive(true);
        }

        rbody.velocity = Vector3.zero;
        rbody.angularVelocity = Vector3.zero;


        //modify shot arrow based on user input
        float rotSpeed = 0.2f; float moveSpeed = 0.01f;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rotSpeed *= 3.0f; moveSpeed *= 3.0f;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            rotSpeed /= 3.0f; moveSpeed /= 3.0f;
        }

        //Q and E angle up or downwards for a chip shot or space shot
        //chipShot: you can only go 50 degrees upwards. this is constant, bc going more than this with gravity on
        //would be stupid (we tried it)

        /*i confuse myself reading all this, so for reference, here's what the rotations mean:
         when A/D pressed, shotArrow rotates in the XZ plane, so it rotates about the Y axis.
         when Q/E pressed, shotArrow rotates in the YZ plane, so it rotates about the X axis.
         the Z axis is never used. if it were, it would rotate the arrow in orientations other than "flat, pointing
         in that direction over there". which isn't what we want. */
        if (observer.ability.GetAbilityName().Equals("Chipshot"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                if (chipRotation - rotSpeed >= 0.0f)
                {
                    shotArrow.transform.rotation = shotArrow.transform.rotation * Quaternion.Euler(Vector3.left * rotSpeed);
                    chipRotation -= rotSpeed;
                }
            }

            if (Input.GetKey(KeyCode.Q))
            {

                if (chipRotation + rotSpeed <= 50.0f)
                {
                    shotArrow.transform.rotation = shotArrow.transform.rotation * Quaternion.Euler(Vector3.right * rotSpeed);
                    chipRotation += rotSpeed;
                }
            }
        }
        //zero gravity is similar to chip shot in execution, but you can go anywhere in any direction.
        else if (observer.ability.GetAbilityName().Equals("ZeroGrav"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                shotArrow.transform.rotation = shotArrow.transform.rotation * Quaternion.Euler(Vector3.left * rotSpeed);
            }

            if (Input.GetKey(KeyCode.Q))
            {
                shotArrow.transform.rotation = shotArrow.transform.rotation * Quaternion.Euler(Vector3.right * rotSpeed);
            }
        }

        //A and D rotate the shot left/right
        if (Input.GetKey(KeyCode.A))
        {
            shotArrow.transform.RotateAround(shotArrow.transform.position, Vector3.down, rotSpeed);
        }

        if (Input.GetKey(KeyCode.D))
        {
            shotArrow.transform.RotateAround(shotArrow.transform.position, Vector3.up, rotSpeed);
        }

        if (currSpeed > maxSpeedFactor)
        {
            currSpeed = maxSpeedFactor;
            resizeShotArrow(currSpeed);
        }
        //W and S increase or decrease power
        if (Input.GetKey(KeyCode.W))
        {
            if (currSpeed + moveSpeed <= maxSpeedFactor)
            {
                currSpeed += moveSpeed;
                resizeShotArrow(currSpeed);
            }
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (currSpeed - moveSpeed >= minSpeedFactor)
            {
                currSpeed -= moveSpeed;
                resizeShotArrow(currSpeed);
            }
        }

        //toggles Labyrinth mode if you have the labyrinth ability (to turning it ON). next frame,
        //LabyrinthSetup() called instead
        if (Input.GetKeyDown(KeyCode.F) && observer.ability.GetAbilityName() == "Labyrinth")
        {
            labyrinthMode = true;
            shotArrow.SetActive(false);
            reportLabyrinthChangeInState();
            reportChangeInState(false, observer.ability);
            Debug.Log("Labyrinth mode now " + labyrinthMode);
        }

        //now for the actual vector we use to take our shot...
        //it's the direction of our shot (normalized for consistency) times the scalar of our shot speed.
        
        direction = shotArrow.transform.rotation * Vector3.back;
        direction = direction.normalized;
        speed = currSpeed * defaultSpeed;


        //when the player shoots the ball, add a force to the object with specified speed and direction.
        //and keep score!
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            scorecard.takeShot();

            if (!labyrinthMode)
            {
                sounds[0].Play();
                rbody.AddForce(direction * speed, ForceMode.Impulse);

            }
            else
            {
                movingLabyrinth = true;
                moveLabyrinth();
            }

            //send out a message to change the controls GUI, saying the ball is now in Motion. regardless of its ability,
            //this will work, as controlUI does the heavy lifting.
            reportChangeInState(true, observer.ability);

        }
    }

    //reports a change in state to the Controls UI (left side of screen)
    //more on this feature in that script.
    public void reportChangeInState(bool inMotion, Ability2 ability)
    {
        controlUI.resetControlGUI(inMotion, ability);
    }

    public void reportLabyrinthChangeInState()
    {
        controlUI.reportLabyrinthMode(labyrinthMode, movingLabyrinth);
    }

    //normal movement with no gimmicks. you can usually jump with Space, or slow the ball down with R
    void moveNormally()
    {
        shotArrow.SetActive(false);

        //do all of the following for a NORMAL shot (labyrinth mode disabled.)

        // Changes the height position of the player..
        if (!(observer.ability.GetAbilityName().Equals("Movement+") || observer.ability.GetAbilityName().Equals("ZeroGrav")))
        {
            if (Input.GetKeyDown(KeyCode.Space) && inContactWithGround > 0)
            {
                //Debug.Log("tried to jump");
                sounds[2].Play();
                rbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);

            }
        }

        if (Input.GetKeyDown(KeyCode.R) && inContactWithGround > 0)
        {
            //Debug.Log("slowed down");
            rbody.velocity = rbody.velocity * stoppingRate;
        }

        //movement+ allows you to put spin on the ball and perform FULL STOPs
        if (observer.ability.GetAbilityName().Equals("Movement+"))
        {
            timeSinceLastShot += Time.deltaTime;

            //FULL STOP
            if (Input.GetKeyDown(KeyCode.F) && inContactWithGround > 0)
            {
                //Debug.Log("FULL STOP!");
                rbody.velocity = rbody.velocity * 0.1f;
                rbody.angularVelocity = rbody.angularVelocity * 0.1f;
            }

            //put "spin" on the ball.
            //it's not actually angular motion, but small impulses are a good enough approximation.
            //the power of spin decreases with time, until "spinTime" is reached, when spin is 0
            Vector3 currVelocity = direction; //rbody.velocity;
            currVelocity.y = 0;

            Vector3 rightPerpVector = new Vector3(currVelocity.z, 0, -1 * currVelocity.x);
            float timeFactor = 0;
            if (timeSinceLastShot < spinTime)
            {
                timeFactor = (spinTime - timeSinceLastShot) / spinTime;
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                rbody.AddForce((-1 * rightPerpVector) * spinRate * timeFactor, ForceMode.Impulse);
            }

            if (Input.GetKeyDown(KeyCode.D))
            {
                rbody.AddForce(rightPerpVector * spinRate * timeFactor, ForceMode.Impulse);
            }

            //W goes forwards, in current direction of motion
            if (Input.GetKeyDown(KeyCode.W))
            {
                rbody.AddForce(currVelocity * spinRate * timeFactor, ForceMode.Impulse);
            }

            //...S goes backwards
            if (Input.GetKeyDown(KeyCode.S))
            {
                rbody.AddForce((-1 * currVelocity) * spinRate * timeFactor, ForceMode.Impulse);
            }

        }
    }

    //getters for methods that need them...
    public float getShotPower()
    {
        return currSpeed;
    }

    public float getShotAngle()
    {
        Vector3 XZdir = new Vector3(direction.x, 0, direction.z);
        return -1 * Vector3.SignedAngle(Vector3.forward, XZdir, Vector3.up);
    }

    public float getShotLoft()
    {
        //return Vector3.SignedAngle(Vector3.forward, direction, Vector3.left);
        Vector3 XZdir = new Vector3(direction.x, 0, direction.z);
        if(direction.y >= 0)
        {
            return Vector3.Angle(direction, XZdir);
        } else
        {
            return -1 * Vector3.Angle(direction, XZdir);
        }
        
    }

    public Vector3 getShotDirection()
    {
        return direction;
    }

    //now, detect when a ball is on the ground, so we know when some actions can be taken.
    void OnCollisionEnter(Collision collisionInfo)
    {
        //if we hit the ground, add to the number of ground objects we are hitting
        if (collisionInfo.gameObject.tag == "Ground")
        {

            inContactWithGround++;
        }
        else if (collisionInfo.gameObject.tag == "Curb" || collisionInfo.gameObject.tag == "Enemy"
        || collisionInfo.gameObject.tag == "Windmill")
        {
            sounds[1].Play();
        }

    }


    //detects when a ball leaves the ground.
    void OnCollisionExit(Collision collisionInfo)
    {
        //if we leave the ground, subtract from the number of ground objects we are hitting
        if (collisionInfo.gameObject.tag == "Ground")
        {

            inContactWithGround--;

        }

    }
}

//you made it this far?
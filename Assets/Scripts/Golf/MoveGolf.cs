using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGolf : MonoBehaviour
{
    //if not inMotion, set up and take your next shot. if in motion, allow the ball to move / jump / etc.
    //inProgress is a flag set to true the very frame the user makes their shot, false the frame it stops
    bool inMotion = true;
    float timeSlowed = 0;

    //controls our shot

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

        //TODO: eventually add a better mesh for the arrow and avoid all this nonsense.
        //expansion only works for the arrow head. i don't feel like updating the shaft too, since i'm replacing 
        //all of this anyway with a better mesh.
        //GameObject shotArrowShaft = shotArrow.transform.GetChild(0).gameObject;
        GameObject shotArrowHead = shotArrow.transform.GetChild(0).gameObject;
        shotArrowMF = shotArrowHead.GetComponent<MeshFilter>();
        shotArrowMesh = shotArrowMF.mesh;
        oldVerts = shotArrowMesh.vertices;
        shotArrowMR = shotArrowHead.GetComponent<MeshRenderer>();

        //set initial color of shotArrow
        shotArrowMR.material.SetColor("_Color", new Color(1.0f / maxSpeedFactor, 0, (maxSpeedFactor - 1.0f) / maxSpeedFactor, 1.0f));

        //find observer
        observer = GetComponent<AbObserver2>();

        //TODO: better global scoring system??
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
                        reportChangeInState(false, observer.ability);
                    }

                }
                else
                {
                    timeSlowed += Time.deltaTime;
                }
            }
            else
            {
                timeSlowed = 0.0f;
                inMotion = true;
            }
            //Debug.Log("Velocity: " + rbody.velocity.magnitude);

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
        //when we are actively in labyrinth mode, call the method there.
        else
        {
            moveLabyrinth();
        }


    }

    void moveLabyrinth()
    {
        if (Input.GetKey(KeyCode.A))
        {
            rbody.AddForce(Vector3.left * 0.01f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rbody.AddForce(Vector3.right * 0.01f);
        }
        if (Input.GetKey(KeyCode.W))
        {
            rbody.AddForce(Vector3.up * 0.01f);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rbody.AddForce(Vector3.back * 0.01f);
        }


        if (Input.GetKey(KeyCode.F))
        {
            toggleAllAbilities(true);

            labyrinthMode = false;
            movingLabyrinth = false;
            rotatableObjectScript.enabled = false;
        }
    }

    //call this method if you have to respawn and you're moving with labyrinth at the moment.
    //it's necessary to reset movement back to normal.
    public void respawnWithLabyrinth()
    {
        toggleAllAbilities(true);

        rotatableObjectScript.returnToOriginalRotation();
        rotatableObjectScript.enabled = false;
        labyrinthMode = false;
        movingLabyrinth = false;

        inMotion = true;
        rbody.AddForce(Vector3.down, ForceMode.Impulse);
    }

    public void setRotatableObjects(GameObject courseContainer, GameObject abList)
    {
        rotatableObjectScript = courseContainer.GetComponent<LabyrinthRotate>();
        abilitiesList = abList.GetComponentsInChildren<AbilityPickup2>();
    }

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

    void LabyrinthSetup()
    {
        //toggles Labyrinth mode if you have the labyrinth ability
        if (Input.GetKeyDown(KeyCode.F) && observer.ability.GetAbilityName() == "Labyrinth")
        {
            labyrinthMode = !labyrinthMode;
            shotArrow.SetActive(!labyrinthMode);
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
            moveLabyrinth();
        }
    }


    //SETUP: for the next shot. spawn the shot arrow, control its direction/power with WASD.
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
        float rotSpeed = 0.08f; float moveSpeed = 0.004f;
        if (Input.GetKey(KeyCode.LeftShift))
        {
            rotSpeed *= 3.0f; moveSpeed *= 3.0f;
        }
        else if (Input.GetKey(KeyCode.LeftControl))
        {
            rotSpeed /= 3.0f; moveSpeed /= 3.0f;
        }

        //Q and E angle up or downwards for a chip shot or space shot
        if (observer.ability.GetAbilityName().Equals("Chipshot"))
        {
            if (Input.GetKey(KeyCode.E))
            {
                //Debug.Log(shotArrow.transform.rotation);
                if (chipRotation - rotSpeed >= 0.0f)
                {
                    shotArrow.transform.rotation = shotArrow.transform.rotation * Quaternion.Euler(Vector3.left * rotSpeed);
                    chipRotation -= rotSpeed;
                }
            }

            if (Input.GetKey(KeyCode.Q))
            {
                //Debug.Log(shotArrow.transform.rotation);
                if (chipRotation + rotSpeed <= 50.0f)
                {
                    shotArrow.transform.rotation = shotArrow.transform.rotation * Quaternion.Euler(Vector3.right * rotSpeed);
                    chipRotation += rotSpeed;
                }
            }
        }
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

        //toggles Labyrinth mode if you have the labyrinth ability
        if (Input.GetKeyDown(KeyCode.F) && observer.ability.GetAbilityName() == "Labyrinth")
        {
            labyrinthMode = !labyrinthMode;
            shotArrow.SetActive(!labyrinthMode);
            Debug.Log("Labyrinth mode now " + labyrinthMode);
        }

        direction = direction.normalized;
        direction = shotArrow.transform.rotation * Vector3.back;
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

    public void reportChangeInState(bool inMotion, Ability2 ability)
    {
        controlUI.resetControlGUI(inMotion, ability);
    }

    void moveNormally()
    {
        shotArrow.SetActive(false);

        //do all of the following for a NORMAL shot (labyrinth mode disabled.)

        // Changes the height position of the player..
        if (Input.GetKeyDown(KeyCode.Space) && inContactWithGround > 0)
        {
            //Debug.Log("tried to jump");
            sounds[2].Play();
            rbody.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);

        }

        if (Input.GetKeyDown(KeyCode.R) && inContactWithGround > 0)
        {
            //Debug.Log("slowed down");
            rbody.velocity = rbody.velocity * stoppingRate;
        }


        if (observer.ability.GetAbilityName().Equals("Movement+"))
        {
            timeSinceLastShot += Time.deltaTime;

            //FULL STOP
            if (Input.GetKeyDown(KeyCode.F))
            {
                //Debug.Log("FULL STOP!");
                rbody.velocity = rbody.velocity * 0.1f;
                rbody.angularVelocity = rbody.angularVelocity * 0.1f;
            }

            //put angular motion, AKA "spin" on the ball
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
        // rbody.AddForce(Vector3.Reflect(direction, collisionInfo.contacts[0].normal) * rbody.velocity.magnitude,   
        //          ForceMode.Impulse);

    }



    void OnCollisionExit(Collision collisionInfo)
    {
        //if we leave the ground, subtract from the number of ground objects we are hitting
        if (collisionInfo.gameObject.tag == "Ground")
        {

            inContactWithGround--;

        }

    }
}

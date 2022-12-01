using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGolf : MonoBehaviour
{
    //if not inMotion, set up and take your next shot. if in motion, allow the ball to move / jump / etc.
    //inProgress is a flag set to true the very frame the user makes their shot, false the frame it stops
    bool inMotion = false;
    float timeSlowed = 0;

    //controls our shot
    public Vector3 direction;
    public float defaultSpeed = 0.1F;
    public float jumpHeight = 3.0f;
    public float gravityValue = 30f;
    public GameObject shotArrow;
    private float speed;
    private float maxSpeedFactor = 5.0f, minSpeedFactor = 0.2f;

    //controls physics
    private Vector3 playerVelocity;
    private Rigidbody rbody; //new

    //controls the arrow mesh, for when we change its appearance
    private MeshFilter shotArrowMF;
    private Mesh shotArrowMesh;
    private Vector3[] oldVerts;
    private MeshRenderer shotArrowMR;

    //keeps score
    private ScorecardScript scorecard;

    float currSpeed = 1.0f;
    [SerializeField]
    private int inContactWithGround = 0;
    
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

        //TODO: better global scoring system??
        scorecard = transform.parent.GetComponent<ScorecardScript>();
    }

    //expand or shrink the shot arrow, to represent a stronger or weaker shot
    void resizeShotArrow(float scale)
    {
        Vector3[] newVerts = shotArrowMesh.vertices;
        //don't need to modify triangles or normals for this simple operation.
        Color newcol = new Color(0, 0, 0);

        for (var i=0; i < newVerts.Length; i++)
        {
            newVerts[i].x = oldVerts[i].x * scale;

            newVerts[i].x -= (scale - 1.0f) * 5.0f;

            newcol = new Color(scale / maxSpeedFactor, 0, (maxSpeedFactor-scale) / maxSpeedFactor, 1.0f);
        }

        shotArrowMesh.vertices = newVerts;
        shotArrowMR.material.SetColor("_Color", newcol);
    }

    
    void Update()
    {
        /*for brevity's sake, "almost stopped moving" = not moving.
        We decide the shot is over when the ball has nearly stopped moving for at least 2 seconds.*/
        bool movingFast = (rbody.velocity.magnitude > 0.2f);
        if (!movingFast)
        {
            if (timeSlowed > 2.0f)
            {
                inMotion = false;
            } else
            {
                timeSlowed += Time.deltaTime;
            }
        } else
        {
            timeSlowed = 0.0f;
            inMotion = true;
        }
        //Debug.Log("Velocity: " + rbody.velocity.magnitude);

        //NOT IN MOTION: draw the arrow and line up your shot
        if(!inMotion)
        {
            //basic
            if(!shotArrow.activeInHierarchy)
            {
                shotArrow.SetActive(true);
            }
            rbody.velocity = Vector3.zero;


            //modify shot arrow based on user input
            float rotSpeed = 0.04f; float moveSpeed = 0.001f;
            if (Input.GetKey(KeyCode.LeftShift))
            {
                rotSpeed *= 3.0f; moveSpeed *= 3.0f;
            }

            //A and D rotate the shot left/right
            if (Input.GetKey(KeyCode.A))
            {
                shotArrow.transform.rotation = shotArrow.transform.rotation * Quaternion.Euler(Vector3.down * rotSpeed);
            }

            if (Input.GetKey(KeyCode.D))
            {
                shotArrow.transform.rotation = shotArrow.transform.rotation * Quaternion.Euler(Vector3.up * rotSpeed);
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

            direction = direction.normalized;
            direction = shotArrow.transform.rotation * Vector3.back;
            speed = currSpeed * defaultSpeed;
            //Debug.Log("Speed: " + currSpeed);


            //when the player shoots the ball, add a force to the object with specified speed and direction.
            //and keep score!
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            {
                rbody.AddForce(direction * speed, ForceMode.Impulse);
                scorecard.takeShot();
            }
        }


        //IN MOTION: the ball is now moving. allow for jumping
        else
        {
            shotArrow.SetActive(false);

            /*if (inContactWithGround > 0 && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
            }*/

            //TODO: groundedPlayer isn't working for the golf ball, it is always false.
            //Debug.Log(inContactWithGround);

            // Changes the height position of the player..
            if (Input.GetKeyDown(KeyCode.Space) && inContactWithGround > 0)
            {
                Debug.Log("tried to jump");
                rbody.AddForce(Vector3.up * speed, ForceMode.Impulse);
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

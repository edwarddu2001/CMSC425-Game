using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    //This object's rigidbody
    private Rigidbody rb;
    //Movement speed (is just a factor used in forces, not in units)
    public float speed = 15.0f;

    //tracks if we can jump and are grounded (usually the same, but some ablilities may change that)
    private bool canJump = false;
    private bool grounded = true;

    //A divider on aerial control, higher number = less responsive, 1 = as if on ground 
    public float airMoveFactor = 1.0f;

    //how fast you slow down when not inputting
    public float dragCoeff = 5f;

    //how hard the jump force is
    public float jumpSpeed = 15;

    //when this is 0 we are not able to jump
    private int inContactWithGround = 0;

    //used to see which movement buttons have been pushed since last fixedUpdate
    private bool[] queueWASDJ = {false, false, false, false, false};
    

    void Start()
    {
        //set rb
        rb = GetComponent<Rigidbody>();
    }

    
    void Update (){
        //queues the inputs for fixed update
        if (Input.GetKey(KeyCode.W)){
            queueWASDJ[0] = true;
        }
        if (Input.GetKey(KeyCode.A)){
            queueWASDJ[1] = true;
        }
        if (Input.GetKey(KeyCode.S)){
            queueWASDJ[2] = true;
        }
        if (Input.GetKey(KeyCode.D)){
            queueWASDJ[3] = true;
        }
        if (Input.GetKey(KeyCode.Space)){
            queueWASDJ[4] = true;
        }
    }

    void FixedUpdate()
    {
        //used to see if we should slow down
        bool movePressed = false;

        //if any of the movement keys were queued...
        if (queueWASDJ[1])
        {
            if (grounded) {
                rb.AddForce(new Vector3(-speed,0,0));
            } else {
                rb.AddForce(new Vector3(-speed/airMoveFactor,0,0));
            }

            movePressed = true;
                
        }
        if (queueWASDJ[3])
        {
            if (grounded) {
            rb.AddForce(new Vector3(speed,0,0));
            } else {
                rb.AddForce(new Vector3(speed/airMoveFactor,0,0));
            }
            movePressed = true;
                
        }
        if (queueWASDJ[2])
        {
            if (grounded){
                rb.AddForce(new Vector3(0,0,-speed));
            } else {
                rb.AddForce(new Vector3(0,0,-speed/airMoveFactor));
            }
            movePressed = true;
                
        }
        if (queueWASDJ[0])
        { 
            if(grounded) {
                rb.AddForce(new Vector3(0,0,speed));
            } else {
                rb.AddForce(new Vector3(0,0,speed/airMoveFactor));
            }
            movePressed = true;
                
        }
        if (queueWASDJ[4])
        {
            if (grounded) {
                rb.AddForce(Vector3.up * jump(),ForceMode.Impulse );
            }
               
        }
        //add drag if not moving
        if(!movePressed){
            rb.AddForce(dragXZ(rb.velocity, dragCoeff));
        }
        //reset the queue
        queueWASDJ = new bool[] {false, false, false, false, false};

    }

    //create the drag, don't affect y velocity
    private Vector3 dragXZ (Vector3 v, float drag){
        return new Vector3(-v.x*drag, 0, -v.z*drag);
    }

    float jump(){
        if (canJump){
            Debug.Log("jumping");
            canJump = false;
            grounded = false;
            return jumpSpeed;

            
        }
        Debug.Log("not grounded");
        return 0f;
    }

    void OnCollisionEnter(Collision collisionInfo)
    {   
        //if we hit the ground, add to the number of ground objects we are hitting
        if (collisionInfo.gameObject.tag == "Ground"){
            Debug.Log("bump");
            canJump = true;
            grounded = true;
            inContactWithGround ++;
        }
        
    }



    void OnCollisionExit(Collision collisionInfo)
    {
        //if we leave the ground, subtract from the number of ground objects we are hitting
        if (collisionInfo.gameObject.tag == "Ground"){
            Debug.Log("pmub");
            inContactWithGround --;
            if (inContactWithGround == 0){
                canJump = false;
                grounded = false;
            }
        }
        
    }
    
}
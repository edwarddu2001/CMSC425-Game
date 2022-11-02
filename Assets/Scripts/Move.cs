using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 15.0f;
    private bool canJump = false;
    private bool grounded = true;
    public float airMoveFactor = 1.0f;
    public float dragCoeff = 5f;
    public float jumpSpeed = 15;
    private int inContactWithGround = 0;
    private bool[] queueWASDJ = {false, false, false, false, false};
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update (){
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
        bool movePressed = false;
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
        if(!movePressed){
            rb.AddForce(dragXZ(rb.velocity, dragCoeff));
        }
        queueWASDJ = new bool[] {false, false, false, false, false};

    }

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
        if (collisionInfo.gameObject.tag == "Ground"){
            Debug.Log("bump");
            canJump = true;
            grounded = true;
            inContactWithGround ++;
        }
        
    }



    void OnCollisionExit(Collision collisionInfo)
    {
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
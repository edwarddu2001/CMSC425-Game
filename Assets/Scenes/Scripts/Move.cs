using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 5.0f;
    public GameObject ground;
    private bool canJump = false;
    private bool canMove = true;
    public float jumpSpeed = 30;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        bool movePressed = false;
        Vector3 vel = rb.velocity;
        
            if (Input.GetKey(KeyCode.A))
            {
                if (canMove) {
                    movePressed = true;
                    vel.x = -speed;
                }
                
            }
            if (Input.GetKey(KeyCode.D))
            {
                if (canMove) {
                    movePressed = true;
                    vel.x = speed;
                }
                
            }
            if (Input.GetKey(KeyCode.S))
            {
                if (canMove){
                    movePressed = true;
                    vel.z = -speed;
                }
                
            }
            if (Input.GetKey(KeyCode.W))
            {
                if(canMove) {
                    movePressed = true;
                    vel.z = speed;
                }
                
            }
            if (Input.GetButtonDown("Jump"))
            {
                if (canMove) {
                    //vel.y += jump();
                    rb.AddForce(Vector3.up * jump(),ForceMode.Impulse );
                }
               
            }
        if (movePressed == true){
            vel = Vector3.Normalize(vel - new Vector3(0,vel.y,0)) ;
            vel = vel*speed + new Vector3(0,vel.y,0);
        }

        rb.velocity = vel;
    }

    float jump(){
        if (canJump){
            Debug.Log("jumping");
            canJump = false;
            canMove = false;
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
            canMove = true;
        }
        
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        if (collisionInfo.gameObject.tag == "Ground"){
            Debug.Log("pmub");
            canJump = false;
            canMove = false;
        }
        
    }
    
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    private Rigidbody rb;
    public float speed = 5.0f;
    public GameObject ground;
    private bool canJump = false;
    public float jumpSpeed = 10;
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
                movePressed = true;
                vel.x = -speed;
            }
            if (Input.GetKey(KeyCode.D))
            {
                movePressed = true;
                vel.x = speed;
            }
            if (Input.GetKey(KeyCode.S))
            {
                movePressed = true;
                vel.z = -speed;
            }
            if (Input.GetKey(KeyCode.W))
            {
                movePressed = true;
                vel.z = speed;
            }
            if (Input.GetKey(KeyCode.E))
            {
                vel.y += jump();
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
            return jumpSpeed;

            
        }
        Debug.Log("not grounded");
        return 0f;
    }

    void OnCollisionEnter(Collision collisionInfo)
    {   
        if (collisionInfo.gameObject == ground){
            Debug.Log("bump");
            canJump = true;
        }
    }

    void OnCollisionExit(Collision collisionInfo)
    {
        if (collisionInfo.gameObject == ground){
            Debug.Log("pmub");
            canJump = false;
        }
    }
    
}

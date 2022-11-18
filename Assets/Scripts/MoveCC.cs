using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MoveCC : MonoBehaviour
{
    
    
    public float speed = 10.0F;
    public float jumpHeight = 1.0f;
    public float gravityValue = -9.81f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Camera mainCamera; 
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        mainCamera = Camera.main;    
    }

    // Update is called once per frame
    void Update()
    {
    
    
        Vector3 move = new Vector3(0,0,0);
        if (Input.GetKey(KeyCode.W)){
            move = move + Vector3.forward;
        }
        if (Input.GetKey(KeyCode.A)){
            move = move + Vector3.left;
        }
        if (Input.GetKey(KeyCode.S)){
            move = move + Vector3.back;
        }
        if (Input.GetKey(KeyCode.D)){
            move = move + Vector3.right;
        }
        
        move = move.normalized;
        
        //temporarily rotate the camera to that it is parallel to the ground for calculating how to move
        //not sure how this will go if the camera is on the ceiling 
        float cameraRotX = mainCamera.transform.eulerAngles.x;
        mainCamera.transform.eulerAngles = mainCamera.transform.eulerAngles - new Vector3(cameraRotX,0,0); 
        move = mainCamera.transform.TransformDirection(move);
        mainCamera.transform.eulerAngles = mainCamera.transform.eulerAngles + new Vector3(cameraRotX,0,0);
        
        move = speed * move;
        
        bool groundedPlayer = controller.isGrounded;
        
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        controller.Move(move);

        // Changes the height position of the player..
        if (Input.GetKey(KeyCode.Space) && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

    }
}

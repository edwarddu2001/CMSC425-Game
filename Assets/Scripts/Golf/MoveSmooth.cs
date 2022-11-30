using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MoveSmooth : MonoBehaviour
{


    public float speed = 0.03F;
    public float jumpHeight = 3.0f;
    public float gravityValue = 30f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    private Camera mainCamera;

    private Rigidbody rbody; //new
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        rbody = GetComponent<Rigidbody>();
        mainCamera = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {



        Vector3 move = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W))
        {
            //move = move + Vector3.forward;
            rbody.AddForce(Vector3.forward * 3);
        }
        if (Input.GetKey(KeyCode.A))
        {
            //move = move + Vector3.left;
            rbody.AddForce(Vector3.left * 3);
        }
        if (Input.GetKey(KeyCode.S))
        {
            //move = move + Vector3.back;
            rbody.AddForce(Vector3.back * 3);
        }
        if (Input.GetKey(KeyCode.D))
        {
            //move = move + Vector3.right;
            rbody.AddForce(Vector3.right * 3);
        }

        move = move.normalized;

        //temporarily rotate the camera to that it is parallel to the ground for calculating how to move
        //not sure how this will go if the camera is on the ceiling 

        move = mainCamera.transform.TransformDirection(move);

        move = speed * move;

        bool groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        //controller.Move(move);

        // Changes the height position of the player..
        if (Input.GetKey(KeyCode.Space) && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

    }

}

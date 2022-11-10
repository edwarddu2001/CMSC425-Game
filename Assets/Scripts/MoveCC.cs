using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MoveCC : MonoBehaviour
{
    
    
    public float speed = 3.0F;
    private CharacterController controller;
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
        if (Input.GetKey(KeyCode.Space)){
            Debug.Log("Jump");
        }
        move = move.normalized;
        move = mainCamera.transform.TransformDirection(move);
        controller.SimpleMove(speed * move);
    }
}

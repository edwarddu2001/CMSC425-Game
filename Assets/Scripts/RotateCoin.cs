using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCoin : MonoBehaviour

{
    private Ability ability;
    public float rotationSpeed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        ability = GetComponent<Ability>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, rotationSpeed, 0) *  Time.deltaTime);
    }

   
}

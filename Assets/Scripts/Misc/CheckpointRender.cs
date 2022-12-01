using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointRender : MonoBehaviour
{
    private MeshRenderer check, checkSpawn;
    // Start is called before the first frame update
    void Start()
    {
        check = GetComponent<MeshRenderer>();
        checkSpawn = transform.GetChild(0).GetComponent<MeshRenderer>();

        check.enabled = false;
        checkSpawn.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

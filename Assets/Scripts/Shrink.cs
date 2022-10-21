using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrink : MonoBehaviour
{

    [SerializeField]
    private GameObject obj;
    [SerializeField]
    private bool isShrunk = false;

    Mesh mesh;

    // Update is called once per frame
    private void OnTriggerEnter(Collider other) {
        if(!isShrunk){
            obj.transform.localScale = new Vector3(.8f,.8f,.8f);
            isShrunk = true;
        }
    }

}


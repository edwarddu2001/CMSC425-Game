using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundingBox : MonoBehaviour
{
    MeshRenderer[] comps;
    // Start is called before the first frame update
    void Start()
    {
        comps = transform.GetComponentsInChildren<MeshRenderer>();

        for(var i = 0; i < comps.Length; i++)
        {
            comps[i].enabled = false;
        }
    }


}

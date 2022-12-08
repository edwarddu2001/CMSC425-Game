using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZachDoorway : MonoBehaviour
{

    void OnTriggerEnter(){
        gameObject.transform.parent.gameObject.GetComponent<ZachWall>().SpawnNeighbors();
    }
}

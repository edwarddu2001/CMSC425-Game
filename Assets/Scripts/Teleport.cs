using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Teleport : MonoBehaviour
{
    public Transform target; 
    public GameObject player;
    public bool teleported;

    void OnTriggerEnter(Collider other) {

        if (teleported) {
            // nothing
        }
        else {
            StartCoroutine(TeleportCoRoutine(player));
        }
    }

    void OnTriggerExit(Collider c)
    {
        teleported = false;
    }

    IEnumerator TeleportCoRoutine(GameObject playerObj)
    {
        yield return new WaitForSeconds(3f);
        teleported = true;
        playerObj.transform.position = target.position;
    
    }
}

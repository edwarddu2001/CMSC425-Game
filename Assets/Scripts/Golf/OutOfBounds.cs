using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script handles going out of bounds and respawning, either at the start or at a checkpoint
public class OutOfBounds : MonoBehaviour
{

    public GameObject BoundingBox;
    public GameObject respawnPoint;
    public GameObject[] checkpoints;

    private int currCheckpoint;
    private Rigidbody rbody;
    private Vector3[] checkpointSpawns;

    private ScorecardScript scorecard;

    public void Start()
    {
        currCheckpoint = -1;
        scorecard = transform.parent.GetComponent<ScorecardScript>();
        rbody = GetComponent<Rigidbody>();

        //set checkpoint spawns
        checkpointSpawns = new Vector3[checkpoints.Length];
        for(var i=0; i < checkpoints.Length; i++)
        {
            checkpointSpawns[i] = checkpoints[i].transform.GetChild(0).transform.position;
        }
    }

    private void Respawn()
    {
        rbody.velocity = Vector3.zero;
        rbody.angularVelocity = Vector3.zero;

        //respawn penalty!
        scorecard.takeShot();
        
        //respawn at the start, or at the furthest reached checkpoint
        Vector3 teleTo = respawnPoint.transform.position;
        if (currCheckpoint != -1)
        {
            teleTo = checkpointSpawns[currCheckpoint];
        }
        teleTo.y += 2;
        transform.position = teleTo;
    }

    private void OnTriggerEnter(Collider collision)
    {
        bool isRespawning = false;

        //two collisions this script will handle: going out of bounds and respawning, or reaching a checkpoint.
        //case 1: collided with the boundary box (went out of bounds), respawn.
        BoxCollider[] bounds = BoundingBox.transform.GetComponentsInChildren<BoxCollider>();
        for(var i=0; i<bounds.Length; i++)
        {
            if(bounds[i].Equals(collision))
            {
                Respawn();
                isRespawning = true;
                break;
            }
        }

        //case 2: hit a checkpoint, is it further along the hole? if so, set the new variable.
        if (!isRespawning)
        {
            for(var i=0; i<checkpoints.Length; i++)
            {
                if (collision.Equals(checkpoints[i].GetComponent<BoxCollider>()))
                {
                    if (i > currCheckpoint)
                    {
                        currCheckpoint = i;
                    }

                }
            }
        }
    }
}

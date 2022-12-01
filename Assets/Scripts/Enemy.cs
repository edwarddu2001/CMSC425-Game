using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    private NavMeshAgent navAgent;

    [SerializeField]
    private Transform[] destinationPoints;

    private Vector3 currentDestination;

    private int destinationIndex;

    private bool moveToPlayer;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        CheckReachedDestination();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            moveToPlayer = true;
            navAgent.SetDestination(other.transform.position);
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            navAgent.SetDestination(other.transform.position);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            moveToPlayer = false;
        }
    }

    //If the player makes contact, reset the scene
    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }   
    }

    void CheckReachedDestination()
    {
        // Check if we've reached the destination
        if (!navAgent.pathPending)
        {
            if (navAgent.remainingDistance <= navAgent.stoppingDistance)
            {
                if (!navAgent.hasPath || navAgent.velocity.sqrMagnitude == 0f)
                {
                    if (!moveToPlayer)
                    {
                        SetNewDestination();
                    }
                }
            }
        }
    }

    void SetNewDestination()
    {
        if (destinationIndex >= destinationPoints.Length)
        {
            destinationIndex = 0;
        }

        currentDestination = destinationPoints[destinationIndex++].position;
        navAgent.SetDestination(currentDestination);
    }
}

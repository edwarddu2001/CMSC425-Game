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
    [SerializeField]
    private InvisibleAbility invisibleAbility;

    private Vector3 currentDestination;
    private int destinationIndex;
    private bool playerInvisible;
    private bool moveToPlayer;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();

        if (invisibleAbility != null)
        {
            invisibleAbility.ReportInvisible += PlayerInvisible;
        }
    }

    void Update()
    {
        CheckReachedDestination();
    }

    void PlayerInvisible(bool invisible)
    {
        playerInvisible = invisible;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !playerInvisible)
        {
            moveToPlayer = true;
            navAgent.SetDestination(other.transform.position);
        }

        if (other.CompareTag("Player") && playerInvisible)
        {
            moveToPlayer = false;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !playerInvisible)
        {
            navAgent.SetDestination(other.transform.position);
        }

        if (other.CompareTag("Player") && playerInvisible)
        {
            moveToPlayer = false;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            moveToPlayer = false;
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

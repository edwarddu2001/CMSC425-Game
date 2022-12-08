using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZachWall : MonoBehaviour
{
    [SerializeField] private ZachDoorway doorway;
    [SerializeField] private Vector3 SignPos;
    [SerializeField] private float signHeight = .3f;
    [SerializeField] private float signSpace = 0f;
    [SerializeField] private GameObject[] SignPrefabs;
    [SerializeField] private int wallNum;

    public enum Sign {Goal, Bulldozer, Lightup, Back};

    public Sign pathTo {
        get; set;
    }

    void PlaceSign(Sign sign){
        Instantiate(SignPrefabs[(int) pathTo], SignPos + new Vector3(0, (int) pathTo * (signHeight + signSpace)), Quaternion.identity);
    }

    public void SpawnNeighbors(){
        Debug.Log("In room " + gameObject.transform.parent.gameObject.GetComponent<ZachRoomScript>().id + ", at wall " + wallNum);
        gameObject.transform.parent.gameObject.GetComponent<ZachRoomScript>().SpawnNeighbors(wallNum);
    }
}

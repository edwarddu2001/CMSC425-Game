using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZachWall : MonoBehaviour
{
    [SerializeField] private ZachDoorway doorway;
    [SerializeField] private Vector3 SignPos;
    //[SerializeField] private float signBuffer = 0f;
    [SerializeField] private GameObject[] SignPrefabs;
    [SerializeField] private int wallNum;

    public void PlaceSign(MazeCreation.SpecialRoom sign){
        Debug.Log("Placing " + (sign - 1) + " on wall " + wallNum);
        GameObject newSign = Instantiate(SignPrefabs[(int) (sign - 1)], transform.GetChild((int)sign + 4));
        newSign.transform.localScale = new Vector3(.2f,.2f,.2f);
        newSign.transform.rotation *= Quaternion.Euler(0,0,90);
        
    }

    public void SpawnNeighbors(){
        Debug.Log("In room " + gameObject.transform.parent.gameObject.GetComponent<ZachRoomScript>().id + ", at wall " + wallNum);
        gameObject.transform.parent.gameObject.GetComponent<ZachRoomScript>().SpawnNeighbors(wallNum);
    }
}

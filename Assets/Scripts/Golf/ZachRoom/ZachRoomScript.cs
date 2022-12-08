using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZachRoomScript : MonoBehaviour
{
    public MazeCreation Spawner {get; set;}
    public GameObject roomPrefab; 
    public GameObject holePrefab;
    public GameObject player;
    public int id = -1; 
    public GameObject[] neighbors; 
    
    void Awake(){
        neighbors = new GameObject[] {null, null, null, null};
    }

    void Start(){
        Spawner = (MazeCreation)FindObjectOfType(typeof(MazeCreation));
    }
    
    public void PrintId(){
        Debug.Log("Room " + id + " created!");
    }

    /*
            \O
            |
        \O - O - \O
            |
        O - O - O
            |
            O
    */

    public void SpawnNeighbors(int wallNum){
        //unload neighbor's neighbors
        for(int myNeighbors = 0; myNeighbors < 4; myNeighbors++){
            if (myNeighbors != wallNum){
                for(int theirNeighbors = 0; theirNeighbors < 4; theirNeighbors++){
                    if(neighbors[myNeighbors] != null && neighbors[myNeighbors].GetComponent<ZachRoomScript>().neighbors[theirNeighbors] != null && neighbors[myNeighbors].GetComponent<ZachRoomScript>().neighbors[theirNeighbors].GetComponent<ZachRoomScript>().id != id){
                        //Debug.Log("Destroying" + myNeighbors + "," + theirNeighbors );
                        Destroy(neighbors[myNeighbors].GetComponent<ZachRoomScript>().neighbors[theirNeighbors]);
                    }
                }
            }
        }
        
        //load newneighbor's neighbors
        Spawner.SpawnNeighbors(transform.position, transform.rotation, this, wallNum);
    }

    public void SpawnHole(GameObject ballCam, GameObject holeGUI, GameObject scorecardGUI){
        GameObject holeObject = Instantiate(holePrefab, transform);
        ZachHole hole = holeObject.transform.GetComponentInChildren<ZachHole>();
            Debug.Log(hole.ballCam);
            hole.ballCam = ballCam;
            hole.holeGUI = holeGUI;
            hole.scorecardGUI = scorecardGUI;
            hole.player = player;
    }
}

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
        SpawnSigns();
    }
    
    public void PrintId(){
        Debug.Log("Room " + id + " created!");
    }

    //TODO:
    //find the dirto for each sign and put it above the correct door
    private void SpawnSigns(){
        for(int sign = 0; sign < 4; sign ++){
            Debug.Log("id: " + id + " sign: " + sign);
            transform.GetChild(Spawner.dirTo((MazeCreation.SpecialRoom) (sign + 1), id)+1).GetComponent<ZachWall>().PlaceSign((MazeCreation.SpecialRoom) sign + 1);
        }
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
            hole.floor = transform.GetChild(0).gameObject;
    }
}

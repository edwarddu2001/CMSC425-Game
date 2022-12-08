using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class MazeCreation : MonoBehaviour
{
    public GameObject roomPrefab;
    public GameObject deadEndPrefab; 
    private Node root; 
    [SerializeField]
    public int seed = 0;
    [SerializeField]
    public const int numNeighbors = 4;
    [SerializeField]
    public const int maxIter = 2;
    [SerializeField]
    private Transform player;
    [SerializeField]
    private int roomIn = 0;
    private ArrayList rooms;
    private ArrayList spawnedRooms;
    
    void Start()
    {
        transform.position = player.position;
        rooms = new ArrayList();
        spawnedRooms = new ArrayList();

        Random.InitState(seed);
        Generate(maxIter);

        GameObject rootRoom = Instantiate(
                        roomPrefab, player.position, Quaternion.identity);
        rootRoom.GetComponent<ZachRoomScript>().id = 0;
        rootRoom.GetComponent<ZachRoomScript>().PrintId();
        rootRoom.name = "Room 0";
        for(int i = 0; i<4; i++){
            GameObject newRoom = Instantiate(
                        roomPrefab, new Vector3(7*Mathf.Cos(Mathf.PI/2*(i-1)),0,7*Mathf.Sin(Mathf.PI/2*(i-1))) + player.position, Quaternion.Euler(0,90*(-i+1),0));
            rootRoom.GetComponent<ZachRoomScript>().neighbors[i] = newRoom;
            newRoom.GetComponent<ZachRoomScript>().id = i+1;
            newRoom.GetComponent<ZachRoomScript>().neighbors[3] = rootRoom;
            newRoom.GetComponent<ZachRoomScript>().PrintId();
            newRoom.name = "Room " + newRoom.GetComponent<ZachRoomScript>().id;
        }

        Print();
        Debug.Log(rooms.Count + " total rooms");
    }

    public void Print(){
        string toReturn = "";
        foreach(Node n in rooms){
            toReturn += n.toString(); 
        }
        Debug.Log(toReturn);
    }

    public void Generate(int levels){
        root = new Node(false, 0);
        ArrayList unfilled = new ArrayList();
        ArrayList nextUnfilled = new ArrayList();
        ArrayList waiting = new ArrayList();
        unfilled.Add(root);
        rooms.Add(root);
        do{
            //for each unfilled node
            foreach(Node node in unfilled){
                //for each null neighbor
                for(int i = 0; i < numNeighbors; i++){
                    if(node.neighbors[i] == null || node.neighbors[i].id == -1){
                        NodeStatus r = (NodeStatus)Random.Range(0,3);
                        Debug.Log(node.id + "[" + i + "], status=" + r);
                        //if we are making a new node, make it.
                        if (r == NodeStatus.Waiting || r == NodeStatus.New || waiting.Count == 0) {
                            //if we still have recursion depth
                            if(node.level < maxIter){
                                //new node, make exit work
                                node.neighbors[i] = new Node((int)r != 1, node.level+1);
                                node.neighbors[i].neighbors[numNeighbors-1] = node;  
                                rooms.Add(node.neighbors[i]);
                                //if we aren't waiting, add it to queue, else add it to wait queue
                                if(r != NodeStatus.Waiting) {
                                    nextUnfilled.Add(node.neighbors[i]);
                                } else {
                                    waiting.Add(node.neighbors[i]);
                                }
                            } else {
                                //with no recrsion depth, make a dead end.
                                node.neighbors[i] = Node.DeadEnd(node);
                                rooms.Add(node.neighbors[i]);
                            }
                        } else {
                            //if we are connecting to an old room
                            int n = Random.Range(0,waiting.Count);
                            //connect to the room
                            node.neighbors[i] = (Node)waiting[n];
                            
                            
                            //set the way back
                            int dir = Random.Range(0,numNeighbors-1);
                            int goTo = -1; 
                            for(int m = 0; m < numNeighbors; m++ ){
                                if(node.neighbors[i].neighbors[n].id != -1){
                                    m++;
                                } else if(dir >= 0) {
                                    goTo++;
                                    dir--;
                                }
                            }
                            node.neighbors[i].neighbors[goTo] = node;
                            //add the connection to the queue
                            nextUnfilled.Add(waiting[n]);
                            ((Node)waiting[n]).wait = false;
                            //take the previously waiting room out of the waiting queue
                            waiting.RemoveAt(n);
                        }
                    }
                }
            }
            //reset the arraylists for next iteration
            unfilled = nextUnfilled;
            nextUnfilled = new ArrayList();
        } while(unfilled.Count > 0);
        
        foreach(Node n in waiting){
            for(int i = 0; i < numNeighbors; i++){
                if(n.neighbors[i] == null){
                    n.neighbors[i] = Node.None;
                } 
            }
            rooms.Add(n);
        }
    }

    private Node roomNum(int roomNum){
        if (roomNum < 0 || roomNum >= rooms.Count){
            return Node.None;
        } 
        return (Node)rooms[roomNum]; 
    }

    public void SpawnNeighbors(Vector3 position, Quaternion rotation, ZachRoomScript roomRef, int wallNum){
        Node nodeEntering = roomNum(roomRef.id).neighbors[wallNum];
        
        ZachRoomScript roomEntering = roomRef.neighbors[wallNum].GetComponent<ZachRoomScript>();
        
        //instantiate new inroom neighbors other than oldroom
        for(int i = 0; i < 4; i++){
            if (roomEntering.neighbors[i] == null){
                int rot = (int)((((Vector3.SignedAngle(roomEntering.gameObject.transform.forward, Vector3.forward, Vector3.up)))/90) -i + 0.01f )% numNeighbors;
                if (nodeEntering.neighbors[i].id < 0 || nodeEntering.neighbors[i].id > rooms.Count){
                    //spawn dead end
                    GameObject newRoom = Instantiate(
                        deadEndPrefab, new Vector3(7*Mathf.Cos(Mathf.PI/2*(rot)),0,7*Mathf.Sin(Mathf.PI/2*(rot))) + roomEntering.gameObject.transform.position, Quaternion.Euler(0,90*(-i+2),0));
        
                } else if (roomEntering.id != roomIn){
                    GameObject newRoom = Instantiate(
                        roomPrefab, new Vector3(7*Mathf.Cos(Mathf.PI/2*(rot)),0,7*Mathf.Sin(Mathf.PI/2*(rot))) + roomEntering.gameObject.transform.position, Quaternion.Euler(0,90*(roomNum(roomEntering.id).neighbors[i].NeighborNumber(roomEntering.id)-i-1),0));
                    roomEntering.GetComponent<ZachRoomScript>().neighbors[i] = newRoom;
                    newRoom.GetComponent<ZachRoomScript>().id = roomNum(roomEntering.id).neighbors[i].id;
                    Debug.Log("newRoom id: " + newRoom.GetComponent<ZachRoomScript>().id + " roomEntering id: " + roomEntering.id + " newneighborNumber: " + roomNum(newRoom.GetComponent<ZachRoomScript>().id).NeighborNumber(roomEntering.id));
                    newRoom.GetComponent<ZachRoomScript>().neighbors[roomNum(newRoom.GetComponent<ZachRoomScript>().id).NeighborNumber(roomEntering.id)] = roomEntering.gameObject;
                    newRoom.GetComponent<ZachRoomScript>().PrintId();
                    newRoom.name = "Room " + newRoom.GetComponent<ZachRoomScript>().id;
                }
            }
        }
        roomIn = roomRef.neighbors[wallNum].GetComponent<ZachRoomScript>().id;
        
    }
    
    enum NodeStatus {Waiting, New, Connecting};
    public class Node{
        public bool wait;
        //left, center, right, back
        public Node[] neighbors;
        public int level;
        public int id;
        public static int idCount = 0;
        public static Node None = new Node(-1);
        
        public Node(bool waitIn, int levelIn){
            neighbors = new Node[numNeighbors];
            wait = waitIn;
            level = levelIn;
            for(int i = 0; i < neighbors.Length; i++){
                neighbors[i] = None;
            }
            id = idCount++;
        }

        private Node(int idIn){
            neighbors = new Node[numNeighbors];
            id = idIn;
        }

        public static Node DeadEnd(Node exit){
            Node n = new Node(false, exit.level + 1);
            n.neighbors[numNeighbors-1] = exit;
            return n;
        }

        public int NeighborNumber (int id){
            for (int i = 0; i < numNeighbors; i++){
                if (neighbors[i].id == id){
                    return i;
                }
            }
            return -1;
        }

        public string toString(){
            string toReturn = "";
            toReturn += id  + ": Level = " + level + ", neighbors = {";
            foreach (Node neighbor in neighbors){
                toReturn += neighbor.id + " ";
            }
            toReturn += "}\n";  
            return toReturn;
        }
    }
}

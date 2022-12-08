using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MazeCreation : MonoBehaviour
{
    public GameObject ballCam;
    public GameObject holeGUI;
    public GameObject scorecardGUI;
    
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

    enum NodeStatus {Waiting, New, Connecting};
    public enum SpecialRoom {Not, Tee, Goal, Bulldozer, LightUp};
    
    void Start()
    {
        transform.position = player.position;
        rooms = new ArrayList();
        spawnedRooms = new ArrayList();

        UnityEngine.Random.InitState(seed);
        Generate(maxIter);

        GameObject rootRoom = Instantiate(
                        roomPrefab, player.position, Quaternion.identity);
        rootRoom.GetComponent<ZachRoomScript>().id = 0;
        rootRoom.GetComponent<ZachRoomScript>().PrintId();
        rootRoom.GetComponent<ZachRoomScript>().player = player.gameObject;
        rootRoom.name = "Room 0";
        for(int i = 0; i<4; i++){
            GameObject newRoom = Instantiate(
                        roomPrefab, new Vector3(7*Mathf.Cos(Mathf.PI/2*(i-1)),0,7*Mathf.Sin(Mathf.PI/2*(i-1))) + player.position, Quaternion.Euler(0,90*(-i+1),0));
            rootRoom.GetComponent<ZachRoomScript>().neighbors[i] = newRoom;
            newRoom.GetComponent<ZachRoomScript>().id = i+1;
            newRoom.GetComponent<ZachRoomScript>().neighbors[3] = rootRoom;
            newRoom.GetComponent<ZachRoomScript>().PrintId();
            newRoom.name = "Room " + newRoom.GetComponent<ZachRoomScript>().id;
            newRoom.GetComponent<ZachRoomScript>().player = player.gameObject;
        }

        Print();
        Debug.Log(rooms.Count + " total rooms");
        Debug.Log(dirTo(SpecialRoom.Tee, 6));
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
        root.roomType = SpecialRoom.Tee;
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
                        NodeStatus r = (NodeStatus)UnityEngine.Random.Range(0,3);
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
                                if(r != NodeStatus.Waiting || node.id == 0) {
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
                            int n = UnityEngine.Random.Range(0,waiting.Count);
                            //connect to the room
                            node.neighbors[i] = (Node)waiting[n];
                            
                            
                            //set the way back
                            int dir = UnityEngine.Random.Range(0,numNeighbors-1);
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

        foreach (SpecialRoom sr in (SpecialRoom[]) Enum.GetValues(typeof(SpecialRoom))){
            if(sr != SpecialRoom.Not && sr != SpecialRoom.Tee){
                for (int i = UnityEngine.Random.Range(1,rooms.Count); true; i = UnityEngine.Random.Range(1,rooms.Count)){
                    if(((Node)rooms[i]).roomType == SpecialRoom.Not) {
                        ((Node) rooms[i]).roomType = sr;
                        Debug.Log("Room " + i + " has the " + sr);
                        break;
                    }
                }
            }
        }

        foreach (Node n in rooms){
            for(int i = 0; i < 4; i++){
                Debug.Log(n.id + ": " + (SpecialRoom) (i+1) + "- " +  dirTo((SpecialRoom) i+1, n.id));
                n.dirTo[i] = dirTo((SpecialRoom) (i+1), n.id);
            }
        }
    }

    private int distFrom(SpecialRoom sr, int roomNum){
        ArrayList traversed = new ArrayList();
        ArrayList queue = new ArrayList();
        int depth = 0;
        traversed.Add(roomNum);
        if (roomNum != -1){
            queue.Add(rooms[roomNum]);
        }
        while(true){
            ArrayList queueNew = new ArrayList();
            foreach(Node n in queue){
                if (n.roomType == sr){
                    return depth;
                }
                foreach(Node neighbor in n.neighbors){
                    if(neighbor != null && !traversed.Contains(neighbor.id)){
                        queueNew.Add(neighbor);
                        traversed.Add(neighbor.id);
                    }
                }
            }
            queue = queueNew;
            if(queue.Count == 0){
                return int.MaxValue;
            }
            depth++;
        }

    }

    public int dirTo(SpecialRoom sr, int roomNum){
        int dir = -1;
        int minDist = int.MaxValue;
        for (int i = 0; i < numNeighbors; i++){
            if (((Node)rooms[roomNum]).neighbors[i] != null && distFrom(sr, ((Node)rooms[roomNum]).neighbors[i].id) < minDist){
                minDist = distFrom(sr, ((Node)rooms[roomNum]).neighbors[i].id);
                dir = i;
            }
        }
        return dir;
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
        Vector3 enteringForward = roomEntering.gameObject.transform.forward;
        
        //instantiate new inroom neighbors other than oldroom
        for(int i = 0; i < 4; i++){
            if (roomEntering.neighbors[i] == null){
                //what neightbor # is
                int rot = Mathf.FloorToInt((((-Vector3.SignedAngle(enteringForward, Vector3.forward, Vector3.up)))/90) - i + .001f);// % numNeighbors;
                Debug.Log("i:" + i +" rot:" + rot);
                if(nodeEntering.neighbors[i].neighbors[0] != null){
                    if (nodeEntering.neighbors[i].neighbors[0].id == -1){
                        //spawn dead end
                        GameObject newRoom = Instantiate(
                            deadEndPrefab, new Vector3(7*Mathf.Sin(Mathf.PI/2*(-rot)),0,-7*Mathf.Cos(Mathf.PI/2*(rot))) + roomEntering.gameObject.transform.position, roomEntering.gameObject.transform.rotation*Quaternion.Euler(0,90*(2*((i+1)%2)+3+i),0));
                            for(int n = 0; n < 3; n++){
                                newRoom.GetComponent<ZachRoomScript>().neighbors[n] = null;
                            }
                            roomEntering.GetComponent<ZachRoomScript>().neighbors[i] = newRoom;
                            newRoom.GetComponent<ZachRoomScript>().neighbors[3] = roomEntering.gameObject;
                            newRoom.GetComponent<ZachRoomScript>().id = roomNum(roomEntering.id).neighbors[i].id;
                            newRoom.GetComponent<ZachRoomScript>().player = player.gameObject;
                            newRoom.name = "DeadEnd " + newRoom.GetComponent<ZachRoomScript>().id;
                            if(((Node) rooms[newRoom.GetComponent<ZachRoomScript>().id]).roomType == SpecialRoom.Goal){
                                newRoom.GetComponent<ZachRoomScript>().SpawnHole(ballCam, holeGUI, scorecardGUI);
                            }
            
                    } else if (roomEntering.id != roomIn){
                        GameObject newRoom = Instantiate(
                            roomPrefab, new Vector3(7*Mathf.Sin(Mathf.PI/2*(-rot)),0,-7*Mathf.Cos(Mathf.PI/2*(rot))) + roomEntering.gameObject.transform.position, roomEntering.gameObject.transform.rotation*Quaternion.Euler(0,90*(2*((i+1)%2)+(roomNum(roomEntering.id).neighbors[i].NeighborNumber(roomEntering.id))+i),0));
                        roomEntering.GetComponent<ZachRoomScript>().neighbors[i] = newRoom;
                        newRoom.GetComponent<ZachRoomScript>().id = roomNum(roomEntering.id).neighbors[i].id;
                        Debug.Log("newRoom id: " + newRoom.GetComponent<ZachRoomScript>().id + " roomEntering id: " + roomEntering.id + " newneighborNumber: " + roomNum(newRoom.GetComponent<ZachRoomScript>().id).NeighborNumber(roomEntering.id));
                        newRoom.GetComponent<ZachRoomScript>().neighbors[roomNum(newRoom.GetComponent<ZachRoomScript>().id).NeighborNumber(roomEntering.id)] = roomEntering.gameObject;
                        newRoom.GetComponent<ZachRoomScript>().PrintId();
                        newRoom.GetComponent<ZachRoomScript>().player = player.gameObject;
                        newRoom.name = "Room " + newRoom.GetComponent<ZachRoomScript>().id;
                        if(((Node) rooms[newRoom.GetComponent<ZachRoomScript>().id]).roomType == SpecialRoom.Goal){
                            newRoom.GetComponent<ZachRoomScript>().SpawnHole(ballCam, holeGUI, scorecardGUI);
                        }
                    }
                }
            }
        }
        roomIn = roomRef.neighbors[wallNum].GetComponent<ZachRoomScript>().id;
        
    }
    

    public class Node{
        public bool wait;
        //left, center, right, back
        public Node[] neighbors;
        public int level;
        public int id;
        public static int idCount = 0;
        public static Node None = new Node(-1);
        public SpecialRoom roomType = SpecialRoom.Not;
        //Tee, Goal, Bulldozer, LightUp
        public int[] dirTo = {-1,-1,-1,-1};
        
 

        
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
            foreach (int dir in dirTo){
                toReturn += dir + " ";
            }
            toReturn += "}\n";
            return toReturn;
        }
    }
}

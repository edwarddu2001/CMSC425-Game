using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.Random;
public class MazeCreation : MonoBehaviour
{
    
    public Node root; 
    
    [SerializeField]
    public int seed = 0;
    [SerializeField]
    public const int numNeighbors = 4;
    [SerializeField]
    public const int maxIter = 2;

    void Start()
    {
        Random.InitState(seed);
        Generate(maxIter);
        Print();
    }

    public void Print(){
        string toReturn = "";
        ArrayList unprinted = new ArrayList();
        ArrayList newUnprinted = new ArrayList();
        int min = -1;
        unprinted.Add(root);
        do {
            foreach(Node node in unprinted){
                toReturn += node.id  + ": Level = " + node.level + ", neighbors = {";
                foreach (Node neighbor in node.neighbors){
                    toReturn += neighbor.id + " ";
                    if(neighbor.id > min && neighbor != node) {newUnprinted.Add(neighbor); min = neighbor.id;}
                }
                toReturn += "}\n";    
            }
            unprinted = newUnprinted;
            newUnprinted = new ArrayList();
        } while (unprinted.Count > 0);
        Debug.Log(toReturn);

    } 


    public void Generate(int levels){
        root = new Node(false, 0);
        ArrayList unfilled = new ArrayList();
        ArrayList nextUnfilled = new ArrayList();
        ArrayList waiting = new ArrayList();
        unfilled.Add(root);

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
                                //if we aren't waiting, add it to queue, else add it to wait queue
                                if(r != NodeStatus.Waiting) {
                                    nextUnfilled.Add(node.neighbors[i]);
                                } else {
                                    waiting.Add(node.neighbors[i]);
                                }
                            } else {
                                //with no recrsion depth, make a dead end.
                                node.neighbors[i] = Node.DeadEnd(node);
                            }
                        } else {
                            //if we are connecting to an old room
                            int n = Random.Range(0,waiting.Count);
                            //connect to the room
                            node.neighbors[i] = (Node)waiting[n];
                            
                            int dir = Random.Range(0,numNeighbors);
                            //set the way back
                            node.neighbors[i].neighbors[dir] = node;
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
    }
}

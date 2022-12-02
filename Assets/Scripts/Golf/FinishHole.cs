using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishHole : MonoBehaviour
{
    /*How it works:
     - The hole is the game object you see as a black circle on the ground, with a flagpole inside of it.
     - The "true hole" is a slightly smaller area in the middle that detects collisions. Only if the ball touches the
     true hole do we say the player got the ball in. This gives the illusion of a more complicated physics simulation.
     
     - The cup is an invisible box that will be inactive until the player gets the ball in the hole. When they do,
     the cup stays invisible but becomes active, giving the illusion of it "holding the ball".*/
    public GameObject hole;
    public GameObject ballCam;
    public GameObject holeGUI;
    public GameObject scorecardGUI;

    private GameObject trueHole;
    private GameObject cup;
    private GameObject holeGround;

    [SerializeField]
    private AudioSource finishSound;

    //keeps score
    private ScorecardScript scorecard;

    // Start is called before the first frame update
    void Start()
    {
        finishSound = this.GetComponent<AudioSource>();
        trueHole = hole.transform.GetChild(1).gameObject;
        cup = hole.transform.GetChild(2).gameObject;
        cup.SetActive(false);

        holeGround = hole.transform.GetChild(3).gameObject;

        scorecard = transform.parent.GetComponent<ScorecardScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("something");
        if(other.Equals(trueHole.GetComponent<CapsuleCollider>()))
        {
            Debug.Log("Finished");
            cup.SetActive(true);

            //remove the ground below the hole's collider, so the ball appears to sink into the hole
            holeGround.GetComponent<BoxCollider>().enabled = false;

            finishSound.Play();

            //remove the move and Camera components from the ball
            Destroy(GetComponent<MoveGolf>());
            Destroy(ballCam.transform.GetChild(0).GetComponentInChildren<ThirdPersonCamera>());

            //lastly, record the final score for this hole...
            scorecard.finishThisHole();
            scorecardGUI.SetActive(true);
            holeGUI.SetActive(false);
            
        }
    }
}

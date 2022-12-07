using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorecardScript : MonoBehaviour
{
    private int holeScore;
    private int courseScore;
    private int[] holeByHole = new int[9];
    int currHole = 0;
    [SerializeField]
    private int par; 
    [SerializeField]
    private AudioSource[] sounds = new AudioSource[2];

    // Start is called before the first frame update
    void Start()
    {
        holeScore = 0;
        courseScore = 0;
        for(var i=0; i<9; i++)
        {
            holeByHole[i] = -1;
        }
    }

    public int getHoleScore()
    {
        return holeScore;
    }

    public int getCourseScore()
    {
        return holeScore;
    }

    public void finishThisHole()
    {
        courseScore += holeScore;
        holeByHole[currHole] = holeScore;
         if (holeScore <= par) {
            sounds[0].Play();

         }
         else {
            sounds[1].Play();
         }

        Debug.Log("Course score: " + courseScore);
        Debug.Log("Score for that hole: " + holeScore);

        //holeScore = 0; //not until we go to the next hole.
        currHole++;
    }

    public void takeShot()
    {
        holeScore += 1;
    }

    public int[] getHoleByHole()
    {
        return holeByHole;
    }
}

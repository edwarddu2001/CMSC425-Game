using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorecardScript : MonoBehaviour
{
    private int holeScore;
    private int courseScore;
    private int[] holeByHole = new int[18];
    int currHole = 0;

    // Start is called before the first frame update
    void Start()
    {
        holeScore = 0;
        courseScore = 0;
        for(var i=0; i<18; i++)
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

        Debug.Log("Course score: " + courseScore);
        Debug.Log("Score for that hole: " + holeScore);

        holeScore = 0;
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

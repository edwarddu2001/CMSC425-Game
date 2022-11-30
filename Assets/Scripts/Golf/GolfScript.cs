using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfScript : MonoBehaviour
{
    private int holeScore;
    private int courseScore;
    private int[] holeByHole = new int[18];

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

    // Update is called once per frame
    void Update()
    {
        
    }

    public int getHoleScore()
    {
        return holeScore;
    }

    public int getCourseScore()
    {
        return holeScore;
    }

    public void addToCourseScore(int strokes)
    {
        courseScore += strokes;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorecardScript : MonoBehaviour
{
    public SavedData data; 
    private int holeScore;
    private int courseScore;
    private string[] holeByHole = new string[9];
    int currHole;
    private string[] temp;

    private string holes;
    
    [SerializeField]
    private int par; 
    [SerializeField]
    private AudioSource[] sounds = new AudioSource[2];

    // Start is called before the first frame update
    void Start()
    {
        currHole = PlayerPrefs.GetInt("currHole");;
        holeScore = 0;
        courseScore = PlayerPrefs.GetInt("courseScore");
        holes = PlayerPrefs.GetString("holeByHole");

        Debug.Log("currHole:" + currHole);
        Debug.Log("holes:" + holes);

        /*since we can't send arrays in playerPrefs, we have to use some string manipulation
        the score of each hole is separated by a comma- like a CSV
        this string is passed between holes, effectively making it an array of ints.

        an effect of this is that starting from any hole other than hole #1 in the level selector will
        report your score as the score from hole 1.
        
        we aren't too concerned about this. The level selector was effectively only made for demonstrating certain
        levels, and for our own testing purposes. it's not how the game is meant to be played.*/
        temp = holes.Split(",");
        
        for (int i = 0 ; i < 9 ; i++) {
            if (i < temp.Length) {
                holeByHole[i] = temp[i];
            }
            else {
                holeByHole[i] = "-1";
            }
            
        }

        
    }

    public int getHoleScore()
    {
        return holeScore;
    }

    public int getCourseScore()
    {
        return courseScore;
    }

    public void finishThisHole()
    {
        courseScore += holeScore;
        holeByHole[currHole] = holeScore.ToString();
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
        holes = string.Concat(holes, holeScore + ",");
    }

    public void takeShot()
    {
        holeScore += 1;
    }

    public string[] getHoleByHole()
    {
        return holeByHole;
    }

    //sets the course score, current hole #, and holeByHole array for future holes.
    public void doIt() {
        PlayerPrefs.SetInt("courseScore", courseScore);
        PlayerPrefs.SetInt("currHole", currHole);
        
        PlayerPrefs.SetString("holeByHole", holes);
    }
}

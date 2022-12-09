using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScorecardUI : MonoBehaviour
{
    public GameObject UIManager;
    public GameObject[] holeScoreDisplays;
    public GameObject holeSummary;

    private GolfUIManager holeUI;
    private TextMeshProUGUI[] holeScoreTexts;
    private ScorecardScript scorecard;
    int holeNum;

    // Start is called before the first frame update
    void Start()
    {
        holeScoreTexts = new TextMeshProUGUI[9];

        holeUI = UIManager.GetComponent<GolfUIManager>();
        scorecard = holeUI.ball.transform.parent.GetComponent<ScorecardScript>();
        for(var i=0; i<holeScoreDisplays.Length; i++)
        {
            holeScoreTexts[i] = holeScoreDisplays[i].GetComponent<TextMeshProUGUI>();
        }
    }

    void UpdateScoreboard()
    {
        Start();

        holeNum = holeUI.hole.GetComponent<HoleProperties>().holeNumber;

        string[] holeScoreValues = scorecard.getHoleByHole();
        string disp;

        string txt = holeSummary.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
        string tot = holeSummary.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text;
        holeSummary.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = txt + scorecard.getHoleScore() + ".";
        holeSummary.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = tot + scorecard.getCourseScore() + ".";

        for (var i = 0; i < 9; i++) {
            if(holeScoreValues[i].Equals("-1"))
            {
                disp = "?";
            } else
            {
                disp = holeScoreValues[i].ToString();
            }

            holeScoreTexts[i].SetText(disp);
        }
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        UpdateScoreboard();
        Cursor.lockState = CursorLockMode.None;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GolfUIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject holeNumDisplay;
    public GameObject shotNumDisplay;
    public GameObject currentAbilityDisplay;

    public GameObject abObserver;
    public GameObject hole;
    public GameObject ball;

    private int holeNum;
    private int shotNum;

    void Start()
    {
        holeNum = hole.GetComponent<HoleProperties>().holeNumber;
        shotNum = 1;
    }

    // Update is called once per frame
    void Update()
    {
        shotNum = ball.transform.parent.GetComponent<ScorecardScript>().getHoleScore();
        holeNumDisplay.GetComponent<TextMeshProUGUI>().SetText("Hole #" + holeNum);
        shotNumDisplay.GetComponent<TextMeshProUGUI>().SetText("Shot #" + shotNum);
        //currentAbilityDisplay.GetComponent<TextMeshProUGUI>().SetText(abObserver.GetComponent<AbilityObserver>().ability.ToString());


        // ability name

    }
}

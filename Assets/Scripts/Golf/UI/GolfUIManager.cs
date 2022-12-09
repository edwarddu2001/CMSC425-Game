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
    public Image currentAbilityIcon;
    public Sprite[] allAbilityIcons;

    public GameObject hole;
    public GameObject ball;

    private AbObserver2 abObserver;
    private int holeNum;
    private int shotNum;

    void Start()
    {
        holeNum = hole.GetComponent<HoleProperties>().holeNumber;
        shotNum = 0;

        abObserver = ball.GetComponent<AbObserver2>();
    }

    // Update is called once per frame
    void Update()
    {
        shotNum = ball.transform.parent.GetComponent<ScorecardScript>().getHoleScore();
        holeNumDisplay.GetComponent<TextMeshProUGUI>().SetText("Hole #" + holeNum);
        shotNumDisplay.GetComponent<TextMeshProUGUI>().SetText("Shot #" + shotNum);

        //display ability name
        string abName = abObserver.ability.GetAbilityName();

        //style choice
        if(abName.Equals("Nothing")) { abName = "No Ability...";  }

        //invisibility also displays the timer
        if (abName.Equals("Invisible")) {
            abName = abName + " (" + 
            ((InvisibleAbility)(abObserver.ability)).getTimeRemaining() + ")";
        }

        currentAbilityDisplay.GetComponent<TextMeshProUGUI>().SetText(abName);




        // ability icon / picture
        updateAbilityIcon();
    }

    void updateAbilityIcon()
    {
        string abName = abObserver.ability.GetAbilityName();
        int index;

        switch (abName)
        {
            case "Shrink": index = 1; break;
            case "Lightup": index = 2; break;
            case "Bulldozer": index = 3; break;
            case "Labyrinth": index = 4; break;
            case "Chipshot": index = 5; break;
            case "Movement+": index = 6; break;
            case "ZeroGrav": index = 7; break;
            default: index = 0; break;
        }

        currentAbilityIcon.sprite = allAbilityIcons[index];
    }
}

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

    public GameObject abObserver;
    public GameObject hole;
    public GameObject ball;

    private int holeNum;
    private int shotNum;

    private MonoBehaviour[] abilityScripts;

    void Start()
    {
        holeNum = hole.GetComponent<HoleProperties>().holeNumber;
        shotNum = 1;

        abilityScripts = new MonoBehaviour[8];
        for(var i=0; i < 8; i++)
        {
            abilityScripts[i] = null;
        }

        determineAbilities();
    }

    // Update is called once per frame
    void Update()
    {
        shotNum = ball.transform.parent.GetComponent<ScorecardScript>().getHoleScore();
        holeNumDisplay.GetComponent<TextMeshProUGUI>().SetText("Hole #" + holeNum);
        shotNumDisplay.GetComponent<TextMeshProUGUI>().SetText("Shot #" + shotNum);
        //currentAbilityDisplay.GetComponent<TextMeshProUGUI>().SetText(abObserver.GetComponent<AbilityObserver>().ability.ToString());


        // ability name
        updateAbilityIcon();
    }

    //add something eventually for all abilities we will have
    void determineAbilities()
    {
        abilityScripts[0] = ball.GetComponent<ShrinkAbility>();
        abilityScripts[1] = ball.GetComponent<LightUpAbility>();
        //abilityScripts[2] = ball.GetComponent<BulldozerAbility>();
        //abilityScripts[3] = ball.GetComponent<LabyrinthAbility>();
        //abilityScripts[4] = ball.GetComponent<ChipshotAbility>();
        //abilityScripts[5] = ball.GetComponent<MoveplusAbility>();
        //abilityScripts[6] = ball.GetComponent<ZerogravAbility>();
        //abilityScripts[7] = ball.GetComponent<???Ability>(); ...
    }

    void updateAbilityIcon()
    {
        if(abilityScripts[0] != null)
        {
            if(((ShrinkAbility) abilityScripts[0]).isShrunk)
            {
                currentAbilityDisplay.GetComponent<TextMeshProUGUI>().SetText("Shrink");
                currentAbilityIcon.sprite = allAbilityIcons[0];
            }
        }
    }
}

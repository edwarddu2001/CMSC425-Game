using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//this script handles the UI on the left side of your screen, telling you what the current controls of the game are.
public class ControlsUI : MonoBehaviour
{
    //player object is the only thing we need for this script.
    public GameObject playerContainer;
    private GameObject playerBall;
    private GameObject shotArrow;

    //DYNAMIC CONTROLS: changes the UI depending on player's game state.
    public GameObject dynamicControlsTemplate;
    private GameObject dynamicControls;
    public Image keyImage;
    public Sprite[] keyOptions;
    public TextMeshProUGUI descTemplate;

    private string[] arr1 = new string[1];
    private string[] arr2 = new string[2];

    private AbObserver2 abObserver;
    string abil;

    private bool activeControls;
    private bool inMotion;

    //for creating new UI- there are too many combos to make by hand! so we'll "generate" the control tooltip
    //based on what the player's current state is.
    private GameObject panel;
    private float keyXCoords = -50.0f;
    private float keyYCoords = -50.0f;

    // initialize variables / get components
    void Start()
    {
        playerBall = playerContainer.transform.GetChild(0).gameObject;
        shotArrow = playerContainer.transform.GetChild(1).gameObject;

        activeControls = true;
        abObserver = playerBall.GetComponent<AbObserver2>();

        //ugly, but no need to set it as a public variable. we know exactly where it is.
        panel = gameObject.transform.GetChild(0).transform.GetChild(0).gameObject;

        string[] sample = new string[2];
        sample[0] = "K"; sample[1] = "F";
        //generateNewUIKey(sample);
    }

    //clears out the old controls and makes new ones.
    //TODO: REMOVE DEBUGGING ITEMS
    public void resetControlGUI(bool inMotion, Ability2 activeAbility)
    {
        if(dynamicControls != null) {
            Destroy(dynamicControls);
            keyYCoords = -50.0f;
        }
        //this object is the parent of all "dynamic controls" we generate. when we want to clear dynamic controls, we just delete it
        dynamicControls = Object.Instantiate(dynamicControlsTemplate);
        dynamicControls.SetActive(true);
        dynamicControls.transform.SetParent(dynamicControlsTemplate.transform.parent);
        dynamicControls.GetComponent<RectTransform>().anchoredPosition3D = dynamicControlsTemplate.GetComponent<RectTransform>().anchoredPosition3D;

        string TESTCONTROLS = "";
        abil = activeAbility.GetAbilityName();

        if (inMotion)
        {
            TESTCONTROLS += "Moving; ";
            standardMovingKeys();
        } else
        {
            TESTCONTROLS += "At rest; ";
            standardSetupKeys();
        }

        TESTCONTROLS += activeAbility.GetAbilityName();

        Debug.Log("New Contols: [" + TESTCONTROLS + "]");
    }

    //generate a new key, or combo of keys, at the next available y coordinate
    private void generateNewUIKey(string[] keys, string desc/*, int[] optionCodes*/)
    {
        Image newKey;
        for (var i=0; i<keys.Length; i++)
        {
            //create the new Key
            newKey = Object.Instantiate(keyImage);
            newKey.gameObject.SetActive(true);
            RectTransform trueTransform = newKey.GetComponent<RectTransform>();
            //trueTransform.SetParent(keyImage.transform.parent);
            trueTransform.SetParent(dynamicControls.transform);

            Vector3 transVec = trueTransform.transform.position;
            trueTransform.anchoredPosition3D = new Vector3(transVec.x - keyXCoords * i, transVec.y + keyYCoords, transVec.z);

            TextMeshProUGUI textToReplace = newKey.GetComponentInChildren<TextMeshProUGUI>();
            textToReplace.SetText(keys[i]);


        }
        //add a description to whatever these controls might be...
        TextMeshProUGUI descTMP = Object.Instantiate(descTemplate);
        descTMP.gameObject.SetActive(true);
        descTMP.transform.SetParent(dynamicControls.transform);
        //move it to the far right edge of the controls panel
        descTMP.rectTransform.anchorMin = new Vector2(1, 1);
        descTMP.rectTransform.anchorMax = new Vector2(1, 1);

        Vector3 moveDescTo = keyImage.rectTransform.anchoredPosition3D;
        descTMP.rectTransform.anchoredPosition3D = new Vector3(moveDescTo.x + 65, moveDescTo.y + keyYCoords, moveDescTo.z);
        descTMP.SetText(desc);

        //start a new row...
        keyYCoords = keyYCoords - 50.0f;
    }

    private void standardSetupKeys()
    {

        arr2[0] = "A"; arr2[1] = "D";
        generateNewUIKey(arr2, "Change Angle");

        arr2[0] = "W"; arr2[1] = "S";
        generateNewUIKey(arr2, "Change Power");

        /*arr2[0] = "Q"; arr2[1] = "E";
        generateNewUIKey(arr2);*/
    }

    private void standardMovingKeys()
    {
        arr1[0] = "R";
        generateNewUIKey(arr1, "Slow Motion");
    }

    // Update is called once per frame
    /*void Update()
    {

    }*/
}

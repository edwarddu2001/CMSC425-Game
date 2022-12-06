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
    public TextMeshProUGUI motionTeller;
    public GameObject dynamicControlsTemplate;
    private GameObject dynamicControls;
    public Image keyImage;
    //public Image bigKeyImage;
    public Sprite[] keyOptions;
    public TextMeshProUGUI descTemplate;

    private string[] arr1 = new string[1];
    private string[] arr2 = new string[2];

    private AbObserver2 abObserver;
    string abil;

    private bool activeControls;
    private bool inMotion;

    /*This script handles the controls that you see on the left side of your screen.
     How it works: Your allowed controls are constantly changing depending on what the player's golf ball
     is currently doing. The player needs to know all of these options at the exact moment they're available to
     them! We can achieve this by checking the player's state, and updating the script whenever the state changes.
     
     Define "Player State" with the two most important factors of what the golf ball is currently doing:
       1. Motion: is the ball moving? (more specifically: is the player in the process of taking their shot, or setting it up?)
       2. Current ability
       
     There are only a handful of ways these factors will ever change, and lucky for us, all of them occur in 3 scripts:
     MoveGolf (ball movement + taking shots), AbilityPickup2 (pickup ability), and OutOfBounds (respawn player.)
     
     We added functionality to those 3 scripts, so that they ping this script whenever something important happens.
     
     You may be asking, why not use event listeners? Because the event in question needs to be setup for 3 different scripts. AbilityPickup2
     is especially problematic because there are multiple ability coins in a level. If we need an event listener for all those objects, it's
     more trouble than it's worth to set up such a system. We know exactly when and why state can change already.
     
     We considered using some sort of static event, but that was never covered in class so we had to abandon that idea.*/
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
    }

    /*Because player state only changes in specific cases, we don't need to constantly be checking for
     those changes. The only thing we need "update" for is to toggle the controls panel on demand.*/
    private void Update()
    {
        if (activeControls != gameObject.activeInHierarchy)
        {
            gameObject.SetActive(activeControls);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            activeControls = !activeControls;
        }
    }

    //clears out the old controls and makes new ones.
    //TODO: REMOVE DEBUGGING ITEMS
    public void resetControlGUI(bool inMotion, Ability2 activeAbility)
    {
        if (activeControls)
        {
            if (dynamicControls != null)
            {
                Destroy(dynamicControls);
                keyYCoords = -50.0f;
            }
            //this object is the parent of all "dynamic controls" we generate. when we want to clear dynamic controls, we just delete it
            dynamicControls = Object.Instantiate(dynamicControlsTemplate);
            dynamicControls.SetActive(true);
            dynamicControls.transform.SetParent(dynamicControlsTemplate.transform.parent);
            dynamicControls.GetComponent<RectTransform>().anchoredPosition3D = dynamicControlsTemplate.GetComponent<RectTransform>().anchoredPosition3D;

            //string TESTCONTROLS = "";
            abil = activeAbility.GetAbilityName();

            if (inMotion)
            {
                //TESTCONTROLS += "Moving; ";
                motionTeller.SetText("In Motion...");
                
                //TODO: Labyrinth special case
                if (abil.Equals("Labyrinth"))
                {
                    arr2[0] = "W"; arr2[1] = "S";
                    generateNewUIKey(arr2, "");
                    arr2[0] = "A"; arr2[1] = "D";
                    generateNewUIKey(arr2, "Rot. Labyrinth");

                    arr1[0] = "F";
                    generateNewUIKey(arr1, "End Labyrinth");
                }
                else if (abil.Equals("Movement+"))
                {
                    arr2[0] = "W"; arr2[1] = "S";
                    generateNewUIKey(arr2, "");
                    arr2[0] = "A"; arr2[1] = "D";
                    generateNewUIKey(arr2, "Spin ball");

                    arr2[0] = "W"; arr2[1] = "F";
                    generateNewUIKey(arr2, "FULL STOP!");
                }
                else
                {
                    standardMovingKeys();

                    if (!abil.Equals("ZeroGrav"))
                    {
                        generateNewBigUIKey("Space", "Jump");
                    }
                }
            }
            else
            {
                //TESTCONTROLS += "At rest; ";
                motionTeller.SetText("Shot Setup");

                standardSetupKeys();
                if(abil.Equals("Chipshot") || abil.Equals("ZeroGrav"))
                {
                    arr2[0] = "Q"; arr2[1] = "E";
                    generateNewUIKey(arr2, "Change Loft");
                } else if(abil.Equals("Labyrinth"))
                {
                    arr1[0] = "F";
                    generateNewUIKey(arr1, "Toggle Labyrinth Mode");
                }

            }


            //TESTCONTROLS += activeAbility.GetAbilityName();
            //Debug.Log("New Contols: [" + TESTCONTROLS + "]");
        }
    }

    //generate a new key, or combo of keys, at the next available y coordinate
    private void generateNewUIKey(string[] keys, string desc/*, Color col, int[] optionCodes*/)
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

    //variant of the above method, except it pushes bigger key icons onto the scene.
    private void generateNewBigUIKey(string key, string desc)
    {
        Image newKey = Object.Instantiate(keyImage);
        newKey.gameObject.SetActive(true);
        newKey.rectTransform.SetParent(dynamicControls.transform);

        Vector3 transVec = newKey.rectTransform.transform.position;
        //newKey.rectTransform.anchoredPosition3D = new Vector3(transVec.x, transVec.y + keyYCoords, transVec.z);

        //make it twice as big as the normal key
        newKey.rectTransform.localScale = new Vector3(2, 1, 1);
        newKey.rectTransform.anchoredPosition3D = new Vector3(transVec.x + 20, transVec.y + keyYCoords, transVec.z);

        TextMeshProUGUI textToReplace = newKey.GetComponentInChildren<TextMeshProUGUI>();
        textToReplace.rectTransform.localScale = new Vector3(0.5f, 1, 1); //correct text size
        textToReplace.fontSize = textToReplace.fontSize * 0.6f;
        textToReplace.SetText(key);

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

        generateNewBigUIKey("LSHIFT", "Speed Setup");
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
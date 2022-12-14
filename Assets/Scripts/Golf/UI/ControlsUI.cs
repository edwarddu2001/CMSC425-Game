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
    private Image panel;

    [SerializeField]
    private bool labyrinthMode = false;
    [SerializeField]
    private bool movingLabyrinth = false;

    //DYNAMIC CONTROLS: changes the UI depending on player's game state.
    public TextMeshProUGUI motionTeller;
    private MotionTellerUI motionTellerUI;
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

    //used for controls on / off animation
    private bool activeControls, transition;
    private float timer;
    private Vector3 veca, vecb;

    //the "shoot" panel blurs a little when the ball is in motion.
    public Image shootPanel;

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

    private float keyXCoords = -50.0f;
    private float keyYCoords = -50.0f;

    // initialize variables / get components
    void Start()
    {
        playerBall = playerContainer.transform.GetChild(0).gameObject;
        shotArrow = playerContainer.transform.GetChild(1).gameObject;

        activeControls = true; transition = false;
        abObserver = playerBall.GetComponent<AbObserver2>();

        motionTellerUI = motionTeller.GetComponent<MotionTellerUI>();

        //ugly, but still better than public variables...
        panel = gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();

        veca = panel.rectTransform.position; vecb = new Vector3(veca.x - 400, veca.y, veca.z);
        timer = 0.0f;
    }

    /*Because player state only changes in specific cases, we don't need to constantly be checking for
     those changes. The only thing we need "update" for is to toggle the controls panel on demand.*/
    private void Update()
    {
        if (transition)
        {
            timer += Time.deltaTime;
            //make the transition last 3 seconds in either direction.
            if (timer <= 3.0f)
            {
                if (activeControls)
                {
                    panel.rectTransform.position = Vector3.Lerp(vecb, veca, timer / 3.0f);
                }
                else
                {
                    panel.rectTransform.position = Vector3.Lerp(veca, vecb, timer / 3.0f);
                }
            } else
            {
                //done transition
                transition = false;
                timer = 0.0f;
            }
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            activeControls = !activeControls;
            transition = true;
        }
    }

    //clears out the old controls and makes new ones.
    //we have to figure out which controls are active based on player's current state, then add them in
    public void resetControlGUI(bool inMotion, Ability2 activeAbility)
    {
        if (activeControls)
        {
            abil = activeAbility.GetAbilityName();
            //change color at the top to the color of our ability
            motionTeller.color = getAssociatedColor(abil);


            if (dynamicControls != null)
            {
                Destroy(dynamicControls);
                keyYCoords = -50.0f;
            }
            //this object is the parent of all "dynamic controls" we generate. when we want to clear dynamic controls, we just delete it
            dynamicControls = Object.Instantiate(dynamicControlsTemplate);
            dynamicControls.SetActive(true);
            dynamicControls.transform.SetParent(dynamicControlsTemplate.transform.parent);
            RectTransform rt = dynamicControls.GetComponent<RectTransform>();
            rt.anchoredPosition3D = dynamicControlsTemplate.GetComponent<RectTransform>().anchoredPosition3D;
            //rt.localScale = new Vector3(rt.localScale.x * 2.0f, rt.localScale.y * 2.0f, rt.localScale.z * 2.0f);
            
            //for all ability cases when the ball IS MOVING
            if (inMotion)
            {
                motionTellerUI.startMovingText();
                shootPanel.color = new Color(shootPanel.color.r, shootPanel.color.g, shootPanel.color.b, 0.2f);

                //TODO: Labyrinth special case
                if (abil.Equals("Labyrinth"))
                {

                    //case 3: labyrinth mode on, and currently moving (rotating the labyrinth Platform)
                    if (movingLabyrinth)
                    {
                        arr2[0] = "W"; arr2[1] = "S";
                        generateNewUIKey(arr2, "", getAssociatedColor(abil));
                        arr2[0] = "A"; arr2[1] = "D";
                        generateNewUIKey(arr2, "Rot. Labyrinth", getAssociatedColor(abil));

                        arr1[0] = "F";
                        generateNewUIKey(arr1, "End Labyrinth", getAssociatedColor(abil));
                    } else
                    {
                        standardMovingKeys();
                    }

                }
                else if (abil.Equals("Movement+"))
                {
                    arr2[0] = "W"; arr2[1] = "S";
                    generateNewUIKey(arr2, "", getAssociatedColor(abil));
                    arr2[0] = "A"; arr2[1] = "D";
                    generateNewUIKey(arr2, "Spin ball", getAssociatedColor(abil));

                    arr1[0] = "R";
                    generateNewUIKey(arr1, "Partial Stop", getAssociatedColor(abil));
                    arr1[0] = "F";
                    generateNewUIKey(arr1, "FULL STOP!", getAssociatedColor(abil));
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

            //for all ability cases where the ball IS STOPPED
            else
            {
                //TESTCONTROLS += "At rest; ";
                motionTellerUI.stopMovingText();
                motionTeller.SetText("Shot Setup");
                shootPanel.color = new Color(shootPanel.color.r, shootPanel.color.g, shootPanel.color.b, 0.5f);

                if (abil.Equals("Labyrinth"))
                {
                    //case 1: labyrinth mode off
                    if (!labyrinthMode)
                    {
                        standardSetupKeys();
                        arr1[0] = "F";
                        generateNewUIKey(arr1, "Lab Mode ON", getAssociatedColor(abil));
                    }
                    //case 2: labyrinth mode on, but not moving yet
                    if (labyrinthMode && !movingLabyrinth)
                    {
                        arr1[0] = "F";
                        generateNewUIKey(arr1, "Lab Mode OFF", getAssociatedColor(abil));

                        generateNewBigUIKey("Space", "Start!");
                    }
                }
                else
                {
                    standardSetupKeys();
                    if (abil.Equals("Chipshot") || abil.Equals("ZeroGrav"))
                    {
                        arr2[0] = "Q"; arr2[1] = "E";
                        generateNewUIKey(arr2, "Change Loft", getAssociatedColor(abil));
                    }
                    else if (abil.Equals("Labyrinth"))
                    {
                        arr1[0] = "F";
                        generateNewUIKey(arr1, "Toggle Labyrinth Mode", getAssociatedColor(abil));
                    }
                }

            }

        }
    }

    //MoveGolf uses these to tell this script when labyrinth mode is on or off, a special but important case.
    public void reportLabyrinthMode(bool on, bool moving)
    {
        labyrinthMode = on;
        movingLabyrinth = moving;

    }



    /*all methods below this line are related to generating the UI itself. since there's too many combinations
    //to hard code, i figured i should do it programatically. it's not that interesting, but if you want a basic
    idea of how it works: there's a disabled object called "dynamicControls" in the controlsUI object, which gets
    set to a new object containing all the controls you see on a reported change in state. when a new change in state 
    happens, the old one is deleted so as to not cause any memory leaks.*/

    //generate a new key, or combo of keys, at the next available y coordinate
    //return value changes the color of the text, if we'd like.
    private void generateNewUIKey(string[] keys, string desc, Color col/*, int[] optionCodes*/)
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
            textToReplace.color = col;

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
        descTMP.color = col;

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
        generateNewUIKey(arr2, "Change Angle", Color.white);

        arr2[0] = "W"; arr2[1] = "S";
        generateNewUIKey(arr2, "Change Power", Color.white);

        generateNewBigUIKey("LSHIFT", "Speed Setup");
        /*arr2[0] = "Q"; arr2[1] = "E";
        generateNewUIKey(arr2);*/
    }

    private void standardMovingKeys()
    {
        arr1[0] = "R";
        generateNewUIKey(arr1, "Slow Motion", Color.white);
    }

    //returns the color we should change text to, depending on our ability
    private Color getAssociatedColor(string abilName)
    {
        if (abilName != null) {
            switch (abilName)
            {
                case "Shrink":
                    return new Color(0, 0, 255/255);
                case "Bulldozer":
                    //return new Color(0, 0, 255);
                    return new Color(236.0f / 255.0f, 19.0f / 255.0f, 19.0f / 255.0f);
                case "Chipshot":
                    return new Color(43.0f / 255.0f, 164.0f / 255.0f, 39.0f / 255.0f);
                case "Labyrinth":
                    return new Color(110.0f / 255.0f, 38.0f / 255.0f, 142.0f / 255.0f);
                case "Lightup":
                    return new Color(226.0f / 255.0f, 170.0f / 255.0f, 97.0f / 255.0f);
                case "Movement+":
                    return new Color(255.0f / 255.0f, 105.0f / 255.0f, 11.0f / 255.0f);
                case "ZeroGrav":
                    return new Color(30.0f / 255.0f, 30.0f / 255.0f, 30.0f / 255.0f);
                default:
                    return new Color(255 / 255, 255 / 255, 255 / 255);
            }
        } else
        {
            Debug.Log("the player ability wasn't set yet.");
            return new Color(0, 0, 0);
        }
    }
}

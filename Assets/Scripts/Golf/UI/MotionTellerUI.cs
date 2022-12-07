using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MotionTellerUI : MonoBehaviour
{
    TextMeshProUGUI txt;
    string starterText;
    bool moving = false, hasNotRegistered1 = true, hasNotRegistered2 = true;
    float timePassed;

    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<TextMeshProUGUI>();
        starterText = "In Motion.";
        timePassed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            timePassed += Time.deltaTime;
            Debug.Log(timePassed % 3.0f);
            if(timePassed % 3.0f > 1.0f && hasNotRegistered1)
            {
                txt.text = txt.text + ".";
                hasNotRegistered1 = false;
            } else if(timePassed % 3.0f > 2.0f && hasNotRegistered2)
            {
                txt.text = txt.text + ".";
                hasNotRegistered2 = false;
            } else if(timePassed % 3.0f < 1.0f && !hasNotRegistered1 && !hasNotRegistered2)
            {
                txt.text = starterText;
                hasNotRegistered1 = true;
                hasNotRegistered2 = true;
            }
        }
    }

    public void startMovingText()
    {
        moving = true;
        txt.text = starterText;
    }

    public void stopMovingText()
    {
        moving = false;
        timePassed = 0;
        hasNotRegistered1 = hasNotRegistered2 = true;
    }
}

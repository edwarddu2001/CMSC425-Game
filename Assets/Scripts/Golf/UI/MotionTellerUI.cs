using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MotionTellerUI : MonoBehaviour
{
    TextMeshProUGUI txt;
    string starterText;
    bool moving = false;
    float timePassed;

    // Start is called before the first frame update
    void Start()
    {
        txt = GetComponent<TextMeshProUGUI>();
        starterText = txt.text;
        timePassed = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
        {
            timePassed += Time.deltaTime;
            if(timePassed % 1.5f < 0.5f)
            {
                txt.text = txt.text + ".";
            } else if(timePassed % 1.5f < 1.0f)
            {
                txt.text = txt.text + ".";
            } else
            {
                txt.text = starterText;
            }
        }
    }

    public void startMovingText()
    {
        moving = true;
    }

    public void stopMovingText()
    {
        moving = false;
        timePassed = 0;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeToNextScene : MonoBehaviour
{
    public Button yourButton;

    // Start is called before the first frame update
    void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        //btn.onClick.AddListener(changeScene());
    }

    void changeScene()
    {
        Debug.Log("Right now i'm useless, but eventually i'll change to the next scene. This script is in assets > " +
           "Scripts > Misc, called 'changeToNextScene'");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

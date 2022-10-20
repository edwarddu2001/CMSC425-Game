using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseGame : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isPaused = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P)) {
            if (isPaused) {
                ResumeGame();
            } else {
                Pause();
            }
        }
    }
    void Pause ()
    {
        Time.timeScale = 0;
        isPaused = true;
    }
    void ResumeGame ()
    {
        Time.timeScale = 1;
        isPaused = false;
    }
}

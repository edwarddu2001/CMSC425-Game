using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class ResetLevel : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    { //you can set a condition here where if true, the following executes (level resets)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}

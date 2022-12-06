using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class LevelLoader : MonoBehaviour
{
    public Animator nextLevelTransition;
    public float transitionTime = 1f;
    
    //serialized for testing purposes
 
    private bool nextLevel = false;
    private bool mainMenu = false;

    [SerializeField]
    private string nextLevelName = "";
    [SerializeField]
    private SceneName mainMenuScene;
    private string mainMenuSceneName; 
 
    void Start(){
        mainMenuSceneName = mainMenuScene.name;
    }

    void Update()
    {
        if (nextLevel){
            StartCoroutine(LoadLevel(nextLevelName));
            nextLevel = false;
        }
    }

    IEnumerator LoadLevel(string levelIndex){
        //play animation
        nextLevelTransition.SetTrigger("Start");
        //wait
        yield return new WaitForSeconds(transitionTime);
        //load scene
         SceneManager.LoadScene(levelIndex);
    }
}

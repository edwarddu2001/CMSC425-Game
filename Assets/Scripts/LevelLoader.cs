using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator nextLevelTransition;
    public float transitionTime = 1f;
    
    public enum levelType {Next, Menu};
    public levelType linkedTo = levelType.Next; 
    public bool active = false;

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
        if (active){
            switch (linkedTo){
                case levelType.Next:
                    StartCoroutine(LoadLevel(nextLevelName));
                    break;
                case levelType.Menu:
                    StartCoroutine(LoadLevel(mainMenuSceneName));
                    break;
                default:
                    break;
            }
            active = false;
        }
 
    }

    IEnumerator LoadLevel(string levelIndex){
        string sceneNameOld;
        
        //play animation
        nextLevelTransition.SetTrigger("Start");
        //wait
        yield return new WaitForSeconds(transitionTime);
        //load scene
        sceneNameOld = SceneManager.GetActiveScene().name;
        Debug.Log(sceneNameOld);
        
        SceneManager.LoadScene(levelIndex, LoadSceneMode.Additive);

        
        yield return null;
        //SceneManager.SetActiveScene(SceneManager.GetSceneByName(levelIndex));
        SceneManager.UnloadSceneAsync(sceneNameOld);
        
    }

    public void activate(){
        Debug.Log("I work");
        if (!active){
            active = true;
        }
    }

   

  
}

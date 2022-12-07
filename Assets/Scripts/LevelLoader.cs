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
    private bool active = false;

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
        //play animation
        nextLevelTransition.SetTrigger("Start");
        //wait
        yield return new WaitForSeconds(transitionTime);
        //load scene
         SceneManager.LoadScene(levelIndex);
    }

    public void activate(){
        if (!active){
            active = true;
        }
    }
}

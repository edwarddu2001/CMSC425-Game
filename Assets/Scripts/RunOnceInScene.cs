using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RunOnceInScene : MonoBehaviour
{
    [SerializeField] private MazeCreation Spawner;
    [SerializeField] private string crazyName;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Start Began");
        StartCoroutine(WaitUntilActive());
    }


    IEnumerator WaitUntilActive(){
        Debug.Log("Wait Coroutine Began");
        yield return new WaitUntil(IsActiveScene);
        Spawner.OnlyRunOnceInCorrectScene();
    }

    bool IsActiveScene(){
        Debug.Log(SceneManager.GetActiveScene().name + "|" + crazyName);
        return SceneManager.GetActiveScene().name.Equals(crazyName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

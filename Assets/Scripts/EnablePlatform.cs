using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnablePlatform : MonoBehaviour
{
    AudioSource platformAppears; 
    private bool isEnabled = false;
    [SerializeField]
    private GameObject platform; 

   

    void Start()
    {
        platformAppears = this.GetComponent<AudioSource>();

    }

    private void OnTriggerEnter(Collider other) {
		if(!isEnabled){
			platform.SetActive(true);
            platformAppears.Play();
            isEnabled = true;
		}
	}
}

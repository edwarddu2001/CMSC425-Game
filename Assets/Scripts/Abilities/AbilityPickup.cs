using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This goes on any object that when interacted with
    will activate an ability.
*/



public class AbilityPickup : MonoBehaviour
{
    private AudioSource pickupSound; 
    private bool hasPlayed = false;
    void Start() {
        pickupSound = this.GetComponent<AudioSource>();
    }
    //the controller in the scene that corresponds 
    //to this object's ability 
    [SerializeField]
    private AbilityController controller;
    
    void OnTriggerEnter(Collider other){
        if(string.Equals(other.gameObject.tag,"Player")){
            controller.ChangeActive(true);

            if(!hasPlayed) {
                pickupSound.Play();
                hasPlayed = true;
            }


        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This goes on any object that when interacted with
    will activate an ability.
*/

public class AbilityPickup : MonoBehaviour
{
    //the controller in the scene that corresponds 
    //to this coin's ability 
    [SerializeField]
    private AbilityController controller;
    
    void OnTriggerEnter(Collider other){
        if(string.Equals(other.gameObject.tag,"Player")){
            controller.ChangeActive(true);
        }
    }
}

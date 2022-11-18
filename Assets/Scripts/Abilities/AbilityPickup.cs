using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPickup : MonoBehaviour
{
    [SerializeField]
    private AbilityController controller;
    
    void OnTriggerEnter(Collider other){
        if(string.Equals(other.gameObject.tag,"Player")){
            controller.ChangeActive(true);
        }
    }
}

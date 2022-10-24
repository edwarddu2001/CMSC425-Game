using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability : MonoBehaviour{
    void OnEquip(){
        Debug.Log("Ability Equipped");
    }

    void OnActivate(){
        Debug.Log("Ability Activated");
    }

    void OnDeactivate(){
        Debug.Log("Ability Deactivated");
    }
}
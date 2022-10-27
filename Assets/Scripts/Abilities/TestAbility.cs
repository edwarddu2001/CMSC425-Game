using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAbility : Ability
{
    
    
    void Update(){

    }

    void OnEquip(){
        Debug.Log("TestAbility Equipped");
    }

    void OnActivate(){
        Debug.Log("TestAbility Activated");
    }

    void OnDeactivate(){
        Debug.Log("TestAbility Deactivated");
    }
}

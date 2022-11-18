using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//A sample ability that just prints to console 
//when turned on and off.
public class SampleAbility : Ability
{
    public override void OnActivate(){
        Debug.Log("I'm active!");
    }
    public override void OnDeactivate(){
        Debug.Log("I'm inactive!");
    }

}

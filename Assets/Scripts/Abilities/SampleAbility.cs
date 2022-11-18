using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleAbility : Ability
{
    public override void OnActivate(){
        Debug.Log("I'm active!");
    }
    public override void OnDeactivate(){
        Debug.Log("I'm inactive!");
    }

}

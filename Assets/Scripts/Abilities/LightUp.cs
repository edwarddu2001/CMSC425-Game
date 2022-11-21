using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightUp : Ability
{
    public bool litUp = false;
    
    public override void OnActivate(){
        litUp = true;
    }
    public override void OnDeactivate(){
        litUp = false;
    }
}

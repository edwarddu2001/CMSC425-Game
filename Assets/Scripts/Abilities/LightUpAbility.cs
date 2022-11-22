using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightUpAbility : Ability
{
   // public event Action<bool> ReportActivate;
    public event Action<bool> ReportLit;
    public bool litUp = false;
     
    public override void OnActivate(){
        litUp = true;
        ReportLit(litUp);
    }
    public override void OnDeactivate(){
        litUp = false;
        ReportLit(litUp);
    }
}

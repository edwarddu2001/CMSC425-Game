using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightUpAbility : Ability
{
   // public event Action<bool> ReportActivate;
    public event Action<bool> ReportLit;
    public bool litUp = false;
    public Material material;
     
    public override void OnActivate(){
        litUp = true;
        ReportLit(litUp);
        this.GetComponent<MeshRenderer>().material = material;
    }
    public override void OnDeactivate(){
        litUp = false;
        ReportLit(litUp);
    }
}

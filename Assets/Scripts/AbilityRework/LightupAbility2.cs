using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightupAbility2 : Ability2
{
    // public event Action<bool> ReportActivate;
    public event Action<bool> ReportLit;
    public bool litUp = false;
    public Material material;

    public override string GetAbilityName()
    {
        return "Lightup";
    }

    public override void OnActivate(GameObject target)
    {
        litUp = true;
        ReportLit(litUp);
        target.GetComponent<MeshRenderer>().material = material;
    }
    public override void OnDeactivate(GameObject target)
    {
        litUp = false;
        ReportLit(litUp);
    }
}

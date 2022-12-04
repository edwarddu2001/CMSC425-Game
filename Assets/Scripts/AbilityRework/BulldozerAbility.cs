using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulldozerAbility : Ability2
{
    [SerializeField]
    public Material material;
    public event Action<bool> ReportBulldozing;

    public override string GetAbilityName()
    {
        return "Bulldozer";
    }

    public override void OnActivate(GameObject target)
    {
        ReportBulldozing(true);
        target.GetComponent<MeshRenderer>().material = material;

    }
    public override void OnDeactivate(GameObject target)
    {
        ReportBulldozing(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleAbility : Ability2
{
    bool invisible = false;
    public event Action<bool> ReportInvisible;

    [SerializeField]
    Material material;

    public override string GetAbilityName()
    {
        return "Invisible";
    }

    public override void OnActivate(GameObject target)
    {
        invisible = true;
        //you will get an error here if you don't add any enemies into the scene, which
        //is the whole point of the invisible ability.
        ReportInvisible(invisible);
        target.GetComponent<MeshRenderer>().material = material;
    }

    public override void OnDeactivate(GameObject target)
    {
        invisible = false;
        ReportInvisible(invisible);
    }
}

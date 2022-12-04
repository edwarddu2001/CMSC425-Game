using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoAbility2 : Ability2
{
    public Material material;

    public override string GetAbilityName()
    {
        return "Lightup";
    }

    public override void OnActivate(GameObject target)
    {
         target.GetComponent<MeshRenderer>().material = material;
    }
    public override void OnDeactivate(GameObject target)
    {
        //do nothing...
    }
}

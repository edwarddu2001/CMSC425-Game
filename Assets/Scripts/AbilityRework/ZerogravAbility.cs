using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZerogravAbility : Ability2
{
    public Material material;

    public override string GetAbilityName()
    {
        return "ZeroGrav";
    }

    public override void OnActivate(GameObject target)
    {
        //TODO: add zero gravity.
        target.GetComponent<MeshRenderer>().material = material;
        Debug.Log("TODO: Add Zero Gravity");

    }
    public override void OnDeactivate(GameObject target)
    {
        //TODO: remove zero gravity.
        Debug.Log("TODO: Undo Zero Gravity");
    }
}

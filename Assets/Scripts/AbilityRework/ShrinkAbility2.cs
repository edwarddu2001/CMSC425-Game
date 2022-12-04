using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkAbility2 : Ability2
{
    [SerializeField]
    public bool isShrunk = false;
    public float shrinkProportion = .6f;
    public Material material;

    public override string GetAbilityName()
    {
        return "Shrink";
    }

    public override void OnActivate(GameObject target)
    {
        if (!isShrunk)
        {

            target.transform.localScale = new Vector3(shrinkProportion, shrinkProportion, shrinkProportion);
            isShrunk = true;
            target.GetComponent<MeshRenderer>().material = material;

        }

    }
    public override void OnDeactivate(GameObject target)
    {
        if (isShrunk)
        {

            target.transform.localScale = new Vector3(1, 1, 1);
            isShrunk = false;

        }

    }
}

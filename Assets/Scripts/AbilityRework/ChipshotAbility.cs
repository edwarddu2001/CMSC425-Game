using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*CHIPSHOT ABILITY: now we're talking
action: allows you to hit the ball "in air" off your shot, as if you had a wedge or some club other than a putter!
pros: 
  - use it to jump over obstacles and chasms
cons:
  - ???
*/
public class ChipshotAbility : Ability2
{
    [SerializeField]
    public float maxChippingRange;
    public Material material;

    public override string GetAbilityName()
    {
        return "Chipshot";
    }

    public override void OnActivate(GameObject target)
    {
        //TODO: add UI here.
        Debug.Log("TODO: Add Chipshot UI");
        target.GetComponent<MeshRenderer>().material = material;

    }
    public override void OnDeactivate(GameObject target)
    {
        //TODO: Remove UI here.
        Debug.Log("TODO: Remove Chipshot UI");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*ZERO GRAVITY ABILITY: i believe i can fly!
action: disables gravity
pros: 
  - no gravity! :D
cons:
  - no gravity? D:
*/
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

        Rigidbody rbody = target.GetComponent<Rigidbody>();
        rbody.useGravity = false;

    }
    public override void OnDeactivate(GameObject target)
    {
        //TODO: remove zero gravity.

        Rigidbody rbody = target.GetComponent<Rigidbody>();
        rbody.useGravity = true;
    }
}

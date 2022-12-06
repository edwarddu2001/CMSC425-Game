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
    private MoveGolf mg;
    private float oldJumpHeight;

    public override string GetAbilityName()
    {
        return "ZeroGrav";
    }

    public override void OnActivate(GameObject target)
    {
        target.GetComponent<MeshRenderer>().material = material;

        //disable jumping
        mg = target.GetComponent<MoveGolf>();
        oldJumpHeight = mg.jumpHeight;
        mg.jumpHeight = 0;

        //turn off gravity for this ball
        Rigidbody rbody = target.GetComponent<Rigidbody>();
        rbody.useGravity = false;

    }
    public override void OnDeactivate(GameObject target)
    {
        mg.jumpHeight = oldJumpHeight;

        Rigidbody rbody = target.GetComponent<Rigidbody>();
        rbody.useGravity = true;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*SHRINK ABILITY: it's a small world
action: decreases size of the golf ball, by the value specified by "shrink proportion".
pros: 
  - go through small areas that'd be too big for any other ability
cons:
  - slightly slower movement speed
  - slightly decreased jump height
*/
public class ShrinkAbility2 : Ability2
{
    [SerializeField]
    public bool isShrunk = false;
    public float shrinkProportion = .6f;
    public float maxSpeedDecreaseFactor = 0.8f;
    public float frictionIncreaseFactor = 2f;
    public Material material;

    public override string GetAbilityName()
    {
        return "Shrink";
    }

    //activate: shrink the physical ball itself, and decrease move speed.
    public override void OnActivate(GameObject target)
    {
        target.transform.localScale = new Vector3(shrinkProportion, shrinkProportion, shrinkProportion);
        isShrunk = true;
        target.GetComponent<MeshRenderer>().material = material;

        //decrease the maximum power with which you can hit the ball.
        MoveGolf movement = target.GetComponent<MoveGolf>();
        movement.maxSpeedFactor *= maxSpeedDecreaseFactor;

        //another thing: to make the ball appear to move slower, we'll increase its friction
        target.GetComponent<SphereCollider>().material.dynamicFriction *= frictionIncreaseFactor;

    }

    //deactivate: undo everything
    public override void OnDeactivate(GameObject target)
    {
        
        target.transform.localScale = new Vector3(1, 1, 1);
        isShrunk = false;

        MoveGolf movement = target.GetComponent<MoveGolf>();
        movement.maxSpeedFactor /= maxSpeedDecreaseFactor;

        target.GetComponent<SphereCollider>().material.dynamicFriction /= frictionIncreaseFactor;

    }
}

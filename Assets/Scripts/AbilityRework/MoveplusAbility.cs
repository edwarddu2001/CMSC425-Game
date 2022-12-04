using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*MOVEMENT + ABILITY: a pro golfer's dream
action: does all of the following to expand what the player can do with their shot:
  - increases the maximum power you can hit the ball with.
  - increases the rate at which you stop the ball with "R" while it's rolling.
  - allows you to FULL STOP when you press "F" while the ball is rolling, which almost completely stops the ball's motion.
  - allows you to put "spin" on the ball with WASD while it's rolling, but only for a few seconds after it's hit.
pros: 
  - greatly increases what you can do with the ball on the ground
cons:
  - disables jumping entirely.
  
notes: this ability is very powerful, but typically only given to you when you reeeeally need it.
*/
public class MoveplusAbility : Ability2
{
    //these can be changed per each ability pickup object, in the inspector.
    public float maxPowerIncrease = 1.5f;
    public float stopRateIncrease = 2.0f;
    public float spinRate = 0.3f;
    public float spinTime = 5.0f;

    private float oldJumpHeight;
    public Material material;

    public override string GetAbilityName()
    {
        return "Movement+";
    }

    public override void OnActivate(GameObject target)
    {
        //Here, we change around some values of the player's movement capabilities!
        target.GetComponent<MeshRenderer>().material = material;

        MoveGolf movement = target.GetComponent<MoveGolf>();

        movement.maxSpeedFactor *= maxPowerIncrease;
        movement.stoppingRate /= stopRateIncrease;
        movement.spinRate = spinRate;

        //but we also take away jumping...
        oldJumpHeight = movement.jumpHeight;
        movement.jumpHeight = 0;

        //as for Full stopping (F key), and spin (WASD), that'll be handled in the MoveGolf script itself.

    }

    //the usual for deactivate, just undo everything.
    public override void OnDeactivate(GameObject target)
    {
        MoveGolf movement = target.GetComponent<MoveGolf>();

        movement.maxSpeedFactor /= maxPowerIncrease;
        movement.stoppingRate *= stopRateIncrease;
        movement.jumpHeight = oldJumpHeight;
        movement.spinRate = 0;
    }
}

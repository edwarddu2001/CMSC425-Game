using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*LABYRINTH ABILITY: a game of its own
action: when taking a shot, press "F" to enable labyrinth mode. in labyrinth mode, you can rotate the entire map.
    some more notes about this:
      - if you die, the map will be oriented back to its normal position (even if you aren't in labyrinth mode.)
      - all ability pickup objects disabled in labyrinth mode.
      - 
pros: 
  - no gravity! :D
cons:
  - the player's shot stats (max speed, ball movement, etc.) become worse in nearly every category.
  - this discourages the player from taking normal shots when they get this ability.
*/
public class LabyrinthAbility : Ability2
{
    //public event Action<GameObject> reportRotatableObject;
    public GameObject courseContainer;
    public Material material;

    public float maxPowerDecrease = 1.5f;
    public float frictionIncrease = 2.0f;
    public float jumpHeightDecrease = 2.0f;
    private float oldStoppingRate;

    public override string GetAbilityName()
    {
        return "Labyrinth";
    }

    public override void OnActivate(GameObject target)
    {
        target.GetComponent<MeshRenderer>().material = material;

        target.GetComponent<SphereCollider>().material.dynamicFriction *= frictionIncrease;
        MoveGolf movement = target.GetComponent<MoveGolf>();
        movement.maxSpeedFactor /= maxPowerDecrease;
        movement.defaultSpeed /= maxPowerDecrease;
        movement.jumpHeight /= jumpHeightDecrease;

        oldStoppingRate = movement.stoppingRate;
        movement.stoppingRate = 1.0f;


        //TODO: add labyrinth rotation.
        //courseContainer.GetComponent<LabyrinthRotate>().enabled = true;
        //reportRotatableObject(courseContainer);
        target.GetComponent<MoveGolf>().setRotatableObject(courseContainer);

    }
    public override void OnDeactivate(GameObject target)
    {
        //TODO: Remove labyrinth rotation.
        //Debug.Log("TODO: Undo Labyrinth Rotation");
        //courseContainer.GetComponent<LabyrinthRotate>().enabled = false;

        target.GetComponent<SphereCollider>().material.dynamicFriction /= frictionIncrease;
        MoveGolf movement = target.GetComponent<MoveGolf>();
        movement.maxSpeedFactor *= maxPowerDecrease;
        movement.defaultSpeed *= maxPowerDecrease;
        movement.jumpHeight *= jumpHeightDecrease;

        movement.stoppingRate = oldStoppingRate;

    }
}

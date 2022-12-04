using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LabyrinthAbility : Ability2
{
    public GameObject courseContainer;
    public Material material;

    public override string GetAbilityName()
    {
        return "Labyrinth";
    }

    public override void OnActivate(GameObject target)
    {
        target.GetComponent<MeshRenderer>().material = material;
        //TODO: add labyrinth rotation.
        Debug.Log("TODO: Add Labyrinth Rotation");

    }
    public override void OnDeactivate(GameObject target)
    {
        //TODO: Remove labyrinth rotation.
        Debug.Log("TODO: Undo Labyrinth Rotation");
    }
}

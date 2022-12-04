using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*BULLDOZER ABILITY: better red than dead
action: allows you to destroy red objects, which are otherwise impassable.
pros: 
  - open up new paths that are completely blocked off otherwise.
cons:
  - significantly slower movement speed
  - significantly slower jump height
*/
public class BulldozerAbility : Ability2
{
    [SerializeField]
    public Material material;
    public event Action<bool> ReportBulldozing;

    public override string GetAbilityName()
    {
        return "Bulldozer";
    }

    public override void OnActivate(GameObject target)
    {
        //you will get an error here if you don't add any destroyable objects into the scene, which
        //is the whole point of the bulldozer ability.
        ReportBulldozing(true);
        target.GetComponent<MeshRenderer>().material = material;

    }
    public override void OnDeactivate(GameObject target)
    {
        ReportBulldozing(false);
    }
}

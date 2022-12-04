using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*LIGHTUP ABILITY: brighten your horizons
action: emits a pale glow from the ball, and reveals secret paths of the same color as your light.
pros: 
  - open up new paths you otherwise couldn't see!
cons:
  - no inherent cons, however, this ability is quite specific.
  - also, the platforms will vanish when you lose this ability.
*/
public class LightupAbility2 : Ability2
{
    // public event Action<bool> ReportActivate;
    public event Action<bool> ReportLit;
    public bool litUp = false;
    public Material material;

    public override string GetAbilityName()
    {
        return "Lightup";
    }

    public override void OnActivate(GameObject target)
    {
        litUp = true;
        //you will get an error here if you don't add any revealable objects into the scene, which
        //is the whole point of the lightup ability.
        ReportLit(litUp);
        target.GetComponent<MeshRenderer>().material = material;
    }
    public override void OnDeactivate(GameObject target)
    {
        litUp = false;
        ReportLit(litUp);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability2 : MonoBehaviour
{
    public abstract string GetAbilityName();
    //what happens when the ability is activated
    public abstract void OnActivate(GameObject target);
    //what happens when the ability is deactivated
    public abstract void OnDeactivate(GameObject target);
    /*
        note that the UI should handle edge cases of activating
        something that has already been activated
    */
}

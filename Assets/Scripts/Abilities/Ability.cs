using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Ability : MonoBehaviour
{
    //what happens when the ability is activated
    public abstract void OnActivate();
    //what happens when the ability is deactivated
    public abstract void OnDeactivate();
    /*
        note that the UI should handle edge cases of activating
        something that has already been activated
    */
}

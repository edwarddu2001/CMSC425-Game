using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbObserver2 : MonoBehaviour
{
    //the controller that we are observing.
    //If there isn't one, it's safe to leave it as null
    /*[SerializeField]
    private AbilityController abilityController;*/

    //the ability this Observer is associated with
    //(Just drag the associated script from the player in here)
    [SerializeField]
    public Ability2 ability;

    void Start()
    {
        
    }

    public void ActivateNewAbility()
    {
        ability.OnActivate(this.gameObject);
    }

    public void DeactivateOldAbility()
    {
        ability.OnDeactivate(this.gameObject);
    }
}

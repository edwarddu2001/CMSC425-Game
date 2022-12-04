using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Ability Pickup: Put this script onto an ability coin, or any other object with a trigger. When the player hits it, they get
 the ability you provide to it in the inspector.*/
public class AbilityPickup2 : MonoBehaviour
{
    public Ability2 ability;
    private AudioSource pickupSound;
    void Start()
    {
        pickupSound = this.GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider thing)
    {
        //only care about player collisions.
        if (string.Equals(thing.gameObject.tag, "Player"))
        {
            GameObject player = thing.transform.gameObject;
            AbObserver2 observer = player.GetComponent<AbObserver2>();

            //only care if the ability is something different than what we already have...
            //this works because "no ability" is considered an ability too
            if (!observer.ability.GetAbilityName().Equals(ability.GetAbilityName()))
            {
                //deactivate the old ability and activate the new one. easy!
                player.GetComponent<AbObserver2>().DeactivateOldAbility();
                player.GetComponent<AbObserver2>().ability = ability;
                player.GetComponent<AbObserver2>().ActivateNewAbility();

                pickupSound.Play();
                
            }


        }

    }
}

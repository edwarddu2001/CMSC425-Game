using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityPickup2 : MonoBehaviour
{
    public Ability2 ability;
    private AudioSource pickupSound;
    private bool hasPlayed = false;
    void Start()
    {
        pickupSound = this.GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider thing)
    {
        if (string.Equals(thing.gameObject.tag, "Player"))
        {
            GameObject player = thing.transform.gameObject;
            player.GetComponent<AbObserver2>().DeactivateOldAbility();
            player.GetComponent<AbObserver2>().ability = ability;
            //do something for repickups?
            
            player.GetComponent<AbObserver2>().ActivateNewAbility();

            if (!hasPlayed)
            {
                pickupSound.Play();
                hasPlayed = true;
            }


        }

    }
}

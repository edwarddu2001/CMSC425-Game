using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*Ability Pickup: Put this script onto an ability coin, or any other object with a trigger. When the player hits it, they get
 the ability you provide to it in the inspector.*/
public class AbilityPickup2 : MonoBehaviour
{
    public Ability2 ability;
    public bool enabledPickups;

    private AudioSource pickupSound;
    private MeshRenderer mr;
    private Color enabledColor;
    private Color disabledColor;

    void Start()
    {
        enabledPickups = true;

        pickupSound = this.GetComponent<AudioSource>();
        mr = GetComponent<MeshRenderer>();
        enabledColor = new Color(mr.material.color.r, mr.material.color.g, mr.material.color.b, 1.0f);
        disabledColor = new Color(enabledColor.r, enabledColor.g, enabledColor.b, 0.5f);
    }

    void OnTriggerEnter(Collider thing)
    {
        if (enabledPickups)
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

    public void disablePickup()
    {
        Debug.Log("Disabled pickup for: " + name);
        enabledPickups = false;
        mr.material.color = disabledColor;
    }

    public void enablePickup()
    {
        enabledPickups = true;
        mr.material.color = enabledColor;
    }
}

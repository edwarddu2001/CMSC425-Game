using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    One of these goes on the player per ability in the game.
    This class tells the ability when to (de)activate
    based on the controller it is observing.
*/
public class AbilityObserver : MonoBehaviour
{
    //the controller that we are observing.
    //If there isn't one, it's safe to leave it as null
    [SerializeField]
    private AbilityController abilityController;
    
    //the ability this Observer is associated with
    //(Just drag the associated script from the player in here)
    [SerializeField]
    private Ability ability;
    
    void Start()
    {
        //make sure there is a controller for this ability in the scene
        if (abilityController != null){
            abilityController.ReportActivate += ChangeActive;
        }
    }

    private void ChangeActive(bool activating){
        if (activating){
            ability.OnActivate();
        } else {
            ability.OnDeactivate();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AbilityObserver : MonoBehaviour
{
    //the controller that we are observing
    [SerializeField]
    private AbilityController abilityController;
    [SerializeField]
    private Ability ability;
    
    void Start()
    {
        abilityController.ReportActivate += ChangeActive;
    }

    private void ChangeActive(bool activating){
        if (activating){
            ability.OnActivate();
        } else {
            ability.OnDeactivate();
        }
    }
}

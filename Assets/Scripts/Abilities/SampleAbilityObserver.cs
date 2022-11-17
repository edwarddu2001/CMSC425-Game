using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SampleAbilityObserver : MonoBehaviour
{
    public Ability ability;
    [SerializeField]
    void Start()
    {
        ability.ReportActivate += ChangeActive;
    }

    private void ChangeActive(bool activating){
        if (activating){
            //turn on ability
        } else {
            //turn off ability
        }
    }
}

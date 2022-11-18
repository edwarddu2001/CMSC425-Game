using System;
using System.Collections;
using UnityEngine;

/*
    This goes on an empty object in the scene.
    There should be one of those for each ability 
    available in the scene
*/
public class AbilityController : MonoBehaviour{
    public event Action<bool> ReportActivate; 
    private bool isActive = false;
    public void ChangeActive(bool active){
        //this line checks if are we are
        //actually changing the state
        if(active != isActive){
            ReportActivate(active);
            active = isActive;
        }
    }
}
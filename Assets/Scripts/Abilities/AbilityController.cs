using System;
using System.Collections;
using UnityEngine;

public class AbilityController : MonoBehaviour{
    public event Action<bool> ReportActivate; 
    private bool isActive = false;
    public void ChangeActive(bool active){
        if(active != isActive){
            ReportActivate(active);
            active = isActive;
        }
    }



}
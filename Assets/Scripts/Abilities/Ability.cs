using System;
using System.Collections;
using UnityEngine;

public class Ability : MonoBehaviour{
    public event Action<bool> ReportActivate; 
    
    public void ChangeActive(bool active){
        ReportActivate(active);
    }

}
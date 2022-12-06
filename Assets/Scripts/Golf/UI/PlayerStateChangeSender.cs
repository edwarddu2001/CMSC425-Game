using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateChangeSender : MonoBehaviour
{
    public event Action<bool, Ability2> reportChangedState;

    public PlayerStateChangeSender()
    {

    }

    public void reportState(bool inMotion, Ability2 ability)
    {
        if (reportChangedState != null)
        {
            reportChangedState(inMotion, ability);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

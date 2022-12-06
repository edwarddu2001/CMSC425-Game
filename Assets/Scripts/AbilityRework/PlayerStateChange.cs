using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*use whenever the player changes its STATE and we want to notify other things, such as UI.
  two important parts of state:
    - is the ball in motion?
    - what is the player's ability?*/
public class PlayerStateChange
{
    //for UI and other listeners
    public event Action<bool, Ability2> reportChangedState;

    public PlayerStateChange()
    {

    }

    public void reportState(bool inMotion, Ability2 ability)
    {
        if (reportChangedState != null)
        {
            reportChangedState(inMotion, ability);
        }
    }
//send out a message to those that are listening (such as UI)
//if we picked up a new ability, logically, we know the ball must be in motion, so pass in true...
/*if (pickedUpAbility != null)
{
    pickedUpAbility(true, ability);
}*/
}

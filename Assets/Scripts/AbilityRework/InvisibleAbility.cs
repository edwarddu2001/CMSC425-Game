using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleAbility : Ability2
{
    private bool invisible = false;
    private float timeRemaining = 15;
    public event Action<bool> ReportInvisible;

    [SerializeField]
    private Material material;
    [SerializeField]
    private GameObject player;

    void Update()
    {
        if (invisible)
        {
            timeRemaining -= Time.deltaTime;
            if (timeRemaining <= 0)
            {
                TimeRanOut(player);
            }
        }
    }

    public override string GetAbilityName()
    {
        return "Invisible";
    }

    public override void OnActivate(GameObject target)
    {
        //you will get an error here if you don't add any enemies into the scene, which
        //is the whole point of the invisible ability.
        invisible = true;
        ReportInvisible(invisible);

        target.GetComponent<MeshRenderer>().material = material;
        timeRemaining = 15;
    }

    void TimeRanOut(GameObject target)
    {
        OnDeactivate(target);

        MoveGolf mg = target.GetComponent<MoveGolf>();
        AbObserver2 observer = target.GetComponent<AbObserver2>();

        NoAbility2 noAbility = target.GetComponent<NoAbility2>();

        mg.reportChangeInState(true, noAbility);
        target.GetComponent<AbObserver2>().ability = noAbility;
        target.GetComponent<AbObserver2>().ActivateNewAbility();
    }

    public string getTimeRemaining()
    {
        return ((int)timeRemaining).ToString();
    }

    public override void OnDeactivate(GameObject target)
    {
        invisible = false;
        ReportInvisible(invisible);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*NO ABILITY: who needs 'em anyway?
by considering the lack of an ability an "Ability2" type object, we can make some logical
     comparisons a lot easier.*/
public class NoAbility2 : Ability2
{
    public Material material;

    public override string GetAbilityName()
    {
        return "Nothing";
    }

    public override void OnActivate(GameObject target)
    {
         target.GetComponent<MeshRenderer>().material = material;
    }
    public override void OnDeactivate(GameObject target)
    {
        //do nothing...
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkAbility : Ability
{

	[SerializeField]
	private GameObject obj;
	private bool isShrunk = false;
	public float shrinkProportion = .8f;


	public override void OnActivate(){
        if(!isShrunk){
			
				obj.transform.localScale = new Vector3(shrinkProportion,shrinkProportion,shrinkProportion);
				isShrunk = true;
			
		}

    }
    public override void OnDeactivate(){
        if(isShrunk){
			
				obj.transform.localScale = new Vector3(1,1,1);
				isShrunk = false;
		
		}
		
    }

	

}


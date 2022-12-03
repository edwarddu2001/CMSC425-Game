using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrinkAbility : Ability
{

	[SerializeField]
	private GameObject obj;
	[SerializeField]
	public bool isShrunk = false;
	public float shrinkProportion = .6f;
	public Material material;



	public override void OnActivate(){
        if(!isShrunk){
			
				obj.transform.localScale = new Vector3(shrinkProportion,shrinkProportion,shrinkProportion);
				isShrunk = true;
				obj.GetComponent<MeshRenderer>().material = material;
			
		}

    }
    public override void OnDeactivate(){
        if(isShrunk){
			
				obj.transform.localScale = new Vector3(1,1,1);
				isShrunk = false;
		
		}
		
    }

	

}


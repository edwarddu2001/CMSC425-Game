using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrink : MonoBehaviour
{

	[SerializeField]
	private GameObject obj;
	private bool isShrunk = false;
	public float shrinkProportion = .8f;
	Mesh mesh;

	// Update is called once per frame
	private void OnTriggerEnter(Collider other) {
		if(!isShrunk){
			if (other.gameObject.tag == "Shrink") {
				obj.transform.localScale = new Vector3(shrinkProportion,shrinkProportion,shrinkProportion);
				isShrunk = true;
			}
		}
	}

	

}


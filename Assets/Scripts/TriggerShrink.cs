using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerShrink : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
 	{
		if (other.gameObject.tag == "Player")
		{
			Shrink myScript = other.gameObject.GetComponent<Shrink>();
			//You could check if you really attached MyScript here, but I think we skip that. ;-)
			myScript.enabled = true;
		}
 	}
 
 	void OnTriggerExit(Collider other)
 	{
		if (other.gameObject.tag == "Player")
		{
			Shrink myScript = other.gameObject.GetComponent<Shrink>();
			myScript.enabled = false;
			
		}
 	}
}

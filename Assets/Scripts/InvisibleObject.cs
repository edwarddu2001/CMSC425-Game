using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleObject : MonoBehaviour
{
    [SerializeField]
    private LightUpAbility lightUpAbility;
    private MeshRenderer mesh;
    private GameObject obj;
    void Start(){
        mesh = GetComponent<MeshRenderer>();
        obj = GameObject.Find("InvisCube");
        //make sure that any object you want to "light up" are called "InvisCube" 
        //the name must be exact for it to work

        //make sure there is a controller for this ability in the scene
        if (lightUpAbility != null){
            lightUpAbility.ReportLit += MakeVisible;
        }
        mesh.enabled = false;
        obj.SetActive(false);
    
    }

    void MakeVisible(bool visible){
        mesh.enabled = visible;
        obj.SetActive(true);
    }
}

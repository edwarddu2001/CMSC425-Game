using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvisibleObject2 : MonoBehaviour
{
    [SerializeField]
    private LightupAbility2 lightUpAbility;
    private MeshRenderer mesh;

    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        //make sure that any object you want to "light up" are called "InvisCube" 
        //the name must be exact for it to work

        //make sure there is a controller for this ability in the scene
        if (lightUpAbility != null)
        {
            lightUpAbility.ReportLit += MakeVisible;
        }
        mesh.enabled = false;
        this.transform.gameObject.SetActive(false);

    }

    void MakeVisible(bool visible)
    {
        mesh.enabled = visible;
        this.transform.gameObject.SetActive(true);
    }
}

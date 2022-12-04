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

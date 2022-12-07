using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour
{
    public float riseHeight;
    public float secondsToRise;
    public GameObject fullElevator;
    public MeshRenderer indicatorLight;

    private Vector3 trans;
    private float timeLeftElevator;
    private float standardHeight;
    float change;
    [SerializeField]
    private bool changing, rising;

    // Start is called before the first frame update
    void Start()
    {
        trans = new Vector3(fullElevator.transform.position.x, fullElevator.transform.position.y, fullElevator.transform.position.z);
        standardHeight = fullElevator.transform.position.y;
        rising = true;
        changing = false;
    }

    // Update is called once per frame
    void Update()
    {
        timeLeftElevator = timeLeftElevator + Time.deltaTime;
        if (changing && timeLeftElevator > 3.0f)
        {
            indicatorLight.material.color = Color.green;
            change = riseHeight * (Time.deltaTime / secondsToRise);
            if (rising)
            {
                risingElevator(change);
            } else
            {
                lowerElevator(change);
            }
        } else if(changing)
        {
            indicatorLight.material.color = Color.yellow;
        } else
        {
            indicatorLight.material.color = Color.red;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            timeLeftElevator = 0;
            changing = true;
            rising = true;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            timeLeftElevator = 0;
            changing = true;
            rising = false;
        }
    }

    private void risingElevator(float delta)
    {
        trans.y = trans.y + delta;
        if (trans.y > standardHeight + riseHeight)
        {
            changing = false;
            trans.y = standardHeight + riseHeight;
        }

        fullElevator.transform.position = trans;
    }

    private void lowerElevator(float delta)
    {
        trans.y = trans.y - delta;
        if (trans.y < standardHeight)
        {
            changing = false;
            trans.y = standardHeight;
        }

        fullElevator.transform.position = trans;
    }

}

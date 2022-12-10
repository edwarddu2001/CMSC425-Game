using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapeMenuUI : MonoBehaviour
{
    Canvas c;
    // Start is called before the first frame update
    void Start()
    {
        c = gameObject.GetComponent<Canvas>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            c.enabled = !c.enabled;
            if (c.enabled)
            {
                //Destroy(FindObjectOfType<ThirdPersonCamera>());
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{   
    public GameObject inventory;
    bool isActive = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Toggle Inventory")) {
            if (isActive == true) {
                inventory.SetActive(false);
                isActive = false;}
            else {
                inventory.SetActive(true);
                isActive = true;              
              }
        }
    }
}

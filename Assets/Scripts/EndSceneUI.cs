using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndSceneUI : MonoBehaviour
{
    void Start () {
        TextMeshProUGUI tmp = gameObject.GetComponent<TextMeshProUGUI>();
        tmp.text = PlayerPrefs.GetInt("courseScore").ToString();
    }
     

}
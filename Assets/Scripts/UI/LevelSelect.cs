using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelect : MonoBehaviour
{
    void Start () {
        menu.SetActive(false);
    }
    public GameObject menu;
    public void showLevelSelectMenu() {
        menu.SetActive(!menu.activeSelf);
    }
}

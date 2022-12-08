using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultValues : MonoBehaviour
{
    public void default2() {
        PlayerPrefs.SetInt("courseScore", 0);
        PlayerPrefs.SetInt("currHole", 0);
        PlayerPrefs.SetString("holeByHole", "");
    }
}

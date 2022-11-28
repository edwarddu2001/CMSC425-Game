using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject timeElapsed;
    public GameObject levelName;
    public GameObject currentAbility;
    public GameObject abObserver;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed.GetComponent<TextMeshProUGUI>().SetText("Time: " + (int) Time.timeSinceLevelLoad);
        levelName.GetComponent<TextMeshProUGUI>().SetText(SceneManager.GetActiveScene().name);
        currentAbility.GetComponent<TextMeshProUGUI>().SetText(abObserver.GetComponent<AbilityObserver>().ability.ToString());


        // ability name

    }
}

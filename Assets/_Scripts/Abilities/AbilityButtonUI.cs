using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityButtonUI : MonoBehaviour
{
    // Controller 0, keyboard 1
    public GameObject[] buttons;

    // Start is called before the first frame update
    void Start()
    {

        if (GameManager.Instance.controllerActive)
        {
            buttons[0].SetActive(true);
            buttons[1].SetActive(false);
        }
        else
        {
            buttons[0].SetActive(false);
            buttons[1].SetActive(true);
        }

    }

}

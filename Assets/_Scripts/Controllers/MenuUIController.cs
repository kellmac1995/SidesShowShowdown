using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIController : MonoBehaviour {


    private float val;

    private bool inMenu;

    public GameObject escMenu;

    // Update is called once per frame
    void Update () {
		
        if (!inMenu && Input.anyKeyDown) 
        {
            if (Input.GetButton("Start"))
            {
                inMenu = true;
                escMenu.SetActive(true);
            }
            else
            {

                GameManager.Instance.LoadIntroScene();

            }

        }
        
    }


    public void Back_OnClick()
    {
        escMenu.SetActive(false);
        StartCoroutine(InputDelay());
    }

    public void NewGame_OnClick()
    {
        PlayerPrefs.DeleteAll();
    }

    public void ExitGame_OnClick()
    {
        Application.Quit();
    }

    IEnumerator InputDelay()
    {
        yield return new WaitForSeconds(1);
        inMenu = false;

    }

}

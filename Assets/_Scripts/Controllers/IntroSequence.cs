using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class IntroSequence : MonoBehaviour
{
    public GameObject fadeout_Panel;

    public float skipDelay = 1.5f;
    public float sequenceLength = 16f;

    private void Start()
    {
        StartCoroutine(EnterGame());
    }

    // Update is called once per frame
    void Update()
    {
        //if any input skip the game
        if (Input.anyKeyDown)
        {
            Debug.Log("Skip!");
            fadeout_Panel.SetActive(true);
            StartCoroutine(Skip());
        }
    }

    //SKIPPED:Wait seconds before loading next scene
    IEnumerator Skip()
    {
        yield return new WaitForSeconds(skipDelay);
        StopAllCoroutines();
        GameManager.Instance.LoadLoadingScene();
    }

    //AUTO: Wait seconds before loading next scene
    IEnumerator EnterGame()
    {
        yield return new WaitForSeconds(sequenceLength);
        fadeout_Panel.SetActive(true);
        StartCoroutine(Skip());
    }
}

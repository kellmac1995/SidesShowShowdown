using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class FadeToNextScene : MonoBehaviour
{
    public GameObject fadeout_Panel;

    public string levelToLoadName;
    public float sequenceLength = 4f;
    public float loadDelay = 1f;


    private void Start()
    {
        StartCoroutine(LoadNextScene());
    }

    //AUTO: Wait seconds before loading next scene
    IEnumerator LoadNextScene()
    {
        //TODO load async then after loadDelay load scene
        yield return new WaitForSeconds(sequenceLength);
        fadeout_Panel.SetActive(true);
        yield return new WaitForSeconds(loadDelay);
        SceneManager.LoadScene(levelToLoadName);
    }
}

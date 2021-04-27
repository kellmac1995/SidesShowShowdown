using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader_Loading : SceneLoader
{

    public float loadTime = 2f;

    public override void Start()
    {

        base.Start();

        GameManager.Instance.InitLoadScene();

        StartCoroutine(LoadArena());

    }


    IEnumerator LoadArena()
    {

        yield return new WaitForSeconds(loadTime);

        GameSceneManager.LoadScene(GameSceneManager.Scenes.Arena);


    }

}

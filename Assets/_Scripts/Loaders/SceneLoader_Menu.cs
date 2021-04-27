using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader_Menu : SceneLoader
{

    public override void Start()
    {

        base.Start();

        GameManager.Instance.InitMenuScene();

    }

}

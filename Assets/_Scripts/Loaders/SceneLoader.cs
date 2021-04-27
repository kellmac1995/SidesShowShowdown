using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoader : MonoBehaviour
{


    public virtual void Start()
    {

        if (!FindObjectOfType<GameManager>())
            CreateManagerInstances();

        GameManager.Instance.cameraShaker.mainCam = Camera.main;


    }


    /// <summary>
    /// Create instances of the managers.
    /// </summary>
    void CreateManagerInstances()
    {

        GameManager.Instance.Awake();
        AudioManager.Instance.Awake();
        UIManager.Instance.Awake();

    }


}

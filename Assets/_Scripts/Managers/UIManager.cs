using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : GenericSingletonClass<UIManager>
{

  
    //Instances
    public UIController UIController;


    // Gameobjects
    public Image playerDamagedFlash;
    public GameObject playerSpeedVignette; 
    public GameObject restartButton;
    public GameObject shopDoor;
    public GameObject arenaCover;
    public GameObject arenaObjects;
    public GameObject endGameObjects;
    public GameObject timeUpText;




    // Panels
    public GameObject pauseMenuPanel;
    //public GameObject endGamePanel;
    public GameObject flashscreenPanel;
    public GameObject settingsPanel;
    //public GameObject enterGamePanel;


    // Text Objects
    public Text timerText;
    public Text endGameText;
    public Text scoreUI;
    public Text totalScoreUI;


    // Animators
    public Animator characterAnimator;
    public Animator timerFrame;
    public Animator camAnimator;
    public Animator vignetteAnimator;



    // Canvas group
    public CanvasGroup familyGroup;


    public override void Awake()
    {

        isPersistent = true;
        base.Awake();
        GameManager.Instance.uIManagerInstance = this;

    }

}

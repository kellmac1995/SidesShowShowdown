using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoader_Shop : SceneLoader
{

    [Space]
    [Header("-Instances-")]
    
    [Space]
    [Header("-Panels-")]
    public GameObject pauseMenuPanel;
    public GameObject settingsPanel;
    //public GameObject enterGamePanel;

    [Space]
    [Header("-Animators-")]
    public Animator familyDropper;


    [Space]
    [Header("-Gameobjects-")]
    public UIController uIController;
    public GameObject buttonEvents;
    public GameObject textPopup;
    public GameObject buttonPopup;
    public GameObject despawnPoof;


    public CanvasGroup familyGroup;

    
    public GameObject[] buyImages;

    public GameObject[] tryImages;

    public GameObject[] equipImages;

    public GameObject[] backImages;

    public GameObject[] shelfImages;


    public override void Start()
    {

        base.Start();

        UIManager.Instance.UIController = uIController;

        UIManager.Instance.pauseMenuPanel = pauseMenuPanel;

        UIManager.Instance.settingsPanel = settingsPanel;

        UIManager.Instance.familyGroup = familyGroup;

        //UIManager.Instance.enterGamePanel = enterGamePanel;

        GameManager.Instance.despawnPoof = despawnPoof;

        ShopManager.Instance.familyDropper = familyDropper;

        ShopManager.Instance.buttonEvents = buttonEvents;

        ShopManager.Instance.textPopup = textPopup;

        ShopManager.Instance.buttonPopup = buttonPopup;

        ShopManager.Instance.buyButtons = buyImages;

        ShopManager.Instance.tryButtons = tryImages;

        ShopManager.Instance.equipButtons = equipImages;

        ShopManager.Instance.backButtons = backImages;

        ShopManager.Instance.shelfButtons = shelfImages;

        GameManager.Instance.InitShopScene();
        

    }

}

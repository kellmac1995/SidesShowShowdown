using UnityEngine;
using UnityEngine.UI;

public class SceneLoader_Arena : SceneLoader
{
    //Scene Objects

    [Space]
    [Header("-Instances-")]
    [Space]
    [Header("--Scene Objects--")]
    
    // Instances
    //public RangedController rangedController;
    public EnemySpawner waveManager;
    public UIController uIController;
    public ControllerViber controllerViber;

    //public TD2D_Enemy_Ranged[] rangedEnemies;

    [Space]
    [Header("-Gameobjects-")]
    // Gameobjects
    public Image playerDamagedFlash;
    public GameObject heeliesVignette; 
    public GameObject restartButton;
    public GameObject shopDoor;
    public GameObject arenaCover;
    public GameObject arenaObjects;
    public GameObject endGameObjects;
    public GameObject despawnPoof;
    public GameObject timeUpText;

    [Space]
    [Header("-Panels-")]
    // Panels
    public GameObject pauseMenuPanel;
    //public GameObject endGamePanel;
    public GameObject flashscreenPanel;
    public GameObject settingsPanel;

    [Space]
    [Header("-UI-")]
    // Text Objects
    public Text timerText;
    public Text endGameText;
    public Text scoreUI;
    public Text totalScoreUI;
    public Text fireText;
    public Text aimText;

    [Space]
    [Header("-Animators-")]
    // Animators
    public Animator characterAnimator;
    public Animator timerFrame;
    public Animator camAnimator;
    public Animator heeliesAnimator;

    [Space]    
    [Header("-Countdown Settings-")]
    [Space]
    [Header("--Gamemanager Settings--")]
    public float countDown = 60;
    public float startCountDown = 3;
    public int finalCountdown = 5;
    public int buildUpCountdown = 15;

    public GameObject shieldPickup;

    [Space]
    [Header("-Despawn Settings-")]
    public float arenaDespawnTimeMin = 0.1f;
    public float arenaDespawnTimeMax = 0.5f;
    public float enemyDespawnTimeMin = 0.1f;
    public float enemyDespawnTimeMax = 0.5f;



    /// <summary>
    /// Assign the scene objects to the game manager and call init scene.
    /// </summary>
    public override void Start()
    {

        base.Start();

        GameManager.Instance.fireText = fireText;
        GameManager.Instance.aimText = aimText;


        // instances
        UIManager.Instance.UIController = uIController;
        GameManager.Instance.enemySpawner = waveManager;

        // Gameobjects
        UIManager.Instance.playerDamagedFlash = playerDamagedFlash;
        UIManager.Instance.playerSpeedVignette = heeliesVignette;
        UIManager.Instance.restartButton = restartButton;
        UIManager.Instance.shopDoor = shopDoor;
        UIManager.Instance.arenaCover = arenaCover;
        UIManager.Instance.arenaObjects = arenaObjects;
        UIManager.Instance.endGameObjects = endGameObjects;
        UIManager.Instance.timeUpText = timeUpText;
        GameManager.Instance.despawnPoof = despawnPoof;
        GameManager.Instance.shieldPickup = shieldPickup;


    // Panels
    UIManager.Instance.pauseMenuPanel = pauseMenuPanel;
        //UIManager.Instance.endGamePanel = endGamePanel;
        UIManager.Instance.flashscreenPanel = flashscreenPanel;
        UIManager.Instance.settingsPanel = settingsPanel;


        // Text Objects
        UIManager.Instance.timerText = timerText;
        UIManager.Instance.endGameText = endGameText;
        UIManager.Instance.scoreUI = scoreUI;
        UIManager.Instance.totalScoreUI = totalScoreUI;


        // Animators
        UIManager.Instance.characterAnimator = characterAnimator;
        UIManager.Instance.timerFrame = timerFrame;
        UIManager.Instance.camAnimator = camAnimator;
        UIManager.Instance.vignetteAnimator = heeliesAnimator;


        // Settings
        GameManager.Instance.startCountDown = startCountDown;
        GameManager.Instance.countDown = countDown;
        GameManager.Instance.finalCountdown = finalCountdown;
        GameManager.Instance.buildUpCountdown = buildUpCountdown;

        GameManager.Instance.enemyDespawnTimeMin = enemyDespawnTimeMin;
        GameManager.Instance.enemyDespawnTimeMax = enemyDespawnTimeMax;
        GameManager.Instance.arenaDespawnTimeMin = arenaDespawnTimeMin;
        GameManager.Instance.arenaDespawnTimeMax = arenaDespawnTimeMax;


        // Initalize the arena scene
        GameManager.Instance.InitArenaScene();

    }
}

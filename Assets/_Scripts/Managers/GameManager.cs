using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : GenericSingletonClass<GameManager>
{

    #region Public Variables


    [HideInInspector]
    public int roundTickets;
    [HideInInspector]
    public int totalTickets;



    public UnityEngine.UI.Text fireText;
    public UnityEngine.UI.Text aimText;
    public GameObject despawnPoof;



    [Header("Status")]
    public bool isLoaded = false;
    public bool musicIsLoaded = false;
    public bool controllerActive = false;
    public bool isPaused = false;
    public bool isPlaying = false;
    public bool isShopping = false;
    public bool bearPurchased = false;


    [Header("Stats")]
    public float killsPerSecond = 0;
    public float roundKillsPerSecond = 0;
    public int secondKillsDucks = 0;
    public int roundKillsDucks = 0;
    public int secondKillsPhants = 0;
    public int roundKillsPhants = 0;


    [HideInInspector]
    public float currKPSCounter = 1;

    public float secondsSinceTookDamge = 0;

    public int killsSinceTookDamage = 0;

    public int highestKillsBeforeTookDamage = 0;


    [HideInInspector]
    public float currCountDown = 60;

    public int highScore = 0;


    [HideInInspector]
    public CameraShaker cameraShaker;
    [HideInInspector]
    public CameraZoom cameraZoom;
    [HideInInspector]
    public FrameFreezer freezer;
    [HideInInspector]
    public ControllerViber controllerViber;
    [HideInInspector]
    public TimeManager timeManager;
    [HideInInspector]
    public EnemySpawner enemySpawner;
    [HideInInspector]
    public UIManager uIManagerInstance;


    public bool pauseBool;
    public bool roundEnded;


    [Header("Settings")]
    public bool loadShopValues = true;
    public float countDown = 60;
    public float startCountDown = 3;

    public int finalCountdown = 5;
    public int buildUpCountdown = 15;

    public float arenaDespawnTimeMin = 0.1f;
    public float arenaDespawnTimeMax = 0.5f;
    public float enemyDespawnTimeMin = 0.1f;
    public float enemyDespawnTimeMax = 0.5f;

    public bool dropShield = false;
    public GameObject shieldPickup;
    public float dropShieldTIme = 30f;

    //?
    public float killsPerSecondThreshold = 10;

    #endregion

    #region Private Variables


    private bool buildupTriggered = false;





    #endregion


    public override void Awake()
    {

        if (!isLoaded)
        {
            isPersistent = true;
            base.Awake();
            PreLoad();
        }
    }



    private void Update()
    {
        //Counts down if the game is playing and not paused
        if (isPlaying && !isPaused)
        {

            currCountDown -= Time.deltaTime;

            uIManagerInstance.UIController.UpdateTimerText(currCountDown.ToString("0.00"));

            if (currCountDown <= 0)
            {

                EndRound();

            }
            else if (currCountDown <= buildUpCountdown && !buildupTriggered)
            {

                CountdownBuildUp();

            }
            else if (currCountDown <= finalCountdown)
            {


                finalCountdown--;

                AudioManager.Instance.singleSirenEv.start();

            }

            secondsSinceTookDamge += Time.deltaTime;


            if (currKPSCounter < 1)
            {

                currKPSCounter += Time.deltaTime;

            }
            else
            {

                killsPerSecond = secondKillsDucks + secondKillsPhants;

                secondKillsDucks = 0;
                secondKillsPhants = 0;

                currKPSCounter = 0;

            }


        }


        // Check for pause being pressed.
        pauseBool = Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Start");


        //Pause and unpause game
        if (pauseBool && (isPlaying || roundEnded || isShopping))
        {
            if (isPaused)
                UnpauseGame();
            else
                PauseGame();
        }
    }




    public void ResetGameData()
    {

        print("Game Reset");
        PlayerPrefs.DeleteAll();

    }

    /// <summary>
    /// Run anything here that will need to apply to all scenes and be applied from the beggining.
    /// </summary>
    public void PreLoad()
    {

        // Keeps the curson in the application window
        Cursor.lockState = CursorLockMode.Confined;

        //if (loadShopValues)
        //   ShopManager.Instance.LoadShopValues();

        LoadScores();

        freezer = gameObject.AddComponent<FrameFreezer>();

        cameraShaker = gameObject.AddComponent<CameraShaker>();

        cameraZoom = gameObject.AddComponent<CameraZoom>();

        controllerViber = gameObject.AddComponent<ControllerViber>();

        timeManager = gameObject.AddComponent<TimeManager>();

        ControllerManager.controllerAvailable = ControllerManager.GetControllerState();

        // TODO Temp until option to shoose kb or mouse
        if (ControllerManager.controllerAvailable)
        {

            controllerActive = true;

        }

        AudioManager.Instance.Awake();

        isLoaded = true;


    }


    public void InitLoadScene()
    {


        HideMouse();

        AudioManager.Instance.backgroundTrackEv.setParameterValue("Menu Loop", 0f);
        //AudioManager.Instance.backgroundTrackEv.setParameterValue("Level Start", 1f);
        AudioManager.Instance.backgroundTrackEv.setParameterValue("Level End", 0f);
        AudioManager.Instance.backgroundTrackEv.setParameterValue("Pause Screen ON-OFF", 1f);
        AudioManager.Instance.backgroundTrackEv.start();

        musicIsLoaded = true;

    }


    /// <summary>
    /// Anything that needs to run at the beggining of the menu scene.
    /// </summary>
    public void InitMenuScene()
    {

        ShowMouse();

        isShopping = false;
        roundEnded = false;
        isPlaying = false;

        AudioManager.Instance.backgroundTrackEv.setParameterValue("Menu Loop", 1f);
        //AudioManager.Instance.backgroundTrackEv.setParameterValue("Level Start", 1f);
        AudioManager.Instance.backgroundTrackEv.setParameterValue("Level End", 0f);
        AudioManager.Instance.backgroundTrackEv.setParameterValue("Pause Screen ON-OFF", 0f);
        AudioManager.Instance.backgroundTrackEv.start();

    }


    /// <summary>
    /// Anything that needs to run at the beggining of the game scene.
    /// </summary>
    public void InitArenaScene()
    {

        HideMouse();

        if (!isLoaded)
        {
            PreLoad();
        }

        if (!musicIsLoaded)
        {
            AudioManager.Instance.backgroundTrackEv.setParameterValue("Menu Loop", 0f);
            //AudioManager.Instance.backgroundTrackEv.setParameterValue("Level Start", 1f);
            AudioManager.Instance.backgroundTrackEv.setParameterValue("Level End", 0f);
            AudioManager.Instance.backgroundTrackEv.setParameterValue("Pause Screen ON-OFF", 1f);
            AudioManager.Instance.backgroundTrackEv.start();
        }

        isShopping = false;
        roundEnded = false;
        isPlaying = false;

        cameraZoom.GetMainCam();

        cameraZoom.SetStartCamSize(10);


        InitRound();

    }


    /// <summary>
    /// Anything that needs to run at the beggining of the shop scene.
    /// </summary>
    public void InitShopScene()
    {

        HideMouse();
        isShopping = true;
        roundEnded = false;
        isPlaying = false;

        cameraShaker.camStartPos = new Vector3(0, 0, -10);

        cameraZoom.GetMainCam();

        cameraZoom.GetStartCamSize();


        AudioManager.Instance.shopTrackEv.setParameterValue("Pause Screen ON-OFF", 1f);

        AudioManager.Instance.shopTrackEv.start();

    }


    /// <summary>
    /// Anything that needs to run at the beggining of the shop scene.
    /// </summary>
    public void InitIntroScene()
    {

        HideMouse();


    }



    /// <summary>
    /// Setup the gamemanger to begin a new round.
    /// </summary>
    public void InitRound()
    {

        Time.timeScale = 1;

        isPaused = false;

        isPlaying = false;

        roundTickets = 0;

        AddTicketsToScore(0);

        //sets the countdown to count down value
        currCountDown = countDown;

        roundEnded = false;

        ResetDamageCounter();

        ResetKillsPerSecond();

        uIManagerInstance.UIController.gameObject.SetActive(true);

        uIManagerInstance.UIController.DisplayStartRoundUI();

        uIManagerInstance.arenaCover.SetActive(true);
        uIManagerInstance.arenaObjects.SetActive(true);

        buildupTriggered = false;

        dropShield = false;

        enemySpawner.SpawnEnemies(ObjectPooler.Instance.GetPooledPrefab("Ducks"), 4);

        AbilitiesController.Instance.DisableAbilites();

        Invoke("StartRound", startCountDown);
        TD2D_PlayerController.Instance.EnableMovementCoolDown(startCountDown);
    }


    /// <summary>
    /// Start the round
    /// </summary>
    public void StartRound()
    {

        // Anything that needs to happen when the round starts
        AudioManager.Instance.playerCheerEv.start();

        AudioManager.Instance.cheeringEv.start();

        cameraZoom.ZoomIn(10, 1);
        enemySpawner.StartSpawner();


        AbilitiesController.Instance.EnableAbilites();


        isPlaying = true;

        StartCoroutine(SpawnSheild());


    }


    IEnumerator SpawnSheild()
    {

        yield return new WaitForSeconds(Random.Range(dropShieldTIme - 10, dropShieldTIme + 10));

        dropShield = true;

    }


    /// <summary>
    /// End the round on timer or death.
    /// </summary>
    public void EndRound()
    {

        isPlaying = false;

        roundEnded = true;

        if (currCountDown <= 0)
        {

            currCountDown = 0;

            AudioManager.Instance.finalSirenEv.start();
            UIManager.Instance.timeUpText.SetActive(true);

        }

        AbilitiesController.Instance.DisableAbilites();

        //AudioManager.Instance.endRoundParam.setValue(1); // End of round trigger

        AudioManager.Instance.cheeringEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        AudioManager.Instance.backgroundTrackEv.setParameterValue("Level End", 57f);

        //Despawn all the enemies within given times
        DespawnEnemyObjects(enemyDespawnTimeMin, enemyDespawnTimeMax);
        //Despawn all the objects within given times
        DespawnArenaObjects(arenaDespawnTimeMin, arenaDespawnTimeMax);

        TD2D_WeaponController.Instance.ChangeWeapon(TD2D_WeaponController.WeaponTypes.Hammer);

        StartCoroutine(TeleportToMiddle());

        uIManagerInstance.UIController.UpdateTimerText(currCountDown.ToString("0.00"));

        EventsManager.Instance.PopupEvent(EventsManager.Instance.endroundTauntEvent);

        SaveScores();

        LoadScores();

        // Get round stats
        GetRoundKillsPerSecond();

    }


    IEnumerator TeleportToMiddle()
    {

        TD2D_PlayerController.Instance.LockPlayer();

        // Play death animation or happy animation!

        yield return new WaitForSeconds(0.5f);

        if (TD2D_PlayerController.Instance.PlayerRenderer.enabled)
        {

            ObjectPooler.Instance.Spawn(despawnPoof, TD2D_PlayerController.Instance.transform.position, Quaternion.identity);
            TD2D_PlayerController.Instance.TogglePlayerVisuals();

        }

        yield return new WaitForSeconds(0.5f);

        TD2D_PlayerController.Instance.Teleport(Vector3.zero);


        TD2D_WeaponController.Instance.weaponCharging = false;
        TD2D_WeaponController.Instance.chargeTimer = 0.0f;
        TD2D_WeaponController.Instance.CurrentWeapon.reticle.transform.localScale = Vector3.one;


        if (!TD2D_PlayerController.Instance.PlayerRenderer.enabled)
        {

            ObjectPooler.Instance.Spawn(despawnPoof, TD2D_PlayerController.Instance.transform.position, Quaternion.identity);
            TD2D_PlayerController.Instance.facingDirection = TD2D_PlayerController.PlayerDirection.Down;
            TD2D_PlayerController.Instance.TogglePlayerVisuals();

        }

        uIManagerInstance.UIController.DisplayEndRoundUI();


        yield return new WaitForSeconds(1.5f);


        TD2D_PlayerController.Instance.UnlockPlayer();


    }



    public void DespawnEnemyObjects(float _minTime, float _maxTime)
    {

        EnemySpawner.Instance.DeSpawnAllEnemies(_minTime, _maxTime);

    }


    public void DespawnArenaObjects(float _minTime, float _maxTime)
    {

        ArenaManager.instance.DespawnAllObjects(_minTime, _maxTime);

    }




    /// <summary>
    /// Restarts a new round in game.
    /// </summary>
    public void RestartRound()
    {

        musicIsLoaded = false;
        LoadArenaScene();
        //GameSceneManager.RestartScene();

    }



    public void LoadArenaScene()
    {

        if (isPaused)
            UnpauseGame();

        cameraShaker.StopShake();
        AudioManager.Instance.cheeringEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        AudioManager.Instance.backgroundTrackEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        GameSceneManager.LoadScene(GameSceneManager.Scenes.Arena);

    }



    public void LoadShopScene()
    {

        if (isPaused)
            UnpauseGame();

        isPlaying = false;
        cameraShaker.StopShake();
        AudioManager.Instance.cheeringEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        AudioManager.Instance.backgroundTrackEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        GameSceneManager.LoadScene(GameSceneManager.Scenes.Shop);

    }


    public void LoadMenuScene()
    {
        if (isPaused)
            UnpauseGame();

        cameraShaker.StopShake();
        AudioManager.Instance.cheeringEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        AudioManager.Instance.backgroundTrackEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);

        GameSceneManager.LoadScene(GameSceneManager.Scenes.Menu);
    }


    public void LoadIntroScene()
    {

        AudioManager.Instance.backgroundTrackEv.setParameterValue("Menu Loop", 0f);
        //AudioManager.Instance.backgroundTrackEv.setParameterValue("Level Start", 1f);
        AudioManager.Instance.backgroundTrackEv.setParameterValue("Level End", 0f);
        AudioManager.Instance.backgroundTrackEv.setParameterValue("Pause Screen ON-OFF", 0f);
        GameSceneManager.LoadScene(GameSceneManager.Scenes.Intro);

    }


    public void LoadLoadingScene()
    {

        AudioManager.Instance.cheeringEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        AudioManager.Instance.backgroundTrackEv.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        GameSceneManager.LoadScene(GameSceneManager.Scenes.Loading);

    }


    /// <summary>
    /// Show the mouase cursor
    /// </summary>
    public void ShowMouse()
    {

        Cursor.visible = true;

    }

    /// <summary>
    /// Hide the mouase cursor
    /// </summary>
    public void HideMouse()
    {

        Cursor.visible = false;

    }



    /// <summary>
    /// Pauses the game
    /// </summary>
    public void PauseGame()
    {
        AudioManager.Instance.backgroundTrackEv.setParameterValue("Pause Screen ON-OFF", 0f);
        Time.timeScale = 0;

        isPaused = true;
        //if (!isShopping)
        //{

        uIManagerInstance.UIController.PauseUI();

        //}
        //else
        // {

        // uIManagerInstance.shopUIController.PauseUI();

        //}
    }



    /// <summary>
    /// Unpauses the game
    /// </summary>
    public void UnpauseGame()
    {
        AudioManager.Instance.backgroundTrackEv.setParameterValue("Pause Screen ON-OFF", 1f);

        uIManagerInstance.UIController.UnpauseUI();




        isPaused = false;
        Time.timeScale = 1;

    }


    public void LoadScores()
    {
        highScore = PlayerPrefs.GetInt("highScore", 0);
        totalTickets = PlayerPrefs.GetInt("totalTickets", 0);
    }

    //save highest number as high score
    public void SaveScores()
    {
        PlayerPrefs.SetInt("highScore", Mathf.Max(highScore, roundTickets));
        PlayerPrefs.SetInt("totalTickets", totalTickets);
    }

    //compare highscore
    public bool IsNewHighScore()
    {
        return roundTickets >= highScore;
    }


    [ContextMenu("Add1kTickets")]
    public void Add1kTickets()
    {

        AddTicketsToScore(1000);

    }


    public void UpdateTotalTickets(int tickets)
    {

        totalTickets += tickets;

        SaveScores();

    }


    public void AddTicketsToScore(int tickets)
    {

        roundTickets += tickets;
        totalTickets += tickets;

        if (!isShopping)
        {
            uIManagerInstance.UIController.UpdateScoreText(roundTickets.ToString("0000"), totalTickets.ToString("0000"));
        }
        else
        {
            // Update shop tickets ui
        }

    }


    //public bool CheckForDrop()
    //{
    //    bool shouldSpawn = (pickupChance >= pickUpDropAmount) ? true : false;
    //    return shouldSpawn;
    //}


    //public GameObject PickupObject()
    //{

    //    int maxObjects = pickUpObjects.Length;

    //    int randPickup = Random.Range(0, maxObjects);

    //    return pickUpObjects[randPickup];

    //}


    public void OnPlayerTookDamage()
    {

        highestKillsBeforeTookDamage = Mathf.Max(highestKillsBeforeTookDamage, killsSinceTookDamage);

        killsSinceTookDamage = 0;

        ResetDamageCounter();

    }


    public void AddToDuckKills()
    {

        secondKillsDucks++;
        roundKillsDucks++;
        killsSinceTookDamage++;

    }

    public void AddToElephantKills()
    {

        secondKillsPhants++;
        roundKillsPhants++;
        killsSinceTookDamage++;

    }


    /// <summary>
    /// Returns the amount of kills per second for this round.
    /// </summary>
    /// <returns></returns>
    public float GetRoundKillsPerSecond()
    {

        roundKillsPerSecond = (roundKillsDucks + roundKillsPhants) / countDown;
        return roundKillsPerSecond;

    }


    /// <summary>
    /// Called at the beggining of the round to reset the kps counter
    /// </summary>
    public void ResetKillsPerSecond()
    {

        killsPerSecond = 0;
        roundKillsPerSecond = 0;


    }

    /// <summary>
    /// Called when the player takes damage from an enemy and also at the start of the round.
    /// </summary>
    public void ResetDamageCounter()
    {

        secondsSinceTookDamge = 0;

    }



    public void CountdownBuildUp()
    {

        AudioManager.Instance.countDownBuildUpEv.start();
        buildupTriggered = true;

    }


}

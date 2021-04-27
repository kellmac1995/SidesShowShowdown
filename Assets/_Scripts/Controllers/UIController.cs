using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class UIController : MonoBehaviour
{

    private GameManager _gameManager;
    private UIManager _uiManager;





    // Use this for initialization
    void Start()
    {

        _gameManager = GameManager.Instance;
        _uiManager = UIManager.Instance;


        //_uiManager.endGamePanel.SetActive(false);
        if (!GameManager.Instance.isShopping)
        {
            _uiManager.flashscreenPanel.SetActive(true);
            _uiManager.timeUpText.SetActive(false);
        }


    }


    public void UpdateTimerText(string _time)
    {

        _uiManager.timerText.text = _time;

    }



    public void UpdateScoreText(string _tickets, string _totalTickets)
    {

        UIManager.Instance.scoreUI.text = "+ " + _tickets; // GameManager.Instance.roundTickets.ToString("0000"); // Display round tickets
        UIManager.Instance.totalScoreUI.text = _totalTickets;

    }



    public void MumAbilityAnimation()
    {
        _uiManager.characterAnimator.SetTrigger("MGazeActivated");
    }

    public void DadAbilityAnimation()
    {
        _uiManager.characterAnimator.SetTrigger("PunActivated");
    }

    public void BabyAbilityAnimation()
    {
        _uiManager.characterAnimator.SetTrigger("RattleActivated");
    }

    public void SisAbilityActivatedAnimation()
    {
        _uiManager.characterAnimator.SetTrigger("HeelyActivated");
        //_uiManager.playerSpeedVignette.SetTrigger("heeliesActiated");
        _uiManager.vignetteAnimator.SetBool("Activated", true);
    }

    public void SisAbilityDeactivatedAnimation()
    {
        _uiManager.characterAnimator.SetTrigger("HeelyDeactivated");
        _uiManager.vignetteAnimator.SetBool("Activated", false);
    }

   


    public void UnpauseButtonClicked()
    {
        _gameManager.UnpauseGame();
    }

    public void PauseUI()
    {
        if (GameManager.Instance.isShopping)
            UIManager.Instance.familyGroup.interactable = false;

        _uiManager.pauseMenuPanel.SetActive(true);
        Cursor.visible = true;
    }

    public void UnpauseUI()
    {

        if (GameManager.Instance.isShopping)
            UIManager.Instance.familyGroup.interactable = true;

        _uiManager.pauseMenuPanel.SetActive(false);
        Cursor.visible = false;

    }

    //TODO MOVE
    public void RestartScene()
    {

        GameManager.Instance.RestartRound();

    }


    public void EnterStore()
    {

        GameManager.Instance.LoadShopScene();
    }


    public void ExitScene()
    {

        GameManager.Instance.LoadMenuScene();

    }


    /// <summary>
    /// Sets and displays the UI for the end of a round.
    /// </summary>
    public void DisplayEndRoundUI()
    {

        //UpdateTimerText("0.00");

        //if (GameManager.Instance.IsNewHighScore())
        //{
        //    _uiManager.endGameText.text = "NEW ";
        //}
        //else
        //{
        //    _uiManager.endGameText.text = "";
        //}

        //_uiManager.endGameText.text += "HIGH SCORE: " + GameManager.Instance.highScore + " TICKETS!";


        //_uiManager.restartButton.SetActive(true);
        //_uiManager.shopDoor.SetActive(true);
        _uiManager.endGameObjects.transform.position = TD2D_PlayerController.Instance.transform.position;

        _uiManager.endGameObjects.SetActive(true);

        //_uiManager.endGamePanel.SetActive(true);

        //MORE ENDGAME UI ANIMATION COMING
        _uiManager.camAnimator.SetTrigger("endGame");

        //Cursor.visible = true;
    }


    /// <summary>
    /// Sets and displays the UI for the start of a round.
    /// </summary>
    public void DisplayStartRoundUI()
    {

        //_uiManager.restartButton.SetActive(false);
        //_uiManager.shopDoor.SetActive(false);


        //_uiManager.endGamePanel.SetActive(false);

        // Do any start of round UI stuff here...

    }

}

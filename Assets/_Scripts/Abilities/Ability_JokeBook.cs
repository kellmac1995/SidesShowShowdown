using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Ability_JokeBook : Ability
{
    public Animator _playerAnimator;
    public float stunTimer = 2f;

    public bool punActive = false;

    public Image[] dadJokes;



    // Update is called once per frame
    public override void Update()
    {

        base.Update();

        bool buttonPressed = Input.GetKeyDown(KeyCode.Alpha2) || Input.GetButton("ButtonA");

        if (buttonPressed && !punActive && !onCoolDown && !GameManager.Instance.isPaused && GameManager.Instance.isPlaying)
        {

            //GetDadJoke();
            // UIManager.Instance.arenaUIController.DadAbilityAnimation();

            EventsManager.Instance.PopupEvent(EventsManager.Instance.dadEvent);

            AudioManager.Instance.dadJokeEv.start();
            cooldownFill.fillAmount = 1;

            // Deactivate abilitiy availible animation here

            StunEnemies(stunTimer);

            EnableCooldown();

        }
    }


    public void StunEnemies(float _stunTimer)
    {
        
        foreach (TD2D_Enemy enemy in EnemySpawner.Instance.activeEnemies)
        {
            enemy.Stun(_stunTimer);
        }
        
       
    }


    public void GetDadJoke()
    {

        foreach (Image joke in dadJokes)
        {

            joke.enabled = false;

        }

        dadJokes[Random.Range(1, dadJokes.Length) - 1].enabled = true;

    }
}

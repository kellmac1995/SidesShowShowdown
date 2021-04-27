using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ability_Rattle : Ability
{
    public float lureTime = 3f;
    public GameObject RattlePrefab;


    // Update is called once per frame
    public override void Update()
    {

        base.Update();

        bool buttonPressed = Input.GetKeyDown(KeyCode.Alpha3) || Input.GetButton("ButtonB");


        if (buttonPressed && !AbilitiesController.Instance.rattleOn && !onCoolDown && !GameManager.Instance.isPaused && GameManager.Instance.isPlaying)
        {
            //print("lure activated");
            cooldownFill.fillAmount = 1;
            LureEnemies();
            EventsManager.Instance.PopupEvent(EventsManager.Instance.babyEvent);
            // Deactivate abilitiy availible animation here
            AudioManager.Instance.babyRattleEv.start();


        }

    }


    public void LureEnemies()
    {

        AbilitiesController.Instance.rattleOn = true;

        AbilitiesController.Instance.rattleObject = ObjectPooler.Instance.Spawn(RattlePrefab, TD2D_PlayerController.Instance.transform.position, transform.rotation);

        foreach (TD2D_Enemy enemy in GameManager.Instance.enemySpawner.activeEnemies)
        {
            if (enemy == null)
                continue;

            Pathfinding.AIDestinationSetter ai = enemy.GetComponent<Pathfinding.AIDestinationSetter>();

            enemy.enemyAnimator.SetBool("isLured", true);

            if (ai)
            {

                ai.target = AbilitiesController.Instance.rattleObject.transform;

            }

        }

        StartCoroutine(DeactivateLure());

        EnableCooldown();

        AudioManager.Instance.babyRattleCountdownEv.start();
        

    }


    IEnumerator DeactivateLure()
    {
        yield return new WaitForSeconds(lureTime);
        
        foreach (TD2D_Enemy enemy in GameManager.Instance.enemySpawner.activeEnemies)
        {
            if (enemy == null)
                continue;

            Pathfinding.AIDestinationSetter ai = enemy.GetComponent<Pathfinding.AIDestinationSetter>();

            if (ai)
            {
                enemy.GetComponent<Pathfinding.AIDestinationSetter>().target = TD2D_PlayerController.Instance.gameObject.transform;

            }

            enemy.enemyAnimator.SetBool("isLured", false);
        }

        AbilitiesController.Instance.rattleOn = false;
        ObjectPooler.Instance.Despawn(AbilitiesController.Instance.rattleObject);

    }
}

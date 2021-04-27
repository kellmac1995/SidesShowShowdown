using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TD2D_Enemy_Melee : TD2D_Enemy
{

    public float attackDistance = 1;
    public float quackInt;

    bool performingAttack = false;
    

    public override void OnEnable()
    {
        base.OnEnable();

        
        if (!aiDestination.target)
        {

            aiDestination.target = target.transform;

        }

    }

    public void Start()
    {
        

    }


    public override void Update()
    {


        base.Update();


        //if (inAir)
        //    destroyCounter -= Time.deltaTime;


        //if (destroyCounter < 0)
        //{

        //    // Run the base class death
        //    base.Death();

        //}


        if (!isAttacking && !beingShrunk)
            CheckRange();


        //if (isStunned)
        //{
        //    aiCooldown -= Time.deltaTime;

        //    if (aiCooldown <= 0 & !inAir)
        //    {

        //        StartEnemyAI();

        //    }
        //}

    }


    void CheckRange()
    {
        if (Vector2.Distance(transform.position, target.position) < attackDistance * 1.5f)
        {

            //performingAttack = true;
            isAttacking = true;
            enemyAnimator.SetTrigger("Attack");
            //TODO FIX EVENT
            //AudioManager.Instance.enemyAttackEv.start();

        }
    }


    public void CheckSuccess()
    {
        if (Vector2.Distance(transform.position, target.position) < attackDistance)
        {
            QuackRandomiser();
            DealDamage();
        }

        FinishedCheckRange();

    }



    public override void DealDamage()
    {

        if (!GameManager.Instance.isPlaying)
            return;
        //AudioManager.Instance.enemyAttackEv.start();

        // TODO move this out of here
        TD2D_PlayerController.Instance.PlayerHealth.TakeDamage(1);

        //print("player took damage"); 
        GameManager.Instance.controllerViber.Vibrate(0.5f, 0.5f, 0.5f);
        //GameManager.Instance.freezer.Freeze(0.1f);
    }

       
    public void FinishedCheckRange()
    {
        QuackRandomiser();
        isAttacking = false;
        //performingAttack = false;
        //enemyAnimator.ResetTrigger("Attack");

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {


        if (collision.transform.CompareTag("Beetle") || collision.transform.CompareTag("PlayerBullet"))
        {

            TakeDamage();

        }
        else if (collision.transform.CompareTag("Player") && canBeSquished)
        {
            //ObjectPooler.Instance.Spawn(scorchPrefab, transform.position, transform.rotation);
            Death();
        }


    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{

    //    if (collision.transform.CompareTag("Beetle") || collision.transform.CompareTag("PlayerBullet") && Time.timeScale == 1)
    //    {

    //        TakeDamage();

    //    }
    //    else if (collision.transform.CompareTag("Player") && canBeSquished)
    //    {
    //        ObjectPooler.Instance.Spawn(scorchPrefab, transform.position, transform.rotation);
    //        Death(); 
    //    }

    //}


  public void QuackRandomiser()
    {
        quackInt = Random.Range(0f, 2f);

        if(quackInt >= 1.5 && quackInt <= 2)
        {
            AudioManager.Instance.duckQuackEv.start();
        }
    }

    public override void Death()
    {

        //if (GameManager.Instance.CheckForDrop())
        //{            
        //    // Spawn Pickup
        //    Instantiate(GameManager.Instance.PickupObject(), transform.position, Quaternion.identity);

        //    GameManager.Instance.pickupChance = 0;

        //}


        AudioManager.Instance.enemyDeathkEv.start();

        //Player animation here
        ObjectPooler.Instance.Spawn(deathParticle, transform.position, Quaternion.identity);

        GameManager.Instance.AddTicketsToScore(ticketDropAmount);
        GameManager.Instance.AddToDuckKills();
        
        // Run the base class death
        base.Death();

    }


}

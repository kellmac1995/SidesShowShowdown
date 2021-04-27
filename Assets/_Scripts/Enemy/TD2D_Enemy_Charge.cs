using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TD2D_Enemy_Charge : TD2D_Enemy
{


    public LayerMask obstacles;

    public float searchRadius = 30;

    public GameObject offscreenArrow;

    public float chargeSpeed = 20;

    public float attackDistance = 1f;
    public float attackTime = 2f;


    public GameObject upParticle;
    public GameObject downParticle;
    public GameObject leftParticle;
    public GameObject rightParticle;

    private GameObject activeParticle;

    public float pushTime = 1;

    public float pushForce = 30;


    bool performingAttack = false;


    Vector3 heading;


    public bool showGizmos = false;

    public override void OnEnable()
    {

        base.OnEnable();

        //Invoke("EnableColliders", 1);
    }


    void EnableColliders()
    {

        GetComponent<CircleCollider2D>().enabled = true;

    }


    public override void Update()
    {



        base.Update();



        if (canMove)
            CheckRange();


        if (aiPath.canMove && !aiPath.pathPending && (aiPath.reachedEndOfPath || !aiPath.hasPath))
        {
            aiPath.destination = PickRandomPoint();

            aiPath.SearchPath();

            return;

        }

        if (canMove)
        {

            heading = aiPath.destination - transform.position;

        }
        else
        {
            heading = target.position - transform.position;
        }


        //if (!isAttacking)
        //{
        //    //UpdateVisual(AngleDir(transform.forward, heading, transform.up));

        //}


    }

    //TODO make a bit better
    Vector3 PickRandomPoint()
    {
        var point = Random.insideUnitSphere * searchRadius;

        point.y = 0;
        point += aiPath.position;
        return point;
    }



    void CheckRange()
    {
        if (Vector2.Distance(transform.position, target.position) < attackDistance)
        {

            CheckLineOfSight();

        }
    }



    void CheckLineOfSight()
    {

        bool playerInSights = false;


        Vector3 direction = (TD2D_PlayerController.Instance.transform.position - transform.position).normalized;

        RaycastHit2D[] intercects = Physics2D.CircleCastAll(transform.position, transform.localScale.x * 2.5f, direction, Vector2.Distance(transform.position, target.position), obstacles);


        foreach (RaycastHit2D obstacle in intercects)
        {

            if (!obstacle.collider.gameObject.CompareTag("Player"))
            {
                return;
            }
            else if (obstacle.collider.gameObject.CompareTag("Player"))
            {
                playerInSights = true;
            }
        }

        if (playerInSights)
        {
            //charge player
            ChargeAttack();

        }

    }



    void ChargeAttack()
    {

        StopEnemyAI(attackTime);

        //performingAttack = true;
        isAttacking = true;

        DetermineEnemyDirection();


        if (facingDirection == EnemyDirection.Up)
        {
            activeParticle = upParticle;
            activeParticle.SetActive(true);
        }
        else if (facingDirection == EnemyDirection.Down)
        {
            activeParticle = downParticle;
            activeParticle.SetActive(true);
        }
        else if (facingDirection == EnemyDirection.Left)
        {
            activeParticle = leftParticle;
            activeParticle.SetActive(true);
        }
        else if(facingDirection == EnemyDirection.Right)
        {
            activeParticle = rightParticle;
            activeParticle.SetActive(true);
        }


        enemyAnimator.SetTrigger("Attack"); // TODO change to a bool

        Vector2 direction = (transform.position - TD2D_PlayerController.Instance.transform.position).normalized;
        rb.velocity = -direction * chargeSpeed;
        AudioManager.Instance.elephantChargeEv.start();

    }


    void StopAttack()
    {

        activeParticle.SetActive(false);
        rb.velocity = Vector2.zero;
        //performingAttack = false;
        isAttacking = false;

    }


    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Vector3 direction = (TD2D_PlayerController.Instance.transform.position - transform.position).normalized;
            Gizmos.DrawRay(transform.position, direction);
            Gizmos.DrawWireSphere(transform.position, transform.localScale.x * 2.5f);
            Gizmos.DrawWireSphere(transform.position, searchRadius);
        }
    }



    //void DealDamage()
    //{
    //    AudioManager.Instance.playerDamagedEv.start();
    //    // TODO move this out of here
    //    TD2D_PlayerController.Instance.playerHP.RemoveHealth();

    //    GameManager.Instance.freezer.Freeze(0.1f);
    //    UIManager.Instance.playerDamagedFlash.SetActive(true);

    //}


    void PushEnemy(TD2D_Enemy enemy)
    {

        Vector3 direction = (transform.position - enemy.transform.position).normalized;
        Vector3 velocity = -direction * pushForce;

        enemy.Jump(velocity, pushTime, false);

    }




    EnemyDirection AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
    {
        Vector3 perp = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);

        if (dir > 0f)
        {
            return EnemyDirection.Right;
        }
        else //if (dir < 0f)
        {
            return EnemyDirection.Left;
        }
    }


    void UpdateVisual(EnemyDirection dir)
    {

        if (dir == EnemyDirection.Right)
        {
            // TODO Set animaor bool
            m_SpriteRenderer.flipX = true;
        }
        else if (dir == EnemyDirection.Left)
        {
            // TODO set animator bool
            m_SpriteRenderer.flipX = false;
        }

    }




    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("Enemy"))
        {

            // make enemies get pushed out of the way
            if (isAttacking)

            {
                PushEnemy(collision.GetComponent<TD2D_Enemy>());
            }

        }
        else if (collision.transform.CompareTag("Player") && isAttacking)
        {

            StopAttack();
            TD2D_PlayerController.Instance.PlayerHealth.TakeDamage(1);

        }
        else if (collision.transform.CompareTag("Beetle"))
        {

            TakeDamage();


        }
        else if (collision.transform.CompareTag("PlayerBullet"))
        {

            TakeDamage();

        }
        else // If hit anything other than ducks or player
        {

            if (isAttacking)
            {
                //hit something!
                StopAttack();
            }
        }

    }


    public override void Death()
    {


        EnemySpawner.Instance.activePhantEnemies.Remove(this);
        GameManager.Instance.AddToElephantKills();
        GameManager.Instance.AddTicketsToScore(ticketDropAmount);
        Instantiate(deathParticle, transform.position, transform.rotation);

        AudioManager.Instance.elephantDeathEV.start();


        base.Death();

    }

}

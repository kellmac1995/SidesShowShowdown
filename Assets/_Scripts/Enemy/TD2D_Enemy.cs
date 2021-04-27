using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TD2D_Enemy : MonoBehaviour
{

    public int ticketDropAmount = 20;

    public float maxSpeed = 7;
    public float shrinkSpeed = 2;

    public GameObject deathParticle;

    public int beetleDamage = 100;

    public int playerDamage = 100;

    public int heeliesDamage = 50;

    public int rangedDamage = 100;

    public int startingHealth = 100;
    public int currentHealth;

    public bool canMove = true;

    public bool beingShrunk = false;

    public bool canBeSquished = false;
    public bool isStunned = false;

    public bool isSprung = false;

    public bool isStuck = false;

    public bool inAir = false;

    public bool isAttacking = false;

    //private float aiCooldown = 1f;

    // Used to store the targets/mouse position
    public float targetX, targetY;

    // Used when calculating the difference between the targets pos and the players pos
    public float xDifference, yDifference;


    public Animator enemyAnimator;
    public Pathfinding.AIPath aiPath;
    public Pathfinding.AIDestinationSetter aiDestination;
    public SpriteRenderer m_SpriteRenderer;
    public Rigidbody2D rb;

    [HideInInspector]
    public Transform target;


    // enum used for the facing direction
    public enum EnemyDirection { Left, Right, Up, Down };

    // stores the direction the enemy is facing
    public EnemyDirection facingDirection;

    public EnemyDirection previousDirection;

    public float dirDetectionDeadzone = 0.15f;

    //public Sprite leftSprite, rightSprite, upSprite, downSprite;


    public virtual void Awake()
    {

        enemyAnimator = GetComponent<Animator>();
        aiPath = GetComponent<Pathfinding.AIPath>();

        m_SpriteRenderer = GetComponent<SpriteRenderer>();

        aiDestination = GetComponent<Pathfinding.AIDestinationSetter>();

        rb = GetComponent<Rigidbody2D>();

    }


    //public virtual void Start()
    //{



    //}


    public virtual void OnEnable()
    {

        ResetValues();

    }


    public virtual void Update()
    {

        if (!isAttacking && !isStunned && !beingShrunk) // if not attacking change enemy direction
        {
            DetermineEnemyDirection();
            // Set enemy speed in animator;
            if (aiPath)
                enemyAnimator.SetFloat("Speed", aiPath.desiredVelocity.normalized.magnitude);
        }



    }


    public virtual void StopEnemyAI()
    {

        canMove = false;
        aiPath.canMove = false;

    }


    public virtual void StopEnemyAI(float _stopTime)
    {

        StopEnemyAI();
        StartCoroutine(WaitThenStartEnemyAI(_stopTime));

    }


    IEnumerator WaitThenStartEnemyAI(float _waitTime)
    {

        yield return new WaitForSeconds(_waitTime);
        StartEnemyAI();

    }


    public virtual void StartEnemyAI()
    {


        aiPath.Teleport(transform.position, true);

        aiPath.canMove = true;
        canMove = true;

    }


    public virtual void DealDamage()
    {



    }



    public virtual void Stun()
    {

        isStunned = true;
        StopEnemyAI();

        enemyAnimator.SetBool("isStunned", true);
        //rb.velocity = Vector2.zero;

    }



    public virtual void Stun(float _stunTime)
    {

        Stun();

        StartCoroutine(WaitThenUnStun(_stunTime));

    }




    public virtual void UnStun()
    {

        enemyAnimator.SetBool("isStunned", false);

        StartEnemyAI();
        isStunned = false;

    }


    IEnumerator WaitThenUnStun(float _waitTime)
    {

        yield return new WaitForSeconds(_waitTime);

        UnStun();

    }


    public virtual void Shrink()
    {

        if (!beingShrunk && !canBeSquished)
        {
            beingShrunk = true;
            enemyAnimator.SetTrigger("Shrink");
            aiPath.maxSpeed = shrinkSpeed;
            
        }

    }


    public virtual void FinishShrink()
    {
        canBeSquished = true;
        beingShrunk = false;
        print("finished shrink");
    }


    public virtual void Jump(Vector3 _velocity, float _jumpTime, bool _isSprung)
    {

        inAir = true;

        isSprung = _isSprung;

        foreach (CapsuleCollider2D coll in GetComponents<CapsuleCollider2D>())
        {
            coll.isTrigger = true;
        }
        foreach (CircleCollider2D coll in GetComponents<CircleCollider2D>())
        {
            coll.isTrigger = true;
        }

        // trigger animator for jumping here (Scale Up/Down)
        enemyAnimator.SetTrigger("Spring");

        StopEnemyAI();

        rb.velocity = _velocity;

        // TODO Run Land at the end of the jump animation instead of invoke
        Invoke("Land", _jumpTime);


    }

    public virtual void Land()
    {


        foreach (CapsuleCollider2D coll in GetComponents<CapsuleCollider2D>())
        {
            coll.isTrigger = false;
        }
        foreach (CircleCollider2D coll in GetComponents<CircleCollider2D>())
        {
            coll.isTrigger = false;
        }


        if (!isSprung)

        {//if not dead
            StartEnemyAI();

            UpdateEnemyVisual();

            inAir = false;

        }
        else
        {
            //enemyAnimator.SetTrigger("Reset");
            Despawn();
        }

    }



    public void Stick()
    {


        StopEnemyAI();
        enemyAnimator.SetBool("isStuck", true);
        isStuck = true;

    }

    public void UnStick()
    {

        StartEnemyAI();
        enemyAnimator.SetBool("isStuck", false);
        isStuck = false;

    }



    public void DetermineEnemyDirection()
    {

        float xVal, yVal;


        xVal = aiPath.desiredVelocity.normalized.x;
        yVal = aiPath.desiredVelocity.normalized.y;


        if (Mathf.Abs(xVal) > Mathf.Abs(yVal))
        {
            if (Mathf.Abs(xVal) < dirDetectionDeadzone)
                return;

            if (xVal > 0)
            {

                facingDirection = EnemyDirection.Right;
                //print("right");

            }
            else if (xVal < 0)
            {

                facingDirection = EnemyDirection.Left;
                //print("left");
            }
        }
        else
        {

            if (Mathf.Abs(yVal) < dirDetectionDeadzone)
                return;

            if (yVal > 0)
            {
                facingDirection = EnemyDirection.Up;
                //print("up");
            }
            else if (yVal < 0)
            {

                facingDirection = EnemyDirection.Down;
                //print("down");
            }
        }


        if (facingDirection != previousDirection)
        {
            previousDirection = facingDirection;
            UpdateEnemyVisual();
        }


    }


    public void UpdateEnemyVisual()
    {

        if (facingDirection == EnemyDirection.Right)
        {
            enemyAnimator.SetTrigger("FacingRight");
        }
        else if (facingDirection == EnemyDirection.Left)
        {
            enemyAnimator.SetTrigger("FacingLeft");
        }
        else if (facingDirection == EnemyDirection.Up)
        {
            enemyAnimator.SetTrigger("FacingUp");
        }
        else if (facingDirection == EnemyDirection.Down)
        {
            enemyAnimator.SetTrigger("FacingDown");
        }

    }


    public virtual void TakeDamage()
    {

        currentHealth -= currentHealth;

        if (currentHealth <= 0)
        {
            // ... the enemy is dead.
            Death();
        }
        else
        {
            //Player animation here
            m_SpriteRenderer.color = new Color(0.5f, 0f, 0f, 1f);
        }

    }


    public virtual void TakeDamage(int damageAmt)
    {

        currentHealth -= damageAmt;

        if (currentHealth <= 0)
        {
            // ... the enemy is dead.
            Death();
        }
        else
        {
            //Player animation here
            m_SpriteRenderer.color = new Color(0.5f, 0f, 0f, 1f);
        }

    }



    public virtual void Death()
    {

        // Do common death stuff here

        if (GameManager.Instance.dropShield)
        {

            ObjectPooler.Instance.Spawn(GameManager.Instance.shieldPickup, transform.position, Quaternion.identity);

            GameManager.Instance.dropShield = false;

        }

        //Removing itself from tracked active enemys.
        EnemySpawner.Instance.activeEnemies.Remove(this);

        // Despawn enemy.
        ObjectPooler.Instance.Despawn(gameObject);


    }

    /// <summary>
    /// Despawn an enemy in a cloud of smoke.
    /// </summary>
    public virtual void Despawn()
    {

        ObjectPooler.Instance.Spawn(GameManager.Instance.despawnPoof, transform.position, Quaternion.identity);

        if (GetComponent<TD2D_Enemy_Charge>() != null)
        {
            EnemySpawner.Instance.activePhantEnemies.Remove(GetComponent<TD2D_Enemy_Charge>());
        }

        EnemySpawner.Instance.activeEnemies.Remove(this);

        ObjectPooler.Instance.Despawn(gameObject);

    }


    public void ResetValues()
    {

        currentHealth = startingHealth;

        aiPath.maxSpeed = maxSpeed;


        if (GameManager.Instance.isPlaying)
        {
            if (isStuck)
            enemyAnimator.SetBool("isStuck", false);
            if (isStunned)
                enemyAnimator.SetBool("isStunned", false);

                enemyAnimator.SetBool("isLured", false);

                enemyAnimator.SetTrigger("Reset");

        }

        canMove = true;
        beingShrunk = false;
        canBeSquished = false;
        isStunned = false;
        isSprung = false;
        inAir = false;
        isAttacking = false;
        isStuck = false;




        target = TD2D_PlayerController.Instance.transform;

        // Check if being lured
        if (AbilitiesController.Instance.rattleOn)
        {

            enemyAnimator.SetBool("isLured", true);

            aiDestination.target = AbilitiesController.Instance.rattleObject.transform;

        }

    }


    /// <summary>
    /// Despawns enemy within a time range.
    /// </summary>
    /// <param name="_minTime">Minimum amount of time before the enemy despawns</param>
    /// <param name="_maxTime">Maximum amount of time before the enemy despawns</param>
    public virtual void Despawn(float _minTime, float _maxTime)
    {

        StartCoroutine(DoTimedDespawn(_minTime, _maxTime));

    }


    IEnumerator DoTimedDespawn(float _minTime, float _maxTime)
    {

        yield return new WaitForSeconds(Random.Range(_minTime, _maxTime));

        Despawn();

    }


    public virtual void OnDisable()
    {

        StopAllCoroutines();

    }


}

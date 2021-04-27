using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRanged : MonoBehaviour
{

    public float shakeAmt, shakeLength;

    public float expForce;

    private Vector3 direction;
    private Vector3 velocity;
    public float pushTime = 0.5f;

    public GameObject expParticle;
    public GameObject mimeTurretPrefab;

    public float killRadius = 3;
    public float knockbackRadius = 5;


    public LayerMask killLayers;
    public LayerMask knockbackLayers;

    public bool drawGizmos = false;

    Animator animator;
    SpriteRenderer sr;

    Pathfinding.AIPath aiPath;

    public float fallDistance = 0.1f;

    public Transform target;


    void Start()
    {

        animator = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();

    }



    //private void OnTriggerEnter2D(Collider2D collision)
    //{

    //    if (collision.transform.CompareTag("Target"))
    //    {
    //        animator.SetTrigger("Fall");
    //        //// play animation 
    //        //print("fall to ground");

    //    }

    //}


    private void Update()
    {

        if (!GameManager.Instance.isPlaying)
            ObjectPooler.Instance.Despawn(gameObject);


        if (Vector2.Distance(transform.position, target.position) < fallDistance)
        {

            animator.SetTrigger("Fall");

        }

    }


    void Explode()
    {

        {
            //instaniate visual and audio feedback
            //GameManager.Instance.cameraShaker.Shake(shakeAmt, shakeLength);

            sr.enabled = false;

            ObjectPooler.Instance.Spawn(expParticle, transform.position, transform.rotation);

            //AudioManager.Instance.barrelExplodeEv.start();

            AudioManager.Instance.mimeThudEv.start();


            // Get all colliders in area
            Collider2D[] killObjects = Physics2D.OverlapCircleAll(transform.position, killRadius, killLayers);

            foreach (Collider2D killedObject in killObjects)
            {

                DealDamage();

            }

            // Get all colliders in area
            Collider2D[] knockbackObjects = Physics2D.OverlapCircleAll(transform.position, knockbackRadius, knockbackLayers);

            foreach (Collider2D knockbackObject in knockbackObjects)
            {

                Rigidbody2D rb = knockbackObject.GetComponent<Rigidbody2D>();

                if (rb == null)
                {

                    print("Error! Incorrect object in knockback layer or does not have a rigidbody!");

                }
                else
                {

                    direction = (transform.position - knockbackObject.transform.position).normalized;
                    velocity = -direction * expForce;

                    if (knockbackObject.CompareTag("Enemy"))
                    {

                        knockbackObject.GetComponent<TD2D_Enemy>().Jump(velocity, pushTime, false);

                    }
                    else
                    {

                        knockbackObject.GetComponent<TD2D_PlayerController>().Jump(velocity, pushTime);

                    }

                    StartCoroutine(Knockback());

                    IEnumerator Knockback()
                    {

                        //direction = (transform.position - knockbackObject.transform.position).normalized;
                        //rb.velocity = -direction * expForce;

                        yield return new WaitForSeconds(pushTime);

                        ObjectPooler.Instance.Despawn(gameObject);

                        //Destroy(gameObject);

                    }


                }
            }
        }


        TD2D_Enemy mime = ObjectPooler.Instance.Spawn(mimeTurretPrefab, transform.position, mimeTurretPrefab.transform.rotation).GetComponent<TD2D_Enemy>();
        EnemySpawner.Instance.activeEnemies.Add(mime);
    }


    void DealDamage()
    {


        //if its an enemy do stuff
        //killedObject.GetComponent<TD2D_Enemy>().Death();

        // if player do stuff
        TD2D_PlayerController.Instance.PlayerHealth.TakeDamage(1);


    }


    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, killRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, knockbackRadius);
        }
    }


}

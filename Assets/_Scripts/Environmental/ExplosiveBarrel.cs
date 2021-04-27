using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ExplosiveBarrel : MonoBehaviour
{
    public float shakeAmt, shakeLength;
    public GameObject scorchMark; 
    //public float countDown;

    public float expForce;

    private Vector3 direction;
    private Vector3 velocity;
    public float pushTime = 0.5f; 

    public GameObject expParticle;
    public GameObject bang;

    public Animator barrelAnim;

    public float killRadius = 3;
    public float knockbackRadius = 5;


    public LayerMask killLayers;
    public LayerMask knockbackLayers;

    public bool drawGizmos = false;



    private void Awake()
    {

        barrelAnim = GetComponent<Animator>();

    }


    void Explode()
    {

        {
            //instaniate visual and audio feedback
            //Instantiate(expParticle, transform.position, transform.rotation);
            //GameManager.Instance.cameraShaker.Shake(shakeAmt, shakeLength);

            barrelAnim.SetTrigger("Boom");
            StartCoroutine(bangEffect());
            AudioManager.Instance.barrelExplodeEv.start();


            // Get all colliders in area
            Collider2D[] killObjects = Physics2D.OverlapCircleAll(transform.position, killRadius, killLayers);

            foreach (Collider2D killedObject in killObjects)
            {

                Instantiate(scorchMark, killedObject.transform.position, transform.rotation);
                killedObject.GetComponent<TD2D_Enemy>().Death();

            }

            // Get all colliders in area
            Collider2D[] knockbackObjects = Physics2D.OverlapCircleAll(transform.position, knockbackRadius, knockbackLayers);

            foreach (Collider2D knockbackObject in knockbackObjects)
            {

                //Rigidbody2D rb = knockbackObject.GetComponent<Rigidbody2D>();

                //if (rb == null)
                //{

                //    print("Error! Incorrect object in knockback layer or does not have a rigidbody!");

                //}
                //else
                //{

                    direction = (transform.position - knockbackObject.transform.position).normalized;
                    velocity = -direction * expForce;

                    if (knockbackObject.CompareTag("Enemy"))
                    {

                        knockbackObject.GetComponent<TD2D_Enemy>().Jump(velocity,pushTime,false);


                    }
                    else 
                    {

                        knockbackObject.GetComponent<TD2D_PlayerController>().Jump(velocity, pushTime);

                    }


                    //StartCoroutine(Knockback());

                    //IEnumerator Knockback()
                    //{

                    //    //direction = (transform.position - knockbackObject.transform.position).normalized;
                    //    //rb.velocity = -direction * expForce;

                    //    yield return new WaitForSeconds(pushTime);

                    //    //ObjectPooler.Instance.Despawn(gameObject);
                    //    //Destroy(gameObject);

                    //}
                //}
            }
        }

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


    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("PlayerBullet"))
        {

            PreExplode();

        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("PlayerBullet"))
        {

            PreExplode();

        }
    }


    public void PreExplode()
    {

        Explode();
        GetComponent<Collider2D>().enabled = false;
        AstarPath.active.Scan();

    }

    private IEnumerator bangEffect()
    {
        bang.SetActive(true);
        yield return new WaitForSeconds(1);
        bang.SetActive(false);
    }


}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletRoman : MonoBehaviour
{

    private Sprite defaultSprite;
    public Sprite muzzleFlash;

    public int framesToFlash = 3;
    public float destroyTime = 10;

    public float killRadius = 1;
    public float knockbackRadius = 3;


    public float shakeAmt, shakeLength;
    public float countDown;

    public float expForce;

    private Vector3 direction;

    public float pushTime = 4f;

    public LayerMask killLayers;
    public LayerMask knockbackLayers;

    public GameObject expParticle;
    public GameObject expParticle2;

    public int minCollisions = 1;
    public int maxCollisions = 5;

    private int collisionNo = 0;
    private int collisionTarget = 0;

    private SpriteRenderer spriteRend;

    // Use this for initialization
    void Start()
    {
        TrailRenderer line = GetComponent<TrailRenderer>();
        line.sortingLayerName = "Foreground";

        spriteRend = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRend.sprite;

        StartCoroutine(FlashMuzzleFlash());
        StartCoroutine(BulletDestruction());


        collisionTarget = Random.Range(minCollisions, maxCollisions);

    }

    IEnumerator FlashMuzzleFlash()
    {
        spriteRend.sprite = muzzleFlash;

        for (int i = 0; i < framesToFlash; i++)
        {
            yield return 0;
        }

        spriteRend.sprite = defaultSprite;
    }

    IEnumerator BulletDestruction()
    {
        yield return new WaitForSeconds(destroyTime);

        Explode();

        Destroy(gameObject);
    }

    IEnumerator ProjectileLifetime()
    {
        yield return new WaitForSeconds(3);
        print("destroy firework");
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        Explode();




        if (collisionNo >= collisionTarget)
        {

            StopCoroutine(BulletDestruction());

            Destroy(gameObject);

        }
        else
        {
            collisionNo++;

        }
    }


    void Explode()
    {

        {
            //AudioManager.Instance.dragonCannonBulletEv.start();
            StartCoroutine(ProjectileLifetime());
            //instaniate visual and audio feedback
            if (Random.Range(0, 2) == 0)
                Instantiate(expParticle, transform.position, transform.rotation);
            else
                Instantiate(expParticle2, transform.position, transform.rotation);



            GameManager.Instance.cameraShaker.Shake(shakeAmt, shakeLength);

            // Get all colliders in area
            Collider2D[] killObjects = Physics2D.OverlapCircleAll(transform.position, killRadius, killLayers);

            foreach (Collider2D killedObject in killObjects)
            {

                // TODO reference enemy die part
                Destroy(killedObject);

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

                    Pathfinding.AILerp aiLerp = knockbackObject.GetComponent<Pathfinding.AILerp>();

                    if (aiLerp != null)
                    {

                        knockbackObject.gameObject.GetComponent<TD2D_Enemy>().StopEnemyAI(1);

                    }

                    StartCoroutine(Knockback());

                    IEnumerator Knockback()
                    {

                        direction = (transform.position - knockbackObject.transform.position).normalized;
                        rb.velocity = -direction * expForce;

                        yield return new WaitForSeconds(pushTime);

                        if (rb != null)
                        {
                            rb.velocity = Vector3.zero;
                        }
                        if (aiLerp != null)
                        {
                            aiLerp.canMove = true;
                        }
                    }


                }

            }

            Destroy(gameObject);

        }

    }

}

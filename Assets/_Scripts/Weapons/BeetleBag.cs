using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetleBag : MonoBehaviour
{
    public float delay;
    float countDown;
    public bool hasExploded = false;
    public float throwSpeed = 10;
    public float throwMultiplier = 20;

    public Rigidbody2D rb;
    public bool ready;
    public Animator anim;
    public float lifeSpan = 15;
    public GameObject prefab;

    public GameObject poofParticle;
    public BeetleBehaviour[] bugs;

    public GameObject bossBeetlePrefab;
    public BossBeetle bossBeetle;

    // Start is called before the first frame update
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(Countdown());
        Throw();
        StartCoroutine(BeetlesTimeout());
    }

    public void Throw()
    {
        // AudioManager.Instance.beetleBombEv.start();
        //rb.AddForce(transform.right * 15);
        anim.SetTrigger("Throw");

    }

    void Spawn()
    {

        bossBeetle = ObjectPooler.Instance.Spawn(bossBeetlePrefab, transform.position, Quaternion.identity).GetComponent<BossBeetle>();
        bossBeetle.beetleBag = this;

        bugs = new BeetleBehaviour[5];
        for (int i = 0; i < bugs.Length; i++)
        {

            bugs[i] = ObjectPooler.Instance.Spawn(prefab, transform.position, Quaternion.identity).GetComponent<BeetleBehaviour>();
            // Set speed

        }
        hasExploded = false;
    }

    public void SetTarget(Transform _enemy)
    {

        if (_enemy == null) //Don't attempt to set the enemy if there is none
            return;

        for (int i = 0; i < bugs.Length; i++)
        {
            bugs[i].SetAIDestComponent();
            bugs[i].SetAiTarget(_enemy);
            //reference ai script
        }
    }


    void Explode()
    {
        anim.SetTrigger("Explode");
        Instantiate(poofParticle, transform.position, transform.rotation);
        Spawn();
        hasExploded = true;
        gameObject.SetActive(false);
    }

    IEnumerator Countdown()
    {
        yield return new WaitForSeconds(delay);
        Explode();
        hasExploded = true;

    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.CompareTag("Enemy"))
        {
            Explode();
        }
        else
        {
            rb.velocity = Vector2.zero;
            rb.constraints = RigidbodyConstraints2D.FreezePosition;
        }
    }

    public void DestoryBeetles()
    {
        for (int i = 0; i < bugs.Length; i++)
        {
            // TODO animation for betteles dissapearing here
            ObjectPooler.Instance.Despawn(bugs[i].gameObject);
        }
    }

    IEnumerator BeetlesTimeout()
    {
        yield return new WaitForSeconds(lifeSpan);
        DestoryBeetles();
    }


}

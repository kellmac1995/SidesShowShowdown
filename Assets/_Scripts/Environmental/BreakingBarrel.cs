using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingBarrel : MonoBehaviour
{

    public Animator breakAnimation;
    //public GameObject poof;

    public bool respawnable = false;


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("PlayerBullet"))
        {

            Break();

        }

    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.CompareTag("PlayerBullet"))
        {

            Break();

        }
    }


    public void Break()
    {

        gameObject.GetComponent<Collider2D>().enabled = false;
        breakAnimation.SetTrigger("Break");

        AudioManager.Instance.barrelShatterEv.start();
        if (!GameManager.Instance.isShopping)
            AstarPath.active.Scan();

        if (respawnable)
        {

            StartCoroutine(Respawn());

        }

    }


    IEnumerator Respawn()
    {

        yield return new WaitForSeconds(2);

        gameObject.GetComponent<Collider2D>().enabled = true;
        ObjectPooler.Instance.Spawn(GameManager.Instance.despawnPoof, transform.position, Quaternion.identity);
        breakAnimation.SetTrigger("Reset");

    }


}

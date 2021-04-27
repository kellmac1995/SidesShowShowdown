using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splatter : MonoBehaviour
{
    //public LayerMask e_layer;

    //public float stickRadius = 2;

    public float lifeTime= 5;

    private List<TD2D_Enemy> stuckEnemies = new List<TD2D_Enemy>();


    private void OnEnable()
    {
        if (GameManager.Instance.isPlaying)
        {
            StartCoroutine(DestroySplatter());
            AudioManager.Instance.bubblePopEv.start();
        }
    }



    IEnumerator DestroySplatter()
    {
        yield return new WaitForSeconds(lifeTime);


        for (int i = 0; i < stuckEnemies.Count; i++)
        {

            if (stuckEnemies[i].gameObject.activeSelf)
            {

                stuckEnemies[i].UnStick();

            }

            stuckEnemies.Remove(stuckEnemies[i]);


        }


        //foreach (TD2D_Enemy enemy in stuckEnemies)
        //{

        //    if (enemy.gameObject.activeSelf)
        //    {

        //        enemy.UnStick();

        //    }

        //    stuckEnemies.Remove(enemy);

        //}

        AudioManager.Instance.bubblePopEv.start();
        ObjectPooler.Instance.Despawn(gameObject); 
    }



    public void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Enemy"))
        {

            TD2D_Enemy enemy = col.GetComponent<TD2D_Enemy>();
            enemy.Stick();
            stuckEnemies.Add(enemy);

        }
    }

}

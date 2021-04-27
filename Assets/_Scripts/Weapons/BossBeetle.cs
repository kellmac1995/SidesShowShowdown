using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBeetle : MonoBehaviour
{
    public BeetleBag beetleBag;
    public float lifeTime = 15;

    private bool enemyFound = false;
    private BeetleBehaviour bossBeet;
    private TD2D_Enemy closestEnemy = null;

    void Start()
    {
        bossBeet = GetComponent<BeetleBehaviour>();
        bossBeet.SetAIDestComponent();
        StartCoroutine(WaitForDestroyBeetles());

    }

    private void Update()
    {
        if (closestEnemy == null || closestEnemy != null && closestEnemy.gameObject.activeSelf == false) //&& enemyFound || !enemyFound
        {
            beetleBag.SetTarget(FindClosestEnemy());
        }
    }


    public Transform FindClosestEnemy()
    {
        float distanceToClosestEnemy = Mathf.Infinity;

        closestEnemy = null;

        TD2D_Enemy[] allEnemies = FindObjectsOfType<TD2D_Enemy>();
        foreach (TD2D_Enemy currentEnemy in allEnemies)
        {
            float distanceToEnemy = (currentEnemy.transform.position - this.transform.position).sqrMagnitude;
            if (distanceToEnemy < distanceToClosestEnemy)
            {
                //print("found em");
                distanceToClosestEnemy = distanceToEnemy;
                closestEnemy = currentEnemy;
                //Debug.DrawLine(this.transform.position, closestEnemy.transform.position);
            }

        }

        if (closestEnemy == null)
        {

            //print("none found");
            enemyFound = false;
            return null;

        }
        else
        {

            //print(closestEnemy.transform.name);
            enemyFound = true;
            bossBeet.SetAiTarget(closestEnemy.transform);

            return closestEnemy.transform;


        }
    }



    IEnumerator WaitForDestroyBeetles()
    {

        yield return new WaitForSeconds(lifeTime);

        beetleBag.DestoryBeetles();
        Destroy(beetleBag.gameObject);
        Destroy(gameObject);

    }
}

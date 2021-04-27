using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArenaObject : MonoBehaviour
{

    public float despawnRateMin = 0.1f;
    public float despawnRateMax = 0.5f;
   // public GameObject poof;


    private void Start()
    {

        ArenaManager.instance.arenaObjects.Add(this);

    }


    /// <summary>
    /// Despawn an object in a cloud of smoke.
    /// </summary>
    public virtual void Despawn()
    {

        ObjectPooler.Instance.Spawn(GameManager.Instance.despawnPoof, transform.position, Quaternion.identity);
       // StartCoroutine(poofEffect());
        gameObject.SetActive(false);

    }


    /// <summary>
    /// Despawns object within a time range.
    /// </summary>
    /// <param name="_minTime">Minimum amount of time before the object despawns</param>
    /// <param name="_maxTime">Maximum amount of time before the object despawns</param>
    public virtual void Despawn(float _minTime, float _maxTime)
    {

        StartCoroutine(DoTimedDespawn(_minTime, _maxTime));

    }


    IEnumerator DoTimedDespawn(float _minTime, float _maxTime)
    {

        yield return new WaitForSeconds(Random.Range(_minTime, _maxTime));

        Despawn();

    }

    //private IEnumerator poofEffect()
    //{
    //    poof.SetActive(true);
    //    yield return new WaitForSeconds(1);
    //    poof.SetActive(false);
    //}


}

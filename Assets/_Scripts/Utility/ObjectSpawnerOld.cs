using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    //public GameObject[] Objects;
    public GameObject enemyPrefab;

    public Transform spawnPoint;

    int ObjectGenerator;
    public int StartWait;

    public float SpawnWait;
    public float SpawnMostWait;
    public float SpawnLeastWait;

    public bool isSpawning = true;



    // Use this for initialization
    void Start()
    {
        StartCoroutine(Spawner());
    }

    // Update is called once per frame
    void Update()
    {
        SpawnWait = Random.Range(SpawnLeastWait, SpawnMostWait);
    }

    IEnumerator Spawner()
    {// function to generate random enemy spawning within a set area
        yield return new WaitForSeconds(StartWait);


        if (isSpawning)
        {
            //ObjectGenerator = Random.Range(0, Objects.Length);
            //Instantiate(Objects[ObjectGenerator], spawnPoint.position, gameObject.transform.rotation);
            //var bullet = 
                Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);

        }
        StartCoroutine(Spawner());
    }
}

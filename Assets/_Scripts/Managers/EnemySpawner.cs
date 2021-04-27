using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : GenericSingletonClass<EnemySpawner>
{
    public enum SpawnState { OFF, WAITING, SPAWNING };
    public enum EnemyType { Duck, Elephant, Mime };

    //A list of all of the spawned enemies that are active.
    [HideInInspector]
    public List<TD2D_Enemy> activeEnemies;

    [HideInInspector]
    public List<TD2D_Enemy_Charge> activePhantEnemies;

    [Tooltip("Random time range between spawns for a burst, X = minTime, Y = maxTime. Used to stop spawning in each other.")]
    public Vector2 burstSpawnInterval = new Vector2(0.05f, 0.25f);


    [System.Serializable]
    public class Enemy
    {

        public TD2D_Enemy enemyPrefab;

        [Tooltip("How many enemies per minute will spawn.")]
        [MinMax(1, 250, ShowEditRange = false, ShowDebugValues = true)]
        public Vector2 spawnRate = new Vector2(0, 1);


        [Tooltip("Time to wait before spawning the first batch.")]
        public float preSpawnWaitTime = 0;
        

        [Tooltip("The current difficulty level, calculated using the spawn rate range and entering a percentage, eg. spawn range 50 - 100 @ 50% would be 75 spawns per minute.")]
        [Range(1, 100)]
        public int difficultyLevel = 50;


        [Tooltip("The amount of these enemies to spawn at once.")]
        public int inMultiplesOf = 4;

        [HideInInspector]
        public SpawnState currentState;

        [HideInInspector]
        public float currentWaitTime;


        public float CalculateSpawnRate()
        {

            float range = spawnRate.y - spawnRate.x;

            float rangeAdjusted = range + spawnRate.x;

            if (range > 0)
            {

                rangeAdjusted = (range * (difficultyLevel / 100)) + spawnRate.x;

            }

            //TODO make sure pre time is >= than 0;
            float baseWaitTime = GameManager.Instance.countDown / rangeAdjusted;

            currentWaitTime = baseWaitTime * inMultiplesOf;
            
            return currentWaitTime;

        }


    }


    public List<Enemy> enemies;


    [Tooltip("All spawn point gameobjects should be listed here.")]
    public Transform[] spawnPoints;
    
    public bool debugging;



    private SpawnState state = SpawnState.OFF;
    public SpawnState State
    {
        get { return state; }
    }


    public override void Awake()
    {
        base.Awake();
    }

    void Start()
    {

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("There are no spawn points in the list.");
        }

        state = SpawnState.OFF;

        //StartSpawner();

    }


    public void StartSpawner()
    {

        state = SpawnState.SPAWNING;

        foreach (Enemy enemy in enemies)
        {

            if (enemy.preSpawnWaitTime > 0)
            {
                StartCoroutine(WaitBeforeSpawn(enemy));
            }
            else
            {
                StartCoroutine(SpawnEnemyBatch(enemy));
            }
        }

    }


    IEnumerator WaitBeforeSpawn(Enemy enemy)
    {

        yield return new WaitForSeconds(enemy.preSpawnWaitTime);

        StartCoroutine(SpawnEnemyBatch(enemy));

    }


    IEnumerator SpawnEnemyBatch(Enemy enemy)
    {

        ////Spawn enemies in a burst (multiples of).
        SpawnEnemies(enemy.enemyPrefab.gameObject, enemy.inMultiplesOf);

        yield return new WaitForSeconds(enemy.CalculateSpawnRate());

        if (state == SpawnState.SPAWNING)
            StartCoroutine(SpawnEnemyBatch(enemy));

    }



    private void SpawnEnemy(GameObject enemyPrefab)
    {

        // Choose a random spawn point from the spawnpoints list
        Transform _spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];

        GameObject newEnemy = ObjectPooler.Instance.Spawn(enemyPrefab, _spawn.position, Quaternion.identity);

        activeEnemies.Add(newEnemy.GetComponent<TD2D_Enemy>());

        TD2D_Enemy_Charge chargeComp = newEnemy.GetComponent<TD2D_Enemy_Charge>();
        if (chargeComp)
        {

            activePhantEnemies.Add(chargeComp);

        }

    }


    /// <summary>
    /// Spawns a certain amount of enemies with the specified enemy prefab.
    /// </summary>
    /// <param name="enemyPrefab">Enemy prefab to spawn</param>
    /// <param name="amount">Amount of enemies to spawn.</param>
    public void SpawnEnemies(GameObject enemyPrefab, int amount)
    {

        StartCoroutine(SpawnBurst(enemyPrefab, amount, burstSpawnInterval.x, burstSpawnInterval.y));

    }



    IEnumerator SpawnBurst(GameObject enemyPrefab, int amount, float minTime, float maxTime)
    {

        for (int i = 0; i < amount; i++)
        {

            SpawnEnemy(enemyPrefab);

            yield return new WaitForSeconds(Random.Range(minTime, maxTime));            

        }

    }

    

    /// <summary>
    /// Despawns all enemies.
    /// </summary>
    public void DeSpawnAllEnemies()
    {

        StopAllCoroutines();

        foreach (TD2D_Enemy enemy in activeEnemies)
        {

            enemy.Despawn();

        }

    }


    /// <summary>
    /// Despawns all active enemies within given time range.
    /// </summary>
    /// <param name="_minTime"></param>
    /// <param name="_maxTime"></param>
    public void DeSpawnAllEnemies(float _minTime, float _maxTime)
    {

        StopAllCoroutines();

        foreach (TD2D_Enemy enemy in activeEnemies)
        {

            enemy.Despawn(_minTime, _maxTime);

        }
    }


    private void OnDestroy()
    {

        StopAllCoroutines();

    }


}
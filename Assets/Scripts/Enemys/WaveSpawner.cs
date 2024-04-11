using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public string waveName;

        public int enemyAmount;
        public GameObject[] enemyTypes;

        public float delayBetweenSpawns;

        public GameObject GetRandomEnemy()
        {
            int randomNumber = Random.Range(0, enemyTypes.Length);
            return enemyTypes[randomNumber];
        }
    }

    public enum SpawnState {SPAWNING, WAITING, COUNTING};
    private SpawnState state = SpawnState.COUNTING;

    public GameObject[] spawnAreas;

    public Wave[] waves;
    private int nextWave;

    public float timeBetweenWaves = 5f;
    private float waveCountdown;

    private float searchCountdown = 1f;

    public bool loop;

    private PlayerController playerController;
    private UIHandler uiHandler;

    private void Start()
    {
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        uiHandler = GameObject.FindObjectOfType<UIHandler>();
        waveCountdown = timeBetweenWaves;
        uiHandler.ShowWaveDisplayText(nextWave + 1, waves.Length);
    }

    private void Update()
    {
        if(state == SpawnState.WAITING)
        {
            if(!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if(waveCountdown <= 0)
        {
            if(state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    void WaveCompleted()
    {
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if(nextWave+1 > waves.Length-1)
        {
            if(loop)
            {
                nextWave = 0;
            }
            else
            {
                playerController.Win();
            }
        }
        else
        {
            nextWave++;
            uiHandler.ShowWaveDisplayText(nextWave+1, waves.Length);
        }
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if(searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        state = SpawnState.SPAWNING;

        for(int i=0; i < _wave.enemyAmount; i++)
        {
            SpawnEnemy(_wave.GetRandomEnemy());
            yield return new WaitForSeconds(_wave.delayBetweenSpawns);
        }

        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy(GameObject enemy)
    {
        Instantiate(enemy, FindSpawnPoint(enemy.GetComponent<BoxCollider>().size), Quaternion.identity);
    }

    private Vector3 FindSpawnPoint(Vector3 enemySize)
    {
        Vector3 spawnPoint = new Vector3(0, 0, 0);

        bool spawnpointFound = false;

        int trys = 0;
        while (!spawnpointFound && trys <= 50)
        {
            Transform randomArea = spawnAreas[Random.Range(0, spawnAreas.Length)].transform;

            Vector3 scale = randomArea.lossyScale;

            float x1 = randomArea.position.x - (scale.x / 2);
            float x2 = randomArea.position.x + (scale.x / 2);
            float x = Random.Range(x1, x2);

            float z1 = randomArea.position.z - (scale.z / 2);
            float z2 = randomArea.position.z + (scale.z / 2);
            float z = Random.Range(z1, z2);

            spawnPoint = new Vector3(x, randomArea.position.y, z);

            Collider[] hitColliders = Physics.OverlapBox(spawnPoint, enemySize/2, Quaternion.identity);

            if(hitColliders.Length <= 0)
            {
                spawnpointFound = true;
            }

            trys++;
            Debug.Log("try: " + trys);
        }

        return spawnPoint;
    }
}

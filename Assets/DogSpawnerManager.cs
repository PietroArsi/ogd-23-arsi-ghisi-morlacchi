using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class DogSpawnerManager : NetworkBehaviour
{
    public float spawnInterval = 30f;
    private ActionCooldown spawnTimer;

    //[System.Serializable]
    //public struct EnemyEntry {
    //    public GameObject prefab;
    //}
    //public List<EnemyEntry> enemiesToSpawn;
    public GameObject enemy;
    public List<Transform> spawnPositions;

    void Start()
    {
        //LucaAddition
        if (!IsHost) return;

        spawnTimer = new ActionCooldown();
        spawnTimer.Set(spawnInterval);
    }

    void Update()
    {
        if (ConnectionManager.Instance != null)
        {
            if (!IsHost) return;
            if (spawnTimer.Check() && GameManagerStates.Instance.IsGamePlaying())
            {
                SpawnEnemy();
                spawnTimer.Set(spawnInterval);
            }

            spawnTimer.Advance(Time.deltaTime);
        }
        else
        {
            if (spawnTimer.Check() && GameManagerStates.Instance.IsGamePlaying())
            {
                SpawnEnemy();
                spawnTimer.Set(spawnInterval);
            }

            spawnTimer.Advance(Time.deltaTime);
        }
    }

    private void SpawnEnemy() {
        if (spawnPositions.Count > 0) {
            Vector3 spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Count)].position;
            
            if (ConnectionManager.Instance != null)
            {
                ConnectionManager.Instance.SpawnEnemyDog(spawnPosition, enemy);
            }
            else
            {
                Instantiate(enemy, spawnPosition, Quaternion.identity);
            }

        } else {
            Debug.Log("No spawn positions");
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogSpawnerManager : MonoBehaviour
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
        spawnTimer = new ActionCooldown();
        spawnTimer.Set(spawnInterval);
    }

    void Update()
    {
        if (spawnTimer.Check()) {
            SpawnEnemy();
            spawnTimer.Set(spawnInterval);
        }

        spawnTimer.Advance(Time.deltaTime);
    }

    private void SpawnEnemy() {
        if (spawnPositions.Count > 0) {
            Vector3 spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Count)].position;
            Instantiate(enemy, spawnPosition, Quaternion.identity);
        } else {
            Debug.Log("No spawn positions");
        }
    }
}

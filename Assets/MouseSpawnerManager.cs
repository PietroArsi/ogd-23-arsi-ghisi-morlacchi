using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSpawnerManager : MonoBehaviour
{
    public float spawnInterval = 30f;
    private ActionCooldown spawnTimer;
    public int spawnCount = 1;
    private AudioSource audioSource;
    public AudioClip spawnSound;

    //[System.Serializable]
    //public struct EnemyEntry {
    //    public GameObject prefab;
    //}
    //public List<float> timedSpawns;
    public GameObject mouse;
    //public List<Transform> spawnPositions;
    public Transform spawnPosition;

    public List<Transform> path;

    void Start() {
        spawnTimer = new ActionCooldown();
        spawnTimer.Set(spawnInterval);
        audioSource = GetComponent<AudioSource>();
    }

    void Update() {
        if (spawnCount > 0) {
            if (spawnTimer.Check()) {
                SpawnEnemy();
                spawnTimer.Set(spawnInterval);
            }

            spawnTimer.Advance(Time.deltaTime);
        }
    }

    private void SpawnEnemy() {
        spawnCount -= 1;
        if (spawnPosition) {
            //Vector3 spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Count)].position;
            GameObject obj = Instantiate(mouse, spawnPosition.position, Quaternion.identity);
            obj.transform.GetComponent<MouseMovement>().SetPath(path);
            obj.transform.GetComponent<MouseMovement>().SetEscape(spawnPosition.position);
            obj.transform.GetComponent<MouseMovement>().Spawn();
            PlaySpawn();
        }
        else {
            Debug.Log("No spawn positions");
        }
    }

    private void PlaySpawn() {
        audioSource.clip = spawnSound;
        audioSource.loop = false;
        audioSource.Play();
    }
}

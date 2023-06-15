using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class MouseSpawnerManager : NetworkBehaviour
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
        if (ConnectionManager.Instance != null)
        {
            //if (!IsHost) return;
            audioSource = GetComponent<AudioSource>();
        }
        else
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    void Update() {
        if (ConnectionManager.Instance != null)
        {
            if (!IsHost) return;
            if (spawnCount > 0)
            {
                if (spawnTimer.Check())
                {
                    SpawnEnemy();
                    spawnTimer.Set(spawnInterval);
                }

                spawnTimer.Advance(Time.deltaTime);
            }
        }
        else
        {
            if (spawnCount > 0)
            {
                if (spawnTimer.Check())
                {
                    SpawnEnemy();
                    spawnTimer.Set(spawnInterval);
                }

                spawnTimer.Advance(Time.deltaTime);
            }
        }
    }

    private void SpawnEnemy() {
        spawnCount -= 1;
        if (spawnPosition) {
            //Vector3 spawnPosition = spawnPositions[Random.Range(0, spawnPositions.Count)].position;

            //GameObject obj = Instantiate(mouse, spawnPosition.position, Quaternion.identity);
            if (ConnectionManager.Instance != null)
            {
                if (IsHost)
                {
                    ConnectionManager.Instance.SpawnEnemyMouse(spawnPosition.position, mouse);
                    GameObject obj = ConnectionManager.Instance.GetMouseObject();
                    //Debug.Log(obj);
                    obj.transform.GetComponent<MouseMovement>().SetPath(path);
                    obj.transform.GetComponent<MouseMovement>().SetEscape(spawnPosition.position);
                    obj.transform.GetComponent<MouseMovement>().Spawn();

                    MousePlaySpawnClientRpc();
                }
                    
            }
            else
            {
                GameObject obj = Instantiate(mouse, spawnPosition.position, Quaternion.identity);
                obj.transform.GetComponent<MouseMovement>().SetPath(path);
                obj.transform.GetComponent<MouseMovement>().SetEscape(spawnPosition.position);
                obj.transform.GetComponent<MouseMovement>().Spawn();
                PlaySpawn();
            }
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

    [ClientRpc]
    private void MousePlaySpawnClientRpc()
    {
        PlaySpawn();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;

public class PawliceMovement : NetworkBehaviour
{
    private Transform target;
    public LayerMask catnipLayer;
    private NavMeshAgent navMeshAgent;
    public float distanceFromTarget;

    public GameObject spawnMarker;
    private GameObject spawnedMarker;
    private PawliceStatus status;

    public enum PawliceStatus {
        Idle, Steal, Flee
    }

    void Start()
    {
        if (ConnectionManager.Instance != null)
        {
            if (!IsHost) return;
            navMeshAgent = GetComponent<NavMeshAgent>();
            status = PawliceStatus.Idle;
        }
        else
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            status = PawliceStatus.Idle;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ConnectionManager.Instance != null)
        {
            if (!IsHost) return;
            if (status == PawliceStatus.Idle)
            {
                CheckTarget();
                navMeshAgent.destination = target.position;
                spawnedMarker = Instantiate(spawnMarker, transform.position, Quaternion.identity);
            }

            CheckDistance();
        }
        else {
            if (status == PawliceStatus.Idle)
            {
                CheckTarget();
                navMeshAgent.destination = target.position;
                spawnedMarker = Instantiate(spawnMarker, transform.position, Quaternion.identity);
            }

            CheckDistance();
        }

    }

    private void CheckTarget() {
        //bisogna fare il check sulla catnip prendibile, non su tutte!!!
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 100, catnipLayer);

        if (hitColliders.Length > 0) {
            Transform closestCatnip = hitColliders[0].transform;
            float nearestDistance = (transform.position - hitColliders[0].transform.position).sqrMagnitude;
            float distance;

            foreach (Collider c in hitColliders) {
                distance = (transform.position - c.transform.position).sqrMagnitude;
                if (distance < nearestDistance) {
                    nearestDistance = distance;
                    closestCatnip = c.transform;
                }
            }

            status = PawliceStatus.Steal;
            target = closestCatnip.parent;
        }
        else {
            Debug.Log("No catnip found, destroying");
            Destroy(gameObject);
        }
    }

    private void CheckDistance() {
        if (status == PawliceStatus.Steal && target != null && Vector3.Distance(transform.position, target.transform.position) < distanceFromTarget) {
            if (ConnectionManager.Instance != null)
            {
                target.GetComponent<CatnipInteractable>().CollectEnemy(gameObject);
            }
            else
            {
                Debug.Log("GOT CATNIP");
            }
            OnDestinationArrival();
        } else if (status == PawliceStatus.Flee && target != null && Vector3.Distance(transform.position, target.transform.position) < distanceFromTarget) {
            OnSpawnReturn();
        }
    }

    private void OnDestinationArrival() {
        target = spawnedMarker.transform;
        navMeshAgent.destination = target.position;
        status = PawliceStatus.Flee;
        //Debug.Log("Arrived at destination");
    }

    private void OnSpawnReturn() {
        Debug.Log("Catnip stolen successfully");
        //addtion to destroy catnip Luca
       // gameObject.GetComponent<EnemyHoldCatnip>().DestroyCatnipStolen();
        if (ConnectionManager.Instance != null)
        {
            gameObject.GetComponent<EnemyHoldCatnip>().DestroyCatnipStolen();
            if (IsHost)
            {
                DestoryEnemyClientRpc();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // ServerRpc Luca to destroy enemy
    [ClientRpc]
    private void DestoryEnemyClientRpc()
    {
        Destroy(gameObject);
    }
}

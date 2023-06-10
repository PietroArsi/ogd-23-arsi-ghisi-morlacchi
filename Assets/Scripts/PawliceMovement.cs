using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PawliceMovement : MonoBehaviour
{
    private Transform target;
    public LayerMask catnipLayer;
    private NavMeshAgent navMeshAgent;
    public float distanceFromTarget;

    public GameObject spawnMarker;
    private GameObject spawnedMarker;
    private PawliceStatus status;

    public enum PawliceStatus {
        Steal, Flee
    }

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            CheckTarget();
            navMeshAgent.destination = target.position;
            spawnedMarker = Instantiate(spawnMarker, transform.position, Quaternion.identity);
        }

        CheckDistance();
    }

    private void CheckTarget() {
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
        Debug.Log(status);
        if (status == PawliceStatus.Steal && target != null && Vector3.Distance(transform.position, target.transform.position) < distanceFromTarget) {
            target.GetComponent<CatnipInteractable>().Collect();
            OnDestinationArrival();
        } else if (status == PawliceStatus.Flee && target != null && Vector3.Distance(transform.position, target.transform.position) < distanceFromTarget) {
            OnSpawnReturn();
        }
    }

    private void OnDestinationArrival() {
        target = spawnedMarker.transform;
        navMeshAgent.destination = target.position;
        status = PawliceStatus.Flee;
        Debug.Log("Arrived at destination");
    }

    private void OnSpawnReturn() {
        Debug.Log("Catnip stolen successfully");
        Destroy(gameObject);
    }
}

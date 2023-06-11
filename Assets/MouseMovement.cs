using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MouseMovement : MonoBehaviour
{
    public Transform spawn;
    public List<Transform> path;
    public Transform escape;
    public int repeatCount = -1;
    private NavMeshAgent navMeshAgent;
    private Dictionary<Vector3, int> memory;

    private MouseStatus status;
    public enum MouseStatus {
        Hidden, Move, Flee
    }

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        transform.position = spawn.position;
        status = MouseStatus.Hidden;
        memory = new Dictionary<Vector3, int>();

        //if (repeatCount < 1) {
        //    repeatCount = 1;
        //}
    }

    void Update()
    {
        if (status == MouseStatus.Hidden) {
            Spawn();
        }

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
            ContinuePath();
        }
    }

    public void Spawn() {
        status = MouseStatus.Move;
        navMeshAgent.SetDestination(path[0].position);
        memory.Add(path[0].position, 1);
    }

    private void ContinuePath() {
        if (status == MouseStatus.Move) {
            Transform current = path[0];
            path.RemoveAt(0);
            path.Add(current);

            if (memory.ContainsKey(path[0].position)) {
                if (memory[path[0].position] >= repeatCount) {
                    status = MouseStatus.Flee;
                    navMeshAgent.SetDestination(escape.position);
                }
                else {
                    memory[path[0].position] += 1;
                    navMeshAgent.SetDestination(path[0].position);
                }
            } 
            else {
                memory.Add(path[0].position, 1);
                navMeshAgent.SetDestination(path[0].position);
            }
        } else if (status == MouseStatus.Flee) {
            Destroy(gameObject);
        }
    }
}

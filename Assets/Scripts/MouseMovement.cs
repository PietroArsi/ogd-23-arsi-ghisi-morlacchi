using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Unity.Netcode;

public class MouseMovement : NetworkBehaviour, EnemyInteractable
{
    //public Transform spawn;
    private List<Vector3> path;
    private Vector3 escape;
    public int repeatCount = -1;
    private NavMeshAgent navMeshAgent;
    private Dictionary<Vector3, int> memory;

    [SerializeField] private int carriedScore;

    private MouseStatus status;
    public enum MouseStatus {
        Hidden, Move, Flee
    }

    void Start()
    {
        //navMeshAgent = GetComponent<NavMeshAgent>();
        //transform.position = spawn.position;
        //status = MouseStatus.Hidden;
        //memory = new Dictionary<Vector3, int>();
        //path = new List<Vector3>();

        //if (repeatCount < 1) {
        //    repeatCount = 1;
        //}
    }

    void Update()
    {
        //if (status == MouseStatus.Hidden) {
        //    Spawn();
        //}
        if (!IsHost) return;
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance) {
            ContinuePath();
        }
        if (GameManagerStates.Instance.IsGameOver())
        {
            DestroyMouseObjectServerRpc("Destory Mouse");
        }
    }

    public void SetPath(List<Transform> mousePath) {
        //path = mousePath;
        path = new List<Vector3>();
        foreach (Transform t in mousePath) {
            path.Add(t.position);
        }
    }

    public void SetEscape(Vector3 escapePosition) {
        escape = escapePosition;
    }

    public void Spawn() {
        if (ConnectionManager.Instance != null)
        {
            if (!IsHost) return;
            status = MouseStatus.Move;
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.SetDestination(path[0]);
            memory = new Dictionary<Vector3, int>();
            memory.Add(path[0], 1);
        }
        else
        {
            status = MouseStatus.Move;
            navMeshAgent = GetComponent<NavMeshAgent>();
            navMeshAgent.SetDestination(path[0]);
            memory = new Dictionary<Vector3, int>();
            memory.Add(path[0], 1);
        }
    }

    private void ContinuePath() {
        if (ConnectionManager.Instance != null)
        {
            if (!IsHost) return;
            if (status == MouseStatus.Move)
            {
                Vector3 current = path[0];
                path.RemoveAt(0);
                path.Add(current);

                if (memory.ContainsKey(path[0]))
                {
                    if (memory[path[0]] >= repeatCount)
                    {
                        status = MouseStatus.Flee;
                        navMeshAgent.SetDestination(escape);
                    }
                    else
                    {
                        memory[path[0]] += 1;
                        navMeshAgent.SetDestination(path[0]);
                    }
                }
                else
                {
                    memory.Add(path[0], 1);
                    navMeshAgent.SetDestination(path[0]);
                }
            }
            else if (status == MouseStatus.Flee)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            if (status == MouseStatus.Move)
            {
                Vector3 current = path[0];
                path.RemoveAt(0);
                path.Add(current);

                if (memory.ContainsKey(path[0]))
                {
                    if (memory[path[0]] >= repeatCount)
                    {
                        status = MouseStatus.Flee;
                        navMeshAgent.SetDestination(escape);
                    }
                    else
                    {
                        memory[path[0]] += 1;
                        navMeshAgent.SetDestination(path[0]);
                    }
                }
                else
                {
                    memory.Add(path[0], 1);
                    navMeshAgent.SetDestination(path[0]);
                }
            }
            else if (status == MouseStatus.Flee)
            {
                Destroy(gameObject);
            }
        }
    }

    public void KillEnemy(PlayerNetwork player)
    {
        DestroyMouseObjectServerRpc("Kill Mouse");
    }

    public void DestoryEnemyOnGameOver()
    {
        DestroyMouseObjectServerRpc("Kill Mouse");
    }


    [ServerRpc(RequireOwnership = false)]
    private void DestroyMouseObjectServerRpc(string message, ServerRpcParams serverRpcParams = default)
    {
        //_plant

        //Debug.Log(message);
        var clientId = serverRpcParams.Receive.SenderClientId;
        Destroy(gameObject);
        visualDebugger.AddMessage("Recive message form client: " + clientId.ToString());
        if (GameManagerStates.Instance.IsGamePlaying())
        {
            UpdateScoreClientRpc("Add SCore");
        }
    }


    // add for visual queue in the case of the catinip when collected. to   modify for multpile obj
    [ClientRpc]
    private void UpdateScoreClientRpc(string message)
    {
        //ask if only needed only to have a reference 
        GameManager gm = GameManagerStates.Instance.gameObject.GetComponent<GameManager>();
        gm.AddCatnip(carriedScore);
        visualDebugger.AddMessage(message);

    }
}

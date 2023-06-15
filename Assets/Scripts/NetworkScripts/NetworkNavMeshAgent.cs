using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class NetworkNavMeshAgent : NetworkBehaviour
{
    [SerializeField] private NavMeshAgent m_Agent;

    /// <summary>
    /// Is proximity enabled
    /// </summary>
    public bool EnableProximity = false;

    /// <summary>
    /// The proximity range
    /// </summary>
    public float ProximityRange = 50f;

    /// <summary>
    /// The delay in seconds between corrections
    /// </summary>
    public float CorrectionDelay = 3f;

    //TODO rephrase.
    /// <summary>
    /// The percentage to lerp on corrections
    /// </summary>
    [Tooltip("Everytime a correction packet is received. This is the percentage (between 0 & 1) that we will move towards the goal.")]
    public float DriftCorrectionPercentage = 0.1f;

    /// <summary>
    /// Should we warp on destination change
    /// </summary>
    public bool WarpOnDestinationChange = false;

    private void Awake()
    {
        m_Agent = GetComponent<NavMeshAgent>();

    }

    private Vector3 m_LastDestination = Vector3.zero;
    private float m_LastCorrectionTime = 0f;

    private void Update()
    {
        if (IsHost)
        {
            if (m_Agent.destination != m_LastDestination)
            {
                m_LastDestination = m_Agent.destination;
                if (!EnableProximity)
                {
                    visualDebugger.AddMessage("NetworkNavMeshAgent: CLIENT RPC");
                    OnNavMeshStateUpdateClientRpc(m_Agent.destination, m_Agent.velocity, transform.position);
                }
                else
                {
                    var proximityClients = new List<ulong>();
                    foreach (KeyValuePair<ulong, NetworkClient> client in NetworkManager.Singleton.ConnectedClients)
                    {
                        if (client.Value.PlayerObject == null || Vector3.Distance(client.Value.PlayerObject.transform.position, transform.position) <= ProximityRange)
                        {
                            proximityClients.Add(client.Key);
                        }
                    }
                    Debug.Log("CALL CLIENT RPC");
                    OnNavMeshStateUpdateClientRpc(m_Agent.destination, m_Agent.velocity, transform.position, new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = proximityClients.ToArray() } });
                }
            }

            if (NetworkManager.Singleton.ServerTime.FixedDeltaTime - m_LastCorrectionTime >= CorrectionDelay)
            {
                if (!EnableProximity)
                {
                    visualDebugger.AddMessage("NetworkNavMeshAgent: CLIENT RPC");
                    OnNavMeshCorrectionUpdateClientRpc(m_Agent.velocity, transform.position);
                }
                else
                {
                    var proximityClients = new List<ulong>();
                    foreach (KeyValuePair<ulong, NetworkClient> client in NetworkManager.Singleton.ConnectedClients)
                    {
                        if (client.Value.PlayerObject == null || Vector3.Distance(client.Value.PlayerObject.transform.position, transform.position) <= ProximityRange)
                        {
                            proximityClients.Add(client.Key);
                        }
                    }
                    visualDebugger.AddMessage("NetworkNavMeshAgent: CLIENT RPC");
                    OnNavMeshCorrectionUpdateClientRpc(m_Agent.velocity, transform.position, new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = proximityClients.ToArray() } });
                }

                m_LastCorrectionTime = NetworkManager.Singleton.ServerTime.FixedDeltaTime;
            }
        }
    }

    public IEnumerator CheckNavMeshAgent()
    {
        while (true)
        {
            if (m_Agent.destination != m_LastDestination)
            {
                m_LastDestination = m_Agent.destination;
                if (!EnableProximity)
                {
                    visualDebugger.AddMessage("NetworkNavMeshAgent: CLIENT RPC");
                    OnNavMeshStateUpdateClientRpc(m_Agent.destination, m_Agent.velocity, transform.position);
                }
                else
                {
                    var proximityClients = new List<ulong>();
                    foreach (KeyValuePair<ulong, NetworkClient> client in NetworkManager.Singleton.ConnectedClients)
                    {
                        if (client.Value.PlayerObject == null || Vector3.Distance(client.Value.PlayerObject.transform.position, transform.position) <= ProximityRange)
                        {
                            proximityClients.Add(client.Key);
                        }
                    }
                    Debug.Log("CALL CLIENT RPC");
                    OnNavMeshStateUpdateClientRpc(m_Agent.destination, m_Agent.velocity, transform.position, new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = proximityClients.ToArray() } });
                }
            }

            if (NetworkManager.Singleton.ServerTime.FixedDeltaTime - m_LastCorrectionTime >= CorrectionDelay)
            {
                if (!EnableProximity)
                {
                    visualDebugger.AddMessage("NetworkNavMeshAgent: CLIENT RPC");
                    OnNavMeshCorrectionUpdateClientRpc(m_Agent.velocity, transform.position);
                }
                else
                {
                    var proximityClients = new List<ulong>();
                    foreach (KeyValuePair<ulong, NetworkClient> client in NetworkManager.Singleton.ConnectedClients)
                    {
                        if (client.Value.PlayerObject == null || Vector3.Distance(client.Value.PlayerObject.transform.position, transform.position) <= ProximityRange)
                        {
                            proximityClients.Add(client.Key);
                        }
                    }
                    visualDebugger.AddMessage("NetworkNavMeshAgent: CLIENT RPC");
                    OnNavMeshCorrectionUpdateClientRpc(m_Agent.velocity, transform.position, new ClientRpcParams { Send = new ClientRpcSendParams { TargetClientIds = proximityClients.ToArray() } });
                }

                m_LastCorrectionTime = NetworkManager.Singleton.ServerTime.FixedDeltaTime;
            }
            yield return null;
        }


    }
    [ClientRpc]
    private void OnNavMeshStateUpdateClientRpc(Vector3 destination, Vector3 velocity, Vector3 position, ClientRpcParams rpcParams = default)
    {
        visualDebugger.AddMessage("NetworkNavMeshAgent: CLIENT RPC");
        //Debug.Log("<color=yellow> NetworkNavMeshAgent: CLIENT RPC </color>");
        m_Agent.Warp(WarpOnDestinationChange ? position : Vector3.Lerp(transform.position, position, DriftCorrectionPercentage));
        m_Agent.SetDestination(destination);
        m_Agent.velocity = velocity;
    }

    [ClientRpc]
    private void OnNavMeshCorrectionUpdateClientRpc(Vector3 velocity, Vector3 position, ClientRpcParams rpcParams = default)
    {
        visualDebugger.AddMessage("NetworkNavMeshAgent: CLIENT RPC");
        Debug.Log("CALL CLIENT RPC");
        m_Agent.Warp(Vector3.Lerp(transform.position, position, DriftCorrectionPercentage));
        m_Agent.velocity = velocity;
    }
}

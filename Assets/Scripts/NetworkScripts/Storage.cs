using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Storage : NetworkBehaviour,SpawnableObjParent
{
    [SerializeField] private GameManager _gm;
    [SerializeField] private int countCatnip;
    [SerializeField] private int scoreforPuttingInIt = 5;
    private int priority;
    private void Start()
    {
        priority = 8;
    }
    public void ClearSpawnObject()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update


    public void deliverProcessCatnip()
    {
        string message = "Deliver process catnip";
        sendMessageServerRpc(message);
    }

    public NetworkObject getNetworkObject()
    {
        throw new System.NotImplementedException();
    }

    public pickableObject GetObject()
    {
        throw new System.NotImplementedException();
    }

    public Transform getObjectFollowTransform()
    {
        throw new System.NotImplementedException();
    }

    public int GetPriority()
    {
        return priority;
    }

    public bool hasSpawnObject()
    {
        return false;
    }

    public void setspawnObject(GameObject obj)
    {
        throw new System.NotImplementedException();
    }

    [ServerRpc(RequireOwnership = false)]
    private void sendMessageServerRpc(string message, ServerRpcParams serverRpcParams = default)
    {
        //_plant

        //Debug.Log(message);
        var clientId = serverRpcParams.Receive.SenderClientId;

        visualDebugger.AddMessage("Recive message form client: " + clientId.ToString());
        UpdateScoreClientRpc("Add SCore");
    }


    // add for visual queue in the case of the catinip when collected. to   modify for multpile obj
    [ClientRpc]
    private void UpdateScoreClientRpc(string message)
    {
        //ask if only needed only to have a reference 
        _gm.GetComponent<GameManager>().AddCatnip(scoreforPuttingInIt);
        countCatnip++;
        visualDebugger.AddMessage(message);

    }
}

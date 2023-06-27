using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(ChestDetector))]
public class Storage : NetworkBehaviour,SpawnableObjParent
{
    [SerializeField] private GameManager _gm;
    [SerializeField] private int countCatnip;
    [SerializeField] private int scoreforPuttingInIt = 5;
    private int priority;
    public AudioClip depositSound;
    private void Start()
    {
        priority = 8;
    }
    public void ClearSpawnObject()
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update


    public void DeliverProcessCatnip()
    {
        if (depositSound != null)
        {
            GameObject.Find("UI sounds").transform.GetComponent<UISoundManager>().ClickSound(depositSound);
        }
        string message = "Deliver process catnip";
        SendMessageServerRpc(message);
    }

    public NetworkObject GetNetworkObject()
    {
        throw new System.NotImplementedException();
    }

    public PickableObject GetObject()
    {
        throw new System.NotImplementedException();
    }

    public Transform GetObjectFollowTransform()
    {
        throw new System.NotImplementedException();
    }

    public int GetPriority()
    {
        return priority;
    }

    public bool HasSpawnObject()
    {
        return false;
    }

    public void SetspawnObject(GameObject obj)
    {
        throw new System.NotImplementedException();
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendMessageServerRpc(string message, ServerRpcParams serverRpcParams = default)
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

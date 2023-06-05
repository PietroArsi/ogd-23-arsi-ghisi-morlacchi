using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

// Luca this script should be expanded in the case of using this script for differne network funcionality
public class CatnipPickUpNetwork : MultiplayerAdapter
{
    [SerializeField]
 
    private GameObject _gm;
    [SerializeField]
    private List<GameObject> _catnipedRemovedList;
    //public NetworkGameManager networkGameManager;

   /// <summary>
   /// This is done for only 1 plant object, need to expand for multiple objects.
   /// </summary>
   

    public static CatnipPickUpNetwork Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        //THIS HAS TO BE REMOVED IS ONLY FOR TESTING NETWORK
        
    }
    private void Start()
    {
        _gm = GameObject.Find("GameManager");
    }
    public override void OnNetworkSpawn()
    {
        //this.GetComponent<NetworkObject>().ChangeOwnership(PlayerNetwork.LocalIstance.OwnerClientId);
    }

    public override void Adapt(GameObject catnip)
    {
        sendMessageServerRpc("Send info to ServerRpc", catnip);

    }
    public void respawnCatnipNetwork(GameObject catnip)
    {
        //_catnipedRemovedList.Add(catnip);
        showCatnipServerRpc(catnip);
    }

    // RPC functionality
    [ServerRpc(RequireOwnership = false)]
    public void sendMessageServerRpc(string message, NetworkObjectReference catnip, ServerRpcParams serverRpcParams = default)
    {
        //_plant
        
        Debug.Log(message);
        var clientId = serverRpcParams.Receive.SenderClientId;
        
        visualDebugger.AddMessage("Recive message form client: "+clientId.ToString());
        UpdateScoreClientRpc("To all clients", catnip);
    }


    // add for visual queue in the case of the catinip when collected. to   modify for multpile obj
    [ClientRpc]
    private void UpdateScoreClientRpc(string message, NetworkObjectReference catnip)
    {
        //ask if only needed only to have a reference 
        _gm.GetComponent<GameManager>().AddCatnip(1);
        GameObject _currentplant = catnip;
        _catnipedRemovedList.Add(catnip);

        foreach (GameObject plant in _catnipedRemovedList)
            if (_currentplant == plant)
                plant.SetActive(false);
        visualDebugger.AddMessage(message);

    }
    [ServerRpc(RequireOwnership = false)]
    private void showCatnipServerRpc(NetworkObjectReference catnip, ServerRpcParams serverRpcParams = default)
    {

        var clientId = serverRpcParams.Receive.SenderClientId;
        visualDebugger.AddMessage("Recive message form client: " + clientId.ToString());
        spawnCatnipedToAllClientRpc(catnip);
    }

    [ClientRpc]
    private void spawnCatnipedToAllClientRpc(NetworkObjectReference catnip)
    {
        GameObject _currentplant = catnip;
        foreach (GameObject el in _catnipedRemovedList)
            if (_currentplant == el)
                el.SetActive(true);
        _catnipedRemovedList.Remove(_currentplant);

       visualDebugger.AddMessage("Spawn Catnip for other clients");
    }

}

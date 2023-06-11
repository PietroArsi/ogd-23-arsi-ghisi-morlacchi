using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;


//REALLY ROUGH Pickup objects 
public class pickableObject : NetworkBehaviour
{

   // [SerializeField]private GameObject pickedObj;
    [SerializeField] private FollowTransform targetTransform;
    [SerializeField] private SpawnableObjParent currentPickParent;

    [SerializeField] private GameObject placebleObject;
    //[SerializeField] private List<Transform> placementLocation = new List<Transform>();
    //private GameObject attachedObj;

    public bool isPlaced = false;
    // do we set a parent here for the game object?
    private void Awake()
    {
        targetTransform = GetComponent<FollowTransform>();
        //attachedObj = this.gameObject;
    }

    public void Update()
    {
       
    }

    public void setObjectParent(SpawnableObjParent currentParent)
    {
        // targetTransform.SetTargetTransform(currentParent);
        //problem with picking up object in the test this is going to be updated in the future
        setObjectParentServerRpc(currentParent.getNetworkObject());
      
    }

    [ServerRpc(RequireOwnership =false)]
    private void setObjectParentServerRpc(NetworkObjectReference currentParent, ServerRpcParams serverRpcParams = default)
    {
        

        var clientId = serverRpcParams.Receive.SenderClientId;
  
        visualDebugger.AddMessage("This is sent form: " + clientId);
        setObjectParentClientRpc(currentParent);
    }
    [ClientRpc]
    private void setObjectParentClientRpc(NetworkObjectReference currentParent)
    {
        currentParent.TryGet(out NetworkObject parentSpawn);
        SpawnableObjParent parent = parentSpawn.GetComponent<SpawnableObjParent>();
        currentPickParent = parent;
        visualDebugger.AddMessage("Update parent of the object");
        //visualDebugger.AddMessage("Parent: "+ parent);
        currentPickParent.setspawnObject(this.gameObject);
        targetTransform.SetTargetTransform(parent.getObjectFollowTransform());
    }

    public static void spawnObj(GameObject pickObj, GameObject parent)
    {
        ConnectionManager.Instance.spawnNetworkObject(pickObj, parent.GetComponent<SpawnableObjParent>());

    }

    public SpawnableObjParent currentParent()
    {
        return currentPickParent;
    }
}

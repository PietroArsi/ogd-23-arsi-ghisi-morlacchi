using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PickAndPlace : NetworkBehaviour
{
    public LayerMask interactionLayer;
    public SphereCollider checkGround;

    public event EventHandler OnPlaceObject;
    public event EventHandler OnPickUp;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void PickUpObject(PlayerNetwork player)
    {
        //synch sound pick()
        if (GameManagerStates.Instance.GetConstructionWindowActive()) return;
        Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity, interactionLayer);
        foreach (Collider c in hitColliders)
        {
            if (c.gameObject.GetComponent<PickableObject>())
            {
                
                Debug.Log("Remove Collider");
                RemoveColliderServerRpc(c.gameObject);
                c.gameObject.GetComponent<PickableObject>().currentParent().ClearSpawnObject();
                
                c.GetComponent<PickableObject>().setObjectParent(PlayerNetwork.LocalIstance);
                OnPickUp?.Invoke(this, EventArgs.Empty);
                break;
                // Debug.Log("<color=yellow>PlayerNetwork: PickUp Object</color>");
            }
            // else if (c.gameObject.GetComponent<GetWall>())
            //{

            //    c.GetComponent<GetWall>().getWall();
            //    break;
            //}
            if (c.GetComponentInParent<MouseMovement>())
            {
                Debug.Log("HELLO THERE MOUSE");
                c.GetComponentInParent<MouseMovement>().KillEnemy(null);
                break;
            }
            if (c.GetComponent<EnemyHoldCatnip>())
            {
                c.GetComponent<EnemyHoldCatnip>().KillEnemy(player);
                break;
            }
            if (c.gameObject.GetComponent<FurnaceCook>() != null && !PlayerNetwork.LocalIstance.HasSpawnObject() && c.gameObject.GetComponent<FurnaceCook>().IsCookingOver())
            {


                c.gameObject.GetComponent<FurnaceCook>().getProcessCatnip(PlayerNetwork.LocalIstance);
                break;

            }
           
        }

    }
    public void PlaceDownObject(PlayerNetwork player)
    {
        //synch sound placeDown()
        //Debug.Log("<color=yellow>PlayerNetwork Leave Object </color>");
        Collider[] checkCanPlace = Physics.OverlapSphere(checkGround.transform.position, 1f, interactionLayer);
        foreach(Collider collider in checkCanPlace)
        {
            if (collider.gameObject.GetComponent<AlreadyPlace>())
            {
                Debug.Log(collider.name);
                Debug.Log("Cannot place here");
                return;
            }
        }
        Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity, interactionLayer);
        // Sortare hit colliders in base alla priorità fare sorting list 
        if (hitColliders.Length == 0 || !PlayerNetwork.LocalIstance.HasSpawnObject())
        {
            return;
        }
        List<Collider> sortedColliders = new List<Collider>();
        foreach(Collider c in hitColliders)
        {
            if (c.GetComponent<SpawnableObjParent>() != null)
            {
                sortedColliders.Add(c);
            }
        }
        sortedColliders.Sort((a, b) => a.transform.GetComponent<SpawnableObjParent>().GetPriority().CompareTo(b.transform.GetComponent<SpawnableObjParent>().GetPriority()));
        sortedColliders.Reverse();
        if (sortedColliders[0].gameObject.GetComponent<SpawnableObjParent>() != null && !sortedColliders[0].gameObject.GetComponent<SpawnableObjParent>().HasSpawnObject())
        {
            Collider[] placedObjectColldier = Physics.OverlapSphere(checkGround.transform.position, 2f, interactionLayer);

            //Debug.Log(sortedColliders[0].name);
            
            if (sortedColliders[0].name== "Furnace" && sortedColliders[0].GetComponent<FurnaceCook>().isFunraceEmpty()
                && PlayerNetwork.LocalIstance.GetObject().GetComponent<CatnipStatus>().currentStatus == CatnipStatus.status.Unprocessed)
            {
                Debug.Log("FURNACE");
                DestroyHeldObjectServerRpc(PlayerNetwork.LocalIstance.GetObject().gameObject);
                sortedColliders[0].gameObject.GetComponent<FurnaceCook>().StartToCoock();
               
            }
            else if (sortedColliders[0].name == "chest")
            {
                if (PlayerNetwork.LocalIstance.GetObject().GetComponent<CatnipStatus>().currentStatus == CatnipStatus.status.Cut)
                {
                    // Debug.Log("Storage");
                    DestroyHeldObjectServerRpc(PlayerNetwork.LocalIstance.GetObject().gameObject);
                    sortedColliders[0].gameObject.GetComponent<Storage>().DeliverProcessCatnip();
                }
                else
                {
                    Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
                    canvas.GetComponent<GameplayMessages>().CannotStore();

                }
            }
            else if(sortedColliders[0].name == "Field" || sortedColliders[0].name.Contains("Ramp"))
            {
                //Debug.Log("Ground");
                OnPlaceObject?.Invoke(this, EventArgs.Empty);
                PlayerNetwork.LocalIstance.GetObject().setObjectParent(sortedColliders[0].GetComponent<SpawnableObjParent>());
                PlayerNetwork.LocalIstance.ClearSpawnObject();
            }
            else if (sortedColliders[0].name.Contains("place"))
            {
               // Debug.Log("Table");
               
                sortedColliders[0].GetComponent<CuttingTable>().SetToReady();
                AddColliderServerRpc(PlayerNetwork.LocalIstance.spawnObject);
                PlayerNetwork.LocalIstance.GetObject().setObjectParent(sortedColliders[0].GetComponent<SpawnableObjParent>());
                PlayerNetwork.LocalIstance.ClearSpawnObject();
            }
            else
            {
                Debug.Log("CANNOT PLACE");
            }
   
        }
    }
    public void CutCatnip(PlayerNetwork player)
    {
        Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity, interactionLayer);
        List<Collider> sortedColliders = new List<Collider>();
        foreach (Collider c in hitColliders)
        {
            if (c.GetComponent<CuttingTable>() != null)
            {
                sortedColliders.Add(c);
            }
        }
        sortedColliders.Sort((a, b) => a.transform.GetComponent<SpawnableObjParent>().GetPriority().CompareTo(b.transform.GetComponent<SpawnableObjParent>().GetPriority()));
        sortedColliders.Reverse();

        if (sortedColliders[0].GetComponent<PlaceOnTable>().HasSpawnObject() && sortedColliders[0].GetComponent<PlaceOnTable>().GetObject().GetComponent<CatnipStatus>().currentStatus == CatnipStatus.status.Cooodked)
        {
            sortedColliders[0].GetComponent<PlaceOnTable>().GetObject().gameObject.layer = 0;
            sortedColliders[0].GetComponent<CuttingTable>().StartToCut();
        }
        else
        {
            Canvas canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
            canvas.GetComponent<GameplayMessages>().CannotCut();
        }
    }


    [ServerRpc]
    private void DestroyHeldObjectServerRpc(NetworkObjectReference heldOject)
    {
        GameObject destoryObject = heldOject;
        Destroy(destoryObject);
    }


    [ServerRpc(RequireOwnership =false)]
    private void SynchSoundPickAndPlaceServerRpc(NetworkObjectReference sound)
    {
        //play sound effect
        SynchSoundPickAndPlaceClientRpc(sound);
    }
    [ClientRpc]
    private void SynchSoundPickAndPlaceClientRpc(NetworkObjectReference sound)
    {
        //play sound effect here
    }
    [ServerRpc(RequireOwnership = false)]
    private void RemoveColliderServerRpc(NetworkObjectReference picked)
    {
        GameObject newPick = picked;
        RemoveColliderClientRpc(newPick);
    }

    [ClientRpc]
    private void RemoveColliderClientRpc(NetworkObjectReference c)
    {
        visualDebugger.AddMessage("REMOVE COLLIDER FOR THIS OBJECT PLEASE");
        GameObject PickedUp = c;
        PickedUp.GetComponent<BoxCollider>().enabled = false;
    }
    [ServerRpc(RequireOwnership = false)]
    private void AddColliderServerRpc(NetworkObjectReference placed)
    {
        GameObject place = placed;
        AddColliderClientRpc(place);
    }
    [ClientRpc]
    private void AddColliderClientRpc(NetworkObjectReference c)
    {
        visualDebugger.AddMessage("Add COLLIDER FOR THIS OBJECT PLEASE");
        GameObject PickedUp = c;
        PickedUp.GetComponent<BoxCollider>().enabled = true;
    }
}


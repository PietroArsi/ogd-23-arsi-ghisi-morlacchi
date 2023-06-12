using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PickAndPlace : NetworkBehaviour
{
    public LayerMask interactionLayer;
    public SphereCollider checkGround;
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
        Collider[] hitColliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity, interactionLayer);
        foreach (Collider c in hitColliders)
        {
            if (c.gameObject.GetComponent<PickableObject>())
            {
                c.gameObject.GetComponent<BoxCollider>().enabled = false;
                c.gameObject.GetComponent<PickableObject>().currentParent().ClearSpawnObject();
                
                c.GetComponent<PickableObject>().setObjectParent(PlayerNetwork.LocalIstance);
                break;
                // Debug.Log("<color=yellow>PlayerNetwork: PickUp Object</color>");
            }
            else if (c.gameObject.GetComponent<GetWall>())
            {

                c.GetComponent<GetWall>().getWall();
                break;
            }
            if (c.gameObject.GetComponent<FurnaceCook>() != null && !player.HasSpawnObject() && c.gameObject.GetComponent<FurnaceCook>().IsCookingOver())
            {


                c.gameObject.GetComponent<FurnaceCook>().getProcessCatnip(player);
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
        if (hitColliders.Length == 0 || !player.HasSpawnObject())
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

            Debug.Log(sortedColliders[0].name);
            
            if (sortedColliders[0].name== "Furnace" && sortedColliders[0].GetComponent<FurnaceCook>().isFunraceEmpty()
                && player.GetObject().gameObject.tag == "Unprocessed")
            {
                Debug.Log("FURNACE");
                DestroyHeldObjectServerRpc(player.GetObject().gameObject);
                sortedColliders[0].gameObject.GetComponent<FurnaceCook>().StartToCoock();
               
            }
            else if (sortedColliders[0].name == "Storage" && player.GetObject().gameObject.tag == "Processed")
            {
                Debug.Log("Storage");
                DestroyHeldObjectServerRpc(player.GetObject().gameObject);
                sortedColliders[0].gameObject.GetComponent<Storage>().DeliverProcessCatnip();
            }
            else if(sortedColliders[0].name == "Cube" )
            {
                Debug.Log("Ground");
                player.GetObject().setObjectParent(sortedColliders[0].GetComponent<SpawnableObjParent>());
                player.ClearSpawnObject();
            }
            else if (sortedColliders[0].name.Contains("place"))
            {
                Debug.Log("Table");
                player.spawnObject.GetComponent<BoxCollider>().enabled = true;
                player.GetObject().setObjectParent(sortedColliders[0].GetComponent<SpawnableObjParent>());
                player.ClearSpawnObject();
            }
            else
            {
                Debug.Log("CANNOT PLACE");
            }
   
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
    private void SynchSoundPickAndPlaceClientRpc(GameObject sound)
    {
        //play sound effect here
    }
}


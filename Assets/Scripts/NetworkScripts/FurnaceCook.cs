using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;
public class FurnaceCook : NetworkBehaviour, SpawnableObjParent
{
    private NetworkVariable<float> coockingTime = new NetworkVariable<float>(5f);
    public event EventHandler OnFurnaceStateChanged;
    private NetworkVariable<FurnaceStates> state = new NetworkVariable<FurnaceStates>(FurnaceStates.Empty);
    [SerializeField]private GameObject processCatnip;
    private int priority;
   
    //private const float MAX_CATNIP_CAN_BE_COOCKED = 2;
    // Start is called before the first frame update

    private enum FurnaceStates
    {
        Empty,
        Coocking,
        Ready,
    }
    void Start()
    {
        priority = 9;
        state.Value= FurnaceStates.Empty;
    }
    public override void OnNetworkSpawn()
    {
        state.OnValueChanged += FurnaceStates_OnValueChanged;
    }

    private void FurnaceStates_OnValueChanged(FurnaceStates previousValue, FurnaceStates newValue)
    {
        OnFurnaceStateChanged?.Invoke(this, EventArgs.Empty);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsHost)
        {
            return;
        }
        switch (state.Value)
        {
            case FurnaceStates.Empty:
                {
                    coockingTime.Value = 20f;
                }
                break;
            case FurnaceStates.Coocking:
                coockingTime.Value -= Time.deltaTime;
                //Debug.Log(gamePlayingTimer.Value);
                // Debug.Log("CATNIPISCOOKING");
                Debug.Log("HERE COOKING");
                if (coockingTime.Value < 0f)
                {
                    state.Value = FurnaceStates.Ready;
                }
                break;
            case FurnaceStates.Ready:
                {
                   // Debug.Log("CATNIP IS READY");
                }
                break;
        }
    }


    public string GetCookingTime()
    {
        if (coockingTime.Value > 0f)
        {
            Debug.Log("HERE COOKING");

            //string minutes = Mathf.Floor(coockingTime.Value / 60).ToString("00");
            string seconds = Mathf.Floor(coockingTime.Value % 60).ToString("00");

            return seconds;
        }
        else
        {
            return 00 + ":" + 00;
        }
    }
    
    public bool IsCookingOver()
    {
        return state.Value == FurnaceStates.Ready;
    }
    public void StartToCoock()
    {
        if (IsClient)
        {
            startCookingServerRpc();
           
        }
    }

    public bool isFunraceEmpty()
    {
        return state.Value == FurnaceStates.Empty;
    }
    public bool IsFurnaceCooking()
    {
        return state.Value == FurnaceStates.Coocking;
    }

    public void getProcessCatnip(PlayerNetwork player)
    {
        if (IsClient)
        {
            ResetServerRpc();
        }
        
        if (!PlayerNetwork.LocalIstance.HasSpawnObject())
            PickableObject.spawnObj(processCatnip, player.gameObject);
    }

    
    [ServerRpc(RequireOwnership = false)]
    private void startCookingServerRpc()
    {
        Debug.Log("START COOCKING PLEASE");
        state.Value = FurnaceStates.Coocking;
    }
    [ServerRpc(RequireOwnership = false)]
    private void ResetServerRpc()
    {
        state.Value = FurnaceStates.Empty;
    }

    public PickableObject GetObject()
    {
        throw new NotImplementedException();
    }

    public Transform GetObjectFollowTransform()
    {
        throw new NotImplementedException();
    }

    public bool HasSpawnObject()
    {
        return false;
    }

    public NetworkObject GetNetworkObject()
    {
        throw new NotImplementedException();
    }

    public void SetspawnObject(GameObject obj)
    {
        throw new NotImplementedException();
    }

    public void ClearSpawnObject()
    {
        throw new NotImplementedException();
    }

    public int GetPriority()
    {
        return priority;
    }
}

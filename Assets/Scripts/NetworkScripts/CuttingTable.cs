using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class CuttingTable : NetworkBehaviour
{
    private NetworkVariable<float> cuttingTime = new NetworkVariable<float>(5f);
    [SerializeField] private GameObject cuttedCatnip;
    private enum TableState
    {
        Empty,
        Ready,
        Cutting,
        Complete,
    }
    // Start is called before the first frame update
    private NetworkVariable<TableState> state = new NetworkVariable<TableState>(TableState.Ready);
    void Start()
    {
        
    }

    void Update()
    {
        if (!IsHost)
        {
            return;
        }
        switch (state.Value)
        {
            case TableState.Empty:
                {
                    
                }
                break;
            case TableState.Ready:
                {
                    cuttingTime.Value = 5f;
                }
                break;
            case TableState.Cutting:
                cuttingTime.Value -= Time.deltaTime;
                //Debug.Log(gamePlayingTimer.Value);
                Debug.Log("CatnipIsCutting");
               
                if (cuttingTime.Value < 0f)
                {
                    ShowCatnipCut();
                    state.Value = TableState.Complete;
                }
                break;
            case TableState.Complete:
                {
                     Debug.Log("CATNIP IS Complete");
                }
                break;
        }
    }

    public void StartToCut()
    {
        if (IsClient)
        {
            StartCuttingServerRpc();

        }
    }

    public void SetToReady()
    {
        if (IsClient)
        {
            SetToReadyServerRpc();

        }
    }

    private void ShowCatnipCut()
    {
        Destroy(gameObject.GetComponent<PlaceOnTable>().GetObject().gameObject);
        PickableObject.spawnObj(cuttedCatnip, gameObject);
        if (gameObject.GetComponent<PlaceOnTable>().HasSpawnObject())
        {
            gameObject.GetComponent<PlaceOnTable>().GetObject().gameObject.GetComponent<Collider>().enabled = true;
        }
    }
    public void GetCutCatnip(PlayerNetwork player)
    {
        if (IsClient)
        {
            ResetServerRpc();
        }

        if (!player.HasSpawnObject())
        {
            cuttedCatnip.GetComponent<PickableObject>().setObjectParent(player);
        }
        
    }


    [ServerRpc(RequireOwnership = false)]
    private void StartCuttingServerRpc()
    {
        gameObject.GetComponent<PlaceOnTable>().GetObject().gameObject.layer = 0;
        Debug.Log("START COOCKING PLEASE");
        state.Value = TableState.Cutting;
    }
    [ServerRpc(RequireOwnership = false)]
    private void ResetServerRpc()
    {
        state.Value = TableState.Empty;
    }
    [ServerRpc(RequireOwnership = false)]
    private void SetToReadyServerRpc()
    {
        state.Value = TableState.Ready;
    }


    public bool IsTableEmpty()
    {
        return state.Value == TableState.Empty;
    }
    public bool IsCatnipOnTable()
    {
        return state.Value == TableState.Ready;
    }
    public bool IsTableCutting()
    {
        return state.Value == TableState.Cutting;
    }
    public bool IsCatnipCut()
    {
        return state.Value == TableState.Complete;
    }

    public string getCuttingTime()
    {
        if (cuttingTime.Value > 0f)
        {
            Debug.Log("HERE COOKING");

            //string minutes = Mathf.Floor(coockingTime.Value / 60).ToString("00");
            string seconds = Mathf.Floor(cuttingTime.Value % 60).ToString("00");

            return seconds;
        }
        else
        {
            return 00 + ":" + 00;
        }
    }

   
}

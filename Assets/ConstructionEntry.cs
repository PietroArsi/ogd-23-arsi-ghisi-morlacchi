using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionEntry : NetworkBehaviour
{
    public string contructionName;
    //public int amount to be decided
    public int cost;
   [SerializeField] private GameManager gameManager;
    public TMPro.TextMeshProUGUI costText;
    //public TMPro.TextMeshProUGUI amount;

    // Luca Addition for functional consturction menu
    public GameObject constructionType;
    public ConstructionMenu construction;
    void Start()
    {
        costText.text = $"{cost}";
        gameManager=GameObject.Find("GameManager").GetComponent<GameManager>();
        GetComponent<Button>().onClick.AddListener(() => Build());
    }

    public void Build() {
        Debug.Log("CLICK");
        if (gameManager != null && gameManager.GetCatnip() > cost) //AND amount!=0
        {
            //amount--;
            
            UpdateCatnipCountServerRpc();
            SpawnBlock();
        }
        else
        {
            Debug.Log("NOT ENOUGH CATNIP");
        }
    }

    private void SpawnBlock() {
        Debug.Log("CALL THIS SPAWN");
        //Debug.Log($"Build {contructionName}");
       
        PickableObject.spawnObj(constructionType, PlayerNetwork.LocalIstance.gameObject);
        construction.Hide();
    }
    [ServerRpc(RequireOwnership = false)]
    private void UpdateCatnipCountServerRpc(ServerRpcParams serverRpcParams = default)
    {
        //_plant

        //Debug.Log(message);
        var clientId = serverRpcParams.Receive.SenderClientId;
        visualDebugger.AddMessage("CALL THIS HELLO HELLO HELLO SERVER RPC");
        visualDebugger.AddMessage("Recive message form client: " + clientId.ToString());
        UpdateCatnipCountClientRpc("Subtract Score");
    }


    // add for visual queue in the case of the catinip when collected. to   modify for multpile obj
    [ClientRpc]
    private void UpdateCatnipCountClientRpc(string message)
    {

        //ask if only needed only to have a reference 
        gameManager.AddCatnip(-cost);
        
        visualDebugger.AddMessage(message);

    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CharaterSelectPlayer : MonoBehaviour
{
    [SerializeField] private TextMeshPro playerName;
    [SerializeField] private int playerIndex;
    [SerializeField] private TextMeshPro youText;
    [SerializeField] private PlayerVisual playerVisual;
    [SerializeField] private CharacterSelectColor characterSelectColor;
    private void Start()
    {
        ConnectionManager.Instance.onListPlayerDataChanged += ConnectionManager_OnPlayerDataNetworkChanged;

        UpdatePlayer();
    }

    private void ConnectionManager_OnPlayerDataNetworkChanged(object sender, EventArgs e)
    {
        UpdatePlayer();
    }

    private void UpdatePlayer()
    {
        if (ConnectionManager.Instance.IsPlayerIndexConnected(playerIndex))
        {
            PlayerData playerData = ConnectionManager.Instance.GetPlayerDataFromPlayerIndex(playerIndex);
            playerName.text = playerData.playerName.ToString();
            if (ConnectionManager.Instance.showYou(playerData.clientID))
            {
                playerName.text = playerData.playerName.ToString() + "\n(You)";
            }
            else
            {
                playerName.text = playerData.playerName.ToString();
            }
           
            //youText.gameObject.SetActive(ConnectionManager.Instance.showYou(playerData.clientID));
            playerVisual.SetPlayerColor(ConnectionManager.Instance.GetPlayerColor(playerData.colorId));
            Show();
        }
        else
        {
            Hide();
            
        }
    }
    private void Show()
    {

        gameObject.SetActive(true);


    }
    private void Hide()
    {
        gameObject.SetActive(false);
    }


    private void OnDestroy()
    {
        //Debug.Log("<color=yellow> CharacterSelectPlayer called Destroy function OnPlayerDataNetworkChanged event</color>");
        ConnectionManager.Instance.onListPlayerDataChanged -= ConnectionManager_OnPlayerDataNetworkChanged;
    }
}

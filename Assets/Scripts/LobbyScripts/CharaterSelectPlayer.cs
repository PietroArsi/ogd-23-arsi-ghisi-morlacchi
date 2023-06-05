using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CharaterSelectPlayer : MonoBehaviour
{
    [SerializeField] private TextMeshPro playerName;
    [SerializeField] private int playerIndex;
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
            Show();
        }
        else
            Hide();
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
        ConnectionManager.Instance.onListPlayerDataChanged -= ConnectionManager_OnPlayerDataNetworkChanged;
    }
}

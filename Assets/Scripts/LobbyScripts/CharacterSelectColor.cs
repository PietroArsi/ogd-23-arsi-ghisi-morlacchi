using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectColor : MonoBehaviour
{
    //[SerializeField] private int colorId;
    [SerializeField] private int playerIndex;
    [SerializeField] private int materialId;
    
    // [SerializeField] private Image image;

    private void Awake()
    {
    }
    private void Start()
    {
        ConnectionManager.Instance.onListPlayerDataChanged += ConnectionManager_onListPlayerDataChanged;
        ConnectionManager.Instance.OnSameColorPlayer += ConnectionManager_OnSameColorPlayer;
        //image.color = ConnectionManager.Instance.GetPlayerColor(colorId[currentIndex]
    }

    private void ConnectionManager_OnSameColorPlayer(object sender, System.EventArgs e)
    {
        //Debug.Log("EVENT FIRED WHEN FALSE");
        //Debug.Log("MATERIAL ID");
    }

    private void ConnectionManager_onListPlayerDataChanged(object sender, System.EventArgs e)
    {
       // GetColorId();
    }
    public void GetColorId()
    {

        if (ConnectionManager.Instance.IsPlayerIndexConnected(playerIndex))
        {
            PlayerData playerData = ConnectionManager.Instance.GetPlayerDataFromPlayerIndex(playerIndex);
            materialId = playerData.colorId;
        }
            //currentColorId=
        }  
    private void Update()
    {
      // if(SceneManager.GetActiveScene().name!=SceneLoader.Scene.NetworkTestLevel.ToString())

        GetInput();
    }
    void GetInput()
    {
        
        if (Input.GetKeyDown(KeyCode.A))
        {
            //if (!NetworkManager.Singleton.IsHost)
            //{
            materialId = materialId > 0 ? materialId - 1 : ConnectionManager.Instance.totalColors().Count - 1;
                while (!ConnectionManager.Instance.IsColorAvailable(materialId))
                {
                    materialId = materialId > 0 ? materialId - 1 : ConnectionManager.Instance.totalColors().Count - 1;
                   // Debug.Log("MAT " + materialId);
                    //Debug.Log("THIS MATERIAL ID IS NOT AVAILABLE GO TO THE NEXT ONE");
                }
                ConnectionManager.Instance.ChangePlayerColor(materialId);
                //Debug.Log("FOUND AVAILABLE MATERIAL");
            //}
            //else
            //{
            //    materialId = materialId > 0 ? materialId - 1 : ConnectionManager.Instance.totalColors().Count - 1;
            //    ConnectionManager.Instance.ChangePlayerColor(materialId);
            //}
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            materialId = materialId < ConnectionManager.Instance.totalColors().Count - 1 ? materialId + 1 : 0;
            //if (!NetworkManager.Singleton.IsHost)
            //{
                while (!ConnectionManager.Instance.IsColorAvailable(materialId))
                {
                    materialId = materialId < ConnectionManager.Instance.totalColors().Count - 1 ? materialId + 1 : 0;
                   //Debug.Log("MAT " + materialId);
                    Debug.Log("THIS MATERIAL ID IS NOT AVAILABLE GO TO THE NEXT ONE");
                }
                ConnectionManager.Instance.ChangePlayerColor(materialId);
            //}
            //else
            //{
            //    materialId = materialId < ConnectionManager.Instance.totalColors().Count - 1 ? materialId + 1 : 0;
            //    ConnectionManager.Instance.ChangePlayerColor(materialId);
            //} 
        }
    }
}

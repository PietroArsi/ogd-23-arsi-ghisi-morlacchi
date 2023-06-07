using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelectColor : MonoBehaviour
{
    [SerializeField] private int colorId;
   // [SerializeField] private Image image;

    private void Awake()
    {
    }
    private void Start()
    {
        ConnectionManager.Instance.onListPlayerDataChanged += ConnectionManager_onListPlayerDataChanged;
        //image.color = ConnectionManager.Instance.GetPlayerColor(colorId[currentIndex]);
    }

    private void ConnectionManager_onListPlayerDataChanged(object sender, System.EventArgs e)
    {
       // colorIsselected();
    }
   
    private void Update()
    {
       if(SceneManager.GetActiveScene().name!=SceneLoader.Scene.NetworkTestLevel.ToString())
            GetInput();
    }
    void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Debug.Log("<color=yellow> CharacterSelectColorA</color>");
            if (colorId==0)
            {
                colorId= ConnectionManager.Instance.totalColors().Count - 1;
                Debug.Log("<color=yellow> CharacterSelectColorA color id:" +colorId+ "</color>");
                ConnectionManager.Instance.ChangePlayerColor(colorId);
            }
            else
            {
                Debug.Log("<color=yellow> CharacterSelectColorA color id:" + colorId + "</color>");
                colorId--;
                ConnectionManager.Instance.ChangePlayerColor(colorId);
            }
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            Debug.Log("<color=yellow> CharacterSelectColorD</color>");
            if (colorId == ConnectionManager.Instance.totalColors().Count-1)
            {
                Debug.Log("<color=yellow> CharacterSelectColorD color id:" + colorId + "</color>");
                colorId = 0;
                ConnectionManager.Instance.ChangePlayerColor(colorId);
            }
            else
            {
                Debug.Log("<color=yellow> CharacterSelectColorD color id:" + colorId + "</color>");
                colorId++;
                ConnectionManager.Instance.ChangePlayerColor(colorId);
            }
        }
    }
}

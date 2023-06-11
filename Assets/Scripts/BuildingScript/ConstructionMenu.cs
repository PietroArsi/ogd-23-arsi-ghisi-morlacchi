using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstructionMenu : MonoBehaviour
{
    [SerializeField] private GameObject constructionScreen;
    
    private void Awake()
    {
        
    }
    private void Start()
    {
        Hide();

       
    }

    private void GameManagerStates_OnStateChanged(object sender, System.EventArgs e)
    {
    
    }

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        if (Input.GetKeyDown(KeyCode.Q) && !constructionScreen.activeSelf)
        {
            Show();
        }
        else if(Input.GetKeyDown(KeyCode.Q) && constructionScreen.activeSelf)
        {
            Hide();
        }
    }
    private void Show()
    {
        constructionScreen.SetActive(true);
    }
    private void Hide()
    {
        constructionScreen.SetActive(false);
    }

    public void CanBuild()
    {
        //GET SCRIPT OF THE COMPONENT
         //button.clike
        //if (!PlayerNetwork.LocalIstance.hasSpawnObject())
            //pickableObject.spawnObj(block, PlayerNetwork.LocalIstance.gameObject);
    }
}

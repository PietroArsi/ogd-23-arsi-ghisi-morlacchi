using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayMessages : MonoBehaviour
{
    public TextMeshProUGUI messageToPlayer;
    // Start is called before the first frame update
    void Start()
    {
        messageToPlayer.gameObject.SetActive(false);


    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CannotCut()
    {
        messageToPlayer.gameObject.SetActive(true);
        messageToPlayer.text = "Cannot cut, you need to cook it";
        StartCoroutine(ShowMessage());
    }
    public void CannotStore()
    {
        messageToPlayer.gameObject.SetActive(true);
        messageToPlayer.text = "Cannot store, you need to cut it";
        StartCoroutine(ShowMessage());
    }


    IEnumerator ShowMessage()
    {
        yield return new WaitForSeconds(5f);
        messageToPlayer.gameObject.SetActive(false);
    }
}

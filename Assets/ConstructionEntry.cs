using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConstructionEntry : MonoBehaviour
{
    public string contructionName;
    public int cost;
    private GameManager gameManager;

    public TMPro.TextMeshProUGUI costText;

    void Start()
    {
        costText.text = $"{cost}";

        GetComponent<Button>().onClick.AddListener(() => Build());
    }

    public void Build() {
        if (gameManager != null && gameManager.GetCatnip() > cost) {
            gameManager.AddCatnip(-cost);
            SpawnBlock();
        }

        //SpawnBlock();//debug
    }

    private void SpawnBlock() {
        Debug.Log($"Build {contructionName}");
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TableCurrentState : MonoBehaviour
{
    [SerializeField] private GameObject stateTable;
    // Start is called before the first frame update
    private void Update()
    {
        if (gameObject.GetComponent<CuttingTable>().IsTableEmpty())
        {
           // Debug.Log("EMPTY");
            stateTable.SetActive(false);
            stateTable.GetComponent<TextMeshPro>().text = "Empty";
        }
        else if (gameObject.GetComponent<CuttingTable>().IsCatnipOnTable())
        {
            stateTable.SetActive(false);
            stateTable.GetComponent<TextMeshPro>().text = "Press E";
            
        }
        else if (gameObject.GetComponent<CuttingTable>().IsTableCutting())
        {
            stateTable.SetActive(true);
            stateTable.GetComponent<TextMeshPro>().text = "Cutting: " + gameObject.GetComponent<CuttingTable>().getCuttingTime();
          
        }
        else if (gameObject.GetComponent<CuttingTable>().IsCatnipCut())
        {
            stateTable.SetActive(false);
            stateTable.GetComponent<TextMeshPro>().text = "Press E";

        }
        
    }
}

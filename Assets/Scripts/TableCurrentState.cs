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
            stateTable.SetActive(true);
            stateTable.GetComponent<TextMeshPro>().text = "Ready ";
            
        }
        else if (gameObject.GetComponent<CuttingTable>().IsTableCutting())
        {
            //Debug.Log("Ready");
            stateTable.GetComponent<TextMeshPro>().text = "Cutting: " + gameObject.GetComponent<CuttingTable>().getCuttingTime();
          
        }
        else if (gameObject.GetComponent<CuttingTable>().IsCatnipCut())
        {
            // Debug.Log("Ready");
            stateTable.GetComponent<TextMeshPro>().text = "Complete";

        }
        
    }
}

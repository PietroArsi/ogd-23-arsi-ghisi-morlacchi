using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FurnaceCurrentState : MonoBehaviour
{
    [SerializeField] private GameObject stateFurnace;
    // Start is called before the first frame update
    private void Update()
    {
        if (gameObject.GetComponent<FurnaceCook>().isFunraceEmpty())
        {
            Debug.Log("EMPTY");
            stateFurnace.GetComponent<TextMeshPro>().text = "Empty";
        }else if (gameObject.GetComponent<FurnaceCook>().IsFurnaceCooking())
        {
            
            stateFurnace.GetComponent<TextMeshPro>().text = "Cooking: "+ gameObject.GetComponent<FurnaceCook>().GetCookingTime();
        }else if (gameObject.GetComponent<FurnaceCook>().IsCookingOver())
        {
            Debug.Log("Ready");
            stateFurnace.GetComponent<TextMeshPro>().text = "Ready";
        }
    }
}

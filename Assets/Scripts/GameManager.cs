using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI catnipCounter;
    private int catnipAmount;

    void Start()
    {
        catnipAmount = 0;
        UpdateCatnipText();
    }

    void Update()
    {
        
    }

    public int GetCatnip() {
        return catnipAmount;
    }

    public void AddCatnip(int amount) {
        catnipAmount += amount;
        UpdateCatnipText();
    }

    private void UpdateCatnipText() {
        catnipCounter.text = $"{catnipAmount}";
    }
}

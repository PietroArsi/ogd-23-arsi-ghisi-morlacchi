using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI catnipCounter;
    public TMPro.TextMeshProUGUI evidenceCounter;
    private int catnipAmount;
    private int evidence;

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
    public int GetEvidence()
    {
        return evidence;
    }

    public void AddCatnip(int amount) {
        catnipAmount += amount;
        UpdateCatnipText();
    }
    public void AddEvidence()
    {
        evidence++;
        UpdateEvidence();
    }

    private void UpdateCatnipText() {
        catnipCounter.text = $"{catnipAmount}";
    }
    private void UpdateEvidence()
    {
        evidenceCounter.text = $"{evidence}/3";
    }
}

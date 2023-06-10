using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer catMeshRenderer;
    // Start is called before the first frame update

    private Material material;
    private void Awake()
    {
    }


    public void SetPlayerColor(Color color)
    {
        material = new Material(catMeshRenderer.material);
        catMeshRenderer.material = material;
        material.color = color;
    }
}

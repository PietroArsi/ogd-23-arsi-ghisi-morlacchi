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


    public void SetPlayerColor(Material mat)
    {
       // material = new Material(catMeshRenderer.material);
        catMeshRenderer.material = mat;
       // material.color = color;
    }
}

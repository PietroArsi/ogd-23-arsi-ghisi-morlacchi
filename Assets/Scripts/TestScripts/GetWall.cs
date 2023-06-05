using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWall : ResourceInteractable
{
    public GameObject block;

  

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void Collect()
    {
        if (!PlayerNetwork.LocalIstance.hasSpawnObject())
         pickableObject.spawnObj(block, PlayerNetwork.LocalIstance.gameObject);


    }
}

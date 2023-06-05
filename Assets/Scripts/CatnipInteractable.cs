using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatnipInteractable : ResourceInteractable
{
    public GameObject plantModel;
    public float respawnTime;
    bool readyToCollect;
    ActionCooldown resourceCooldown;
    public GameManager gm;

    void Start() {
        readyToCollect = true;
        resourceCooldown = new ActionCooldown();
    }

    void Update() {
        resourceCooldown.Advance(Time.deltaTime);

        if (!readyToCollect && resourceCooldown.Check()) {
            //plantModel.SetActive(true);
            readyToCollect = true;
            // luca addition
            CatnipPickUpNetwork.Instance.respawnCatnipNetwork(plantModel);
        }
    }

    public override void Collect() {
        //Debug.Log($"Catnip gathered!");
        visualDebugger.AddMessage("Catnip gathered");

        //gm.AddCatnip(1);

        //plantModel.SetActive(false);
        readyToCollect = false;
        resourceCooldown.Set(respawnTime);

        //luca possible additions
        CatnipPickUpNetwork.Instance.Adapt(plantModel);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class FixEnemyPosition : NetworkBehaviour
{
    public Vector3 currentTransform;
    public Quaternion currentRotation;
    // Start is called before the first frame update
    void Start()
    {
        currentTransform = transform.position;
        currentRotation = transform.rotation;
        FixClientPositionClientRpc(transform.position, transform.rotation);
        StartCoroutine(CheckEnemyPosition()); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator CheckEnemyPosition()
    {
        while (true)
        {
            FixClientPositionClientRpc(transform.position, transform.rotation);
            yield return new WaitForSeconds(0.5f);
        }
        
    }

    [ClientRpc]
    private void FixClientPositionClientRpc(Vector3 serverPosition, Quaternion serverRotation)
    {
        if ((currentTransform-serverPosition).magnitude != 0)
        {
            //visualDebugger.AddMessage("FIX CLIENT POSITION");
            //Debug.Log("HELLO THERE");
            currentTransform = serverPosition;
            transform.position = currentTransform;
           
        }
        if(currentRotation != serverRotation)
        {
           //visualDebugger.AddMessage("FIX CLIENT Rotation");
            currentRotation = serverRotation;
            transform.rotation = serverRotation;
        }
    }


}

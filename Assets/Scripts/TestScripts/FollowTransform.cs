using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
   // [SerializeField] private LayerMask whenPlaced;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    
    }
    public void SetTargetTransform(Transform targetTransform)
    {
        this.targetTransform = targetTransform;
        //if (targetTransform.GetComponent<pickableObject>() || targetTransform.gameObject.GetComponentInParent<pickableObject>())
        //{
        //    building();
        //}

    }
    private void LateUpdate()
    {
        if (targetTransform == null)
        {
            return;
        }
        //this is temporary is hardcoded for easy testing 
        //in this example, we need to check in wich block of the map we need to place the object so the parent can be placed
        //thi part of the code is not ment to remove the target transform, need to talk to pietro.
        //anotehr problem to solve is overallping bocse, and when place a signle box no collsion
        else if (targetTransform.name == "map")
        {
            targetTransform.gameObject.GetComponent<SpawnableObjParent>().setspawnObject(null);
            targetTransform = null;
            //this need to be modified
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
            transform.rotation = transform.rotation;
            gameObject.GetComponent<BoxCollider>().enabled = true;

            gameObject.layer = LayerMask.NameToLayer("Interactable");

        }
        //this need to be changed is going to be removed the first part of the transform
        //else if (targetTransform.GetComponent<pickableObject>()|| targetTransform.gameObject.GetComponentInParent<pickableObject>())
        //
        else
        {
            transform.position = targetTransform.position;
            transform.rotation = targetTransform.rotation;
        }
    }
    public Transform GetTargetTransform()
    {
        if (targetTransform != null) return targetTransform;
        return null;
    }
}

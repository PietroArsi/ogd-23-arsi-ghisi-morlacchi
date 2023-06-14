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
        else if (targetTransform.gameObject.layer == 7)
        {
            float height = 0f;
            targetTransform.gameObject.GetComponent<SpawnableObjParent>().SetspawnObject(null);
            if (gameObject.name == "Wall")
            {
                 height = targetTransform.position.y + transform.localScale.y / 2;
            }
            //Debug.Log(height);
            transform.position = new Vector3(transform.position.x,height, transform.position.z);
            transform.rotation = transform.rotation;
            gameObject.GetComponent<BoxCollider>().enabled = true;

            gameObject.layer = LayerMask.NameToLayer("Interactable");

        }
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

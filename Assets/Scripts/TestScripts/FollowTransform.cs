using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTransform : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    // [SerializeField] private LayerMask whenPlaced;
    // Start is called before the first frame update
    public LayerMask layerMaskPlaceDown;
    private bool positionSet;

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
        positionSet = false;
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
            //float height = targetTransform.position.y + transform.localScale.y/2;
            //targetTransform.gameObject.GetComponent<SpawnableObjParent>().SetspawnObject(null);
            //if (gameObject.name == "Wall")
            //{
            //     height = targetTransform.position.y + transform.localScale.y;
            //}
            ////Debug.Log(height);
            //transform.position = new Vector3(transform.position.x, height, transform.position.z);
            if (!positionSet)
            {
                RaycastHit hitDown;
                RaycastHit hitUp;
                bool didHitDown = Physics.Raycast(transform.position, Vector3.down, out hitDown, 5f, layerMaskPlaceDown);
                bool didHitUp = Physics.Raycast(transform.position, Vector3.up, out hitUp, 5f, layerMaskPlaceDown);

                if (didHitDown)
                {
                    if (GameObject.ReferenceEquals(hitDown.transform.gameObject, targetTransform.gameObject))
                    {
                        transform.position = new Vector3(transform.position.x, hitDown.point.y, transform.position.z);
                    }
                    else
                    {
                        Physics.Raycast(transform.position, Vector3.down, out hitDown, 10f, layerMaskPlaceDown);
                        transform.position = new Vector3(transform.position.x, hitDown.point.y, transform.position.z);
                    }
                }
                else if (didHitUp && GameObject.ReferenceEquals(hitUp.transform.gameObject, targetTransform.gameObject))
                {
                    transform.position = new Vector3(transform.position.x, hitUp.point.y, transform.position.z);
                }

                //transform.rotation = transform.rotation;
                gameObject.GetComponent<BoxCollider>().enabled = true;
                positionSet = true;
            }

            //gameObject.layer = LayerMask.NameToLayer("Interactable");
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    private bool canLook;

    void Start() {
        canLook = true;
    }

    void Update()
    {
        //Debug.Log($"{Input.mousePosition} - {transform.GetComponent<RectTransform>().TransformPoint(transform.GetComponent<RectTransform>().rect.center)}");
        if (canLook) {
            Vector3 spriteCenter = transform.GetComponent<RectTransform>().TransformPoint(transform.GetComponent<RectTransform>().rect.center);
            Vector3 mousePos = Input.mousePosition;

            Vector3 targetDir = mousePos - spriteCenter;
            float angle = (Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg) - 90f;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        } else {
            transform.rotation = Quaternion.AngleAxis(0f, Vector3.forward);
        }
    }

    public void SetLookAt(bool mode) {
        canLook = mode;
    }
}

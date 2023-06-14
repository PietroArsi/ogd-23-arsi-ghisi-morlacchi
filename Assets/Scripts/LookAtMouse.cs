using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMouse : MonoBehaviour
{
    void Update()
    {
        //Debug.Log($"{Input.mousePosition} - {transform.GetComponent<RectTransform>().TransformPoint(transform.GetComponent<RectTransform>().rect.center)}");

        Vector3 spriteCenter = transform.GetComponent<RectTransform>().TransformPoint(transform.GetComponent<RectTransform>().rect.center);
        Vector3 mousePos = Input.mousePosition;

        Vector3 targetDir = mousePos - spriteCenter;
        float angle = (Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg) - 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        //Vector3 mouseWorldCoord = Camera.main.ScreenPointToRay(Input.mousePosition).origin;
        //print(mouseWorldCoord);

        ///* Get a vector pointing from initialPosition to the target. Vector shouldn't be longer than maxDistance. */
        //var originToMouse = mouseWorldCoord - transform.position;
        //originToMouse = Vector3.ClampMagnitude(originToMouse, 1);

        ///* Linearly interpolate from current position to mouse's position. */
        //transform.position = Vector3.Lerp(transform.position, transform.position + originToMouse, 1f * Time.deltaTime);
    }
}

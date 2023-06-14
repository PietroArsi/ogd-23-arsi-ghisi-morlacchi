using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeepVertical : MonoBehaviour
{
    void Update()
    {
        Vector3 currentRotation = transform.eulerAngles;
        currentRotation = new Vector3(currentRotation.x, currentRotation.y, 0f);
        transform.eulerAngles = currentRotation;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimpleRotatingImage : MonoBehaviour
{
    public float timestep;
    public float anglePerStep;

    private float startTime;
    private RectTransform image;
    private bool canRotate;

    void Start() {
        canRotate = false;
        startTime = Time.time;
        if (image == null) {
            image = transform.GetComponent<RectTransform>();
        }
        gameObject.SetActive(false);
    }

    void Update() {
        if (canRotate && Time.time - startTime >= timestep) {
            Vector3 imageAngle = image.localEulerAngles;
            imageAngle.z += anglePerStep;

            image.localEulerAngles = imageAngle;

            startTime = Time.time;
        }
    }

    public void SetRotate(bool status) {
        canRotate = status;
        gameObject.SetActive(status);
    }
}

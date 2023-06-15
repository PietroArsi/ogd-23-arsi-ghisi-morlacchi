using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomCloseEyes : MonoBehaviour
{
    public Sprite eyeOpen;
    public Sprite eyeClosed;

    public GameObject eyeLeft;
    public GameObject eyeRight;
    public GameObject pupilLeft;
    public GameObject pupilRight;

    private ActionCooldown eyeTimer;
    private Image imageCompLeft;
    private Image imageCompRight;
    private LookAtMouse lookLeft;
    private LookAtMouse lookRight;

    private bool closed;

    void Start()
    {
        imageCompLeft = eyeLeft.GetComponent<Image>();
        imageCompRight = eyeRight.GetComponent<Image>();
        imageCompLeft.sprite = eyeOpen;
        imageCompRight.sprite = eyeOpen;
        lookLeft = eyeLeft.GetComponent<LookAtMouse>();
        lookRight = eyeRight.GetComponent<LookAtMouse>();
        lookLeft.SetLookAt(true);
        lookRight.SetLookAt(true);
        pupilLeft.SetActive(true);
        pupilRight.SetActive(true);

        closed = false;
        eyeTimer = new ActionCooldown();
        eyeTimer.Set(3f);
    }

    void Update()
    {
        if (closed) {
            return;
        }

        if (eyeTimer.Check()) {
            pupilLeft.SetActive(false);
            pupilRight.SetActive(false);
            imageCompLeft.sprite = eyeClosed;
            imageCompRight.sprite = eyeClosed;
            lookLeft.SetLookAt(false);
            lookRight.SetLookAt(false);
            closed = true;
            StartCoroutine(OpenEye());
        }

        eyeTimer.Advance(Time.deltaTime);
    }

    IEnumerator OpenEye() {
        yield return new WaitForSeconds(0.2f);
        pupilLeft.SetActive(true);
        pupilRight.SetActive(true);
        imageCompLeft.sprite = eyeOpen;
        imageCompRight.sprite = eyeOpen;
        lookLeft.SetLookAt(true);
        lookRight.SetLookAt(true);
        eyeTimer.Set(3f + Random.Range(-1f, 1f));
        closed = false;
    }
}

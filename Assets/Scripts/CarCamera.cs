using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CarCamera : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    //public Rigidbody rb;

    public float rotationSpeed = 6f;

    public Transform combatLookAt;

    public CameraStyle currentStyle;

    //added for testing network
    CinemachineFreeLook cinemachineFreeLook;

    public enum CameraStyle {
        Basic, Combat, Topdown
    }

    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;
        //player = PlayerNetwork.LocalIstance.gameObject.transform;
        //playerObj = PlayerNetwork.LocalIstance.gameObject.transform.Find("PlayerModel CC").transform;
        //orientation = PlayerNetwork.LocalIstance.gameObject.transform.Find("Orientation CC").transform;
        // orientation = playerObj.Find("Orientation CC");

        cinemachineFreeLook = this.gameObject.GetComponent<CinemachineFreeLook>();
        //cinemachineFreeLook.Follow = PlayerNetwork.LocalIstance.gameObject.transform;
        //cinemachineFreeLook.LookAt = PlayerNetwork.LocalIstance.gameObject.transform;


        this.enabled = true;
    }

    void Update() {
        //get forward direction player-camera
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        //if (currentStyle == CameraStyle.Basic || currentStyle == CameraStyle.Topdown) {
        //    //rotate player
        //    float horizontalInput = Input.GetAxis("Horizontal");
        //    float verticalInput = Input.GetAxis("Vertical");
        //    Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //    if (inputDir != Vector3.zero) {
        //        playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        //    }
        //}
        //else if (currentStyle == CameraStyle.Combat) {
        //    Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
        //    orientation.forward = dirToCombatLookAt.normalized;

        //    playerObj.forward = dirToCombatLookAt.normalized;
        //}

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDir != Vector3.zero) {
            playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
        }
    }
}

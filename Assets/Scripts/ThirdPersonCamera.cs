using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ThirdPersonCamera : MonoBehaviour
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
    public CinemachineFreeLook cinemachineFreeLook;

    public enum CameraStyle {
        Basic, Combat, Topdown
    }

    void Start() {
        //if (PlayerNetwork.LocalIstance != null)
        //{
            //Cursor.lockState = CursorLockMode.Locked;
            //Cursor.visible = false;
            //player = PlayerNetwork.LocalIstance.gameObject.transform;
            //playerObj = PlayerNetwork.LocalIstance.gameObject.transform.Find("PlayerObj").transform;
           // cinemachineFreeLook = this.gameObject.GetComponent<CinemachineFreeLook>();
           // cinemachineFreeLook.Follow = PlayerNetwork.LocalIstance.gameObject.transform;

            //orientation = PlayerNetwork.LocalIstance.gameObject.transform.Find("Orientation").transform;
        //}
        if(PlayerNetwork.LocalIstance!=null)
        {
            //Debug.Log("HELLO THERE");
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            player = PlayerNetwork.LocalIstance.gameObject.transform;
            playerObj = PlayerNetwork.LocalIstance.gameObject.transform.Find("PlayerModel CC").transform;
            orientation = PlayerNetwork.LocalIstance.gameObject.transform.Find("Orientation CC").transform;
            // orientation = playerObj.Find("Orientation CC");

            cinemachineFreeLook = this.gameObject.GetComponent<CinemachineFreeLook>();
            cinemachineFreeLook.Follow = PlayerNetwork.LocalIstance.gameObject.transform;
            cinemachineFreeLook.LookAt = PlayerNetwork.LocalIstance.gameObject.transform;


            this.enabled = true;
            //this.enabled = false;
            //PlayerNetwork.OnAnyPlayerSpawned += Player_OnAnySpawned;
        }
    }

    // Added when player event spawn
    private void Player_OnAnySpawned(object sender, EventArgs e)
    {
        if (PlayerNetwork.LocalIstance != null)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            player = PlayerNetwork.LocalIstance.gameObject.transform;
            playerObj = PlayerNetwork.LocalIstance.gameObject.transform.Find("PlayerModel CC").transform;
            orientation = PlayerNetwork.LocalIstance.gameObject.transform.Find("Orientation CC").transform;
           // orientation = playerObj.Find("Orientation CC");

            //cinemachineFreeLook = this.gameObject.GetComponent<CinemachineFreeLook>();
            cinemachineFreeLook.Follow = PlayerNetwork.LocalIstance.gameObject.transform;
            cinemachineFreeLook.LookAt = PlayerNetwork.LocalIstance.gameObject.transform;


            //this.enabled = true;
        }
    }

    void Update()
    {
        //LUCA ADDITION WHEN THE GAME IS OVER
        if (!GameManagerStates.Instance.IsGameOver() && !GameManagerStates.Instance.IsPlayerDisconnected() && GameManagerStates.Instance.HostDisconnected==false)
        {
           // Debug.Log("IS PLAYER THERE? " + PlayerNetwork.LocalIstance != null);
            if (PlayerNetwork.LocalIstance != null)
            {
                player = PlayerNetwork.LocalIstance.gameObject.transform;
                playerObj = PlayerNetwork.LocalIstance.gameObject.transform.Find("PlayerModel CC").transform;
                orientation = PlayerNetwork.LocalIstance.gameObject.transform.Find("Orientation CC").transform;
                //get forward direction player-camera
                Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
                orientation.forward = viewDir.normalized;

                if (currentStyle == CameraStyle.Basic || currentStyle == CameraStyle.Topdown)
                {
                    float horizontalInput = 0f;
                    float verticalInput = 0f;
                    if (GameManagerStates.Instance.CanMoveCamera())
                    {
                        //rotate player
                        horizontalInput = Input.GetAxis("Horizontal");
                        verticalInput = Input.GetAxis("Vertical");
                    }
                    Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

                    if (inputDir != Vector3.zero)
                    {
                        playerObj.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);
                    }
                }
                else if (currentStyle == CameraStyle.Combat)
                {
                    Vector3 dirToCombatLookAt = combatLookAt.position - new Vector3(transform.position.x, combatLookAt.position.y, transform.position.z);
                    orientation.forward = dirToCombatLookAt.normalized;

                    playerObj.forward = dirToCombatLookAt.normalized;
                }

                // Luca Addition when the Construction Menu is Open
                if (GameManagerStates.Instance.GetConstructionWindowActive())
                {
                    //Debug.Log("OPEN CONSTRUCTION");
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                }
                else
                {
                    //Debug.Log("CLOSE CONSTRUCTION");
                    Cursor.lockState = CursorLockMode.Locked;
                    Cursor.visible = false;
                }
            }
            else
            {
                //this.enabled = false;
                PlayerNetwork.OnAnyPlayerSpawned += Player_OnAnySpawned;
            }
        }
        else
        {
            Debug.Log("UNLOCK PLEASE");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}

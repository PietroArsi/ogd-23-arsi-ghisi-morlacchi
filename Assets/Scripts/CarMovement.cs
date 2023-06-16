using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class CarMovement : NetworkBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;

    public float groundDrag = 5f;

    public float jumpForce = 12f;
    public float jumpCooldown = 0.25f;
    public float airMultiplier = 0.4f;
    public bool canJump = false;
    //bool readyToJump;

    public Animator playerAnimator;

    public GenericSceneManager genericSceneManager;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask whatIsGround;
    bool grounded;
    public float gravityMultiplier = 1f;

    [Header("Slope Handling")]
    public float maxSlopeAngle = 40f;
    private RaycastHit slopeHit;

    public Transform orientation;

    [Header("Interaction")]
    ActionCooldown attackCooldown;

    //inputs
    float horizontalInput;
    float verticalInput;
    bool attack;
    public LayerMask interactionLayer;
    public Transform interactionCollider;

    Vector3 moveDirection;
    CharacterController cc;

    private void Start() {
        cc = GetComponent<CharacterController>();
        //readyToJump = true;

        attackCooldown = new ActionCooldown();
    }

    private void Update() {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        if (IsHost)
        {
            GetInput();
        }
        attackCooldown.Advance(Time.deltaTime);
    }

    private void FixedUpdate() {
        if (IsHost)
        {
            MovePlayer();
        }
    }

    private void GetInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        attack = Input.GetButtonDown("Fire1");

        if (attack && attackCooldown.Check()) {
            attackCooldown.Set(0.5f);
            Attack();
        }
    }

    private void MovePlayer() {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        //vSpeed = 0;

        if (moveDirection.magnitude != 0) {
            cc.Move(moveDirection.normalized * moveSpeed * 0.02f);
            //playerAnimator.SetBool("walk", true);
        }
        else {
            //playerAnimator.SetBool("walk", false);
        }

    }

    private void SpeedControl() {
        //if (OnSlope()) {
        //    if (rb.velocity.magnitude > moveSpeed) {
        //        rb.velocity = rb.velocity.normalized * moveSpeed;
        //    }
        //} else {
        //    Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //    if (flatVelocity.magnitude > moveSpeed) {
        //        Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
        //        rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        //    }
        //}
        //Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //if (flatVelocity.magnitude > moveSpeed) {
        //    Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
        //    rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
        //}
    }

    private void Attack() {
        string level = transform.GetComponent<LevelSelector>().GetSelectedLevel();

       //LUCA ADDITION LOAD THE GAME SCENE
        if (level != "") {
            Debug.Log($"Selected level: {level}");
            if (genericSceneManager != null)
            {
                genericSceneManager.LoadNetwork("test level");
            }//SceneLoader.LoadNetwork(SceneLoader.Scene.NetworkTestLevel);
            }
    }
}

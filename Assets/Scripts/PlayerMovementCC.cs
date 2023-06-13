using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;
public class PlayerMovementCC : NetworkBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 7f;

    public float groundDrag = 5f;

    public float jumpForce = 12f;
    public float jumpCooldown = 0.25f;
    public float airMultiplier = 0.4f;
    public bool canJump = false;
    bool readyToJump;

    public Animator playerAnimator;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float playerHeight = 2f;
    public LayerMask whatIsGround;
    bool grounded;
    public float gravityMultiplier = 1f;
    private float vSpeed;

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

    private ConstructionMenu constructionMenu;

   
    private void Start() {
        cc = GetComponent<CharacterController>();
        readyToJump = true;

        attackCooldown = new ActionCooldown();

        constructionMenu = GameObject.Find("Canvas").GetComponent<ConstructionMenu>();
    }

    private void Update() {
        if (!IsOwner) return;

        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        GetInput();
        //SpeedControl();

        attackCooldown.Advance(Time.deltaTime);
    }

    private void FixedUpdate() {
        if (!IsOwner) return;
        MovePlayer();
    }

    private void GetInput() {
        //if (canJump && Input.GetKey(jumpKey) && readyToJump && grounded) {
        //    readyToJump = false;
        //    Jump();
        //    Invoke(nameof(ResetJump), jumpCooldown);
        //}

        if (!GameManagerStates.Instance.CanMoveCamera())
        {
            horizontalInput = 0;
            verticalInput = 0;
            attack = false;
        }
        else
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            verticalInput = Input.GetAxisRaw("Vertical");

            attack = Input.GetButtonDown("Fire1");
        }

        if (attack && attackCooldown.Check()) {
            attackCooldown.Set(0.5f);
            Attack();
        }

        if (Input.GetKeyDown(KeyCode.Q) && !constructionMenu.GetConstructionScreen().activeSelf && !PlayerNetwork.LocalIstance.HasSpawnObject())
        {
            constructionMenu.Show();
        }
        else if (Input.GetKeyDown(KeyCode.Q) && constructionMenu.GetConstructionScreen().activeSelf)
        {
            constructionMenu.Hide();
        }
    }

    private void MovePlayer() {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        bool isOnSlope = OnSlope();

        if (!grounded) {
            vSpeed += (-9.81f) * gravityMultiplier * Time.deltaTime;
        }
        else {
            vSpeed = 0;
        }

        //if (moveDirection.magnitude != 0) {
        //    if (grounded) {
        //        //cc.Move(moveDirection.normalized * moveSpeed * 0.02f);
        //        playerAnimator.SetBool("walk", true);
        //    } else {
        //        moveDirection.y = vSpeed;
        //        //cc.Move(moveDirection.normalized * moveSpeed * 0.02f);
        //        playerAnimator.SetBool("walk", false);
        //    }
        //    cc.Move(moveDirection.normalized * moveSpeed * 0.02f);
        //} else {
        //    moveDirection.y = vSpeed;
        //    cc.Move(moveDirection.normalized * moveSpeed * 0.02f);
        //    playerAnimator.SetBool("walk", false);
        //}
        if (moveDirection.magnitude != 0) {
            if (isOnSlope) {
                cc.Move(GetSlopeMoveDirection() * moveSpeed * 0.02f);

                playerAnimator.SetBool("walk", true);
            }
            else if (grounded) { //grounded
                cc.Move(moveDirection.normalized * moveSpeed * 0.02f);
                playerAnimator.SetBool("walk", true);
            }
            else { //not grounded
                moveDirection.y = vSpeed;
                cc.Move(moveDirection.normalized * moveSpeed * 0.02f);
                playerAnimator.SetBool("walk", false);
            }
        }
        else {
            if (!grounded) {
                moveDirection.y = vSpeed;
                cc.Move(moveDirection.normalized * moveSpeed * 0.02f);
            }
            playerAnimator.SetBool("walk", false);
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

    private void Jump() {
        //rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        //rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump() {
        readyToJump = true;
    }

    private bool OnSlope() {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f)) {
            float angle = Mathf.Abs(Vector3.Angle(Vector3.up, slopeHit.normal));

            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection() {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    private void Attack() {
        Collider[] hitColliders = Physics.OverlapBox(interactionCollider.transform.position, interactionCollider.localScale / 2, Quaternion.identity, interactionLayer);
        foreach (Collider c in hitColliders) {
            if (c.GetComponent<ResourceInteractable>()) {
                c.GetComponent<ResourceInteractable>().Collect();
            }
        }
    }

    // when activated causes borders to flicker in game view
    //void OnDrawGizmos() {
    //    Gizmos.color = Color.red;
    //    Vector3 direction = GetSlopeMoveDirection() * 5;
    //    Gizmos.DrawRay(transform.position, direction);
    //}
}

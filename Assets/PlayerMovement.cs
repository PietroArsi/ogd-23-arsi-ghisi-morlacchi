using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
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
    Rigidbody rb;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        readyToJump = true;

        attackCooldown = new ActionCooldown();
    }

    private void Update() {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        GetInput();
        SpeedControl();

        if (grounded) {
            rb.drag = groundDrag;
        } else {
            rb.drag = 0;
        }

        attackCooldown.Advance(Time.deltaTime);
    }

    private void FixedUpdate() {
        MovePlayer();
    }

    private void GetInput() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        attack = Input.GetButtonDown("Fire1");

        if (canJump && Input.GetKey(jumpKey) && readyToJump && grounded) {
            readyToJump = false;
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }

        if (attack && attackCooldown.Check()) {
            attackCooldown.Set(0.5f);
            Attack();
        }
    }

    private void MovePlayer() {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        bool isOnSlope = OnSlope();

        if (moveDirection.magnitude != 0) {
            if (isOnSlope) {
                rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 10f, ForceMode.Force);

                if (rb.velocity.y > 0) {
                    rb.AddForce(Vector3.down * 20f, ForceMode.Force);
                }
                playerAnimator.SetBool("walk", true);
            }
            else if (grounded) { //grounded
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
                playerAnimator.SetBool("walk", true);
            }
            else { //not grounded
                rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
                playerAnimator.SetBool("walk", false);
            }
        } else {
            playerAnimator.SetBool("walk", false);
        }

        //if (rb.velocity.magnitude != 0) {
        //    playerAnimator.SetBool("walk", true);
        //} else {
        //    playerAnimator.SetBool("walk", false);
        //}

        rb.useGravity = !isOnSlope;
    }

    private void SpeedControl() {
        if (OnSlope()) {
            if (rb.velocity.magnitude > moveSpeed) {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        } else {
            Vector3 flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVelocity.magnitude > moveSpeed) {
                Vector3 limitedVelocity = flatVelocity.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVelocity.x, rb.velocity.y, limitedVelocity.z);
            }
        }
    }

    private void Jump() {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
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

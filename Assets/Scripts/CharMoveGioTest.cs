using UnityEngine;

public class CharMoveGioTest : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 3f;
    public float gravity = 9.8f;

    private CharacterController characterController;
    private Vector3 moveDirection;
    private float mouseX;
    private float verticalVelocity;

    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        moveDirection = new Vector3(moveHorizontal, 0f, moveVertical).normalized;

        mouseX += Input.GetAxis("Mouse X") * rotationSpeed;
        transform.rotation = Quaternion.Euler(0f, mouseX, 0f);

        if (!characterController.isGrounded)
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        MoveCharacter(moveDirection);
    }

    private void MoveCharacter(Vector3 direction)
    {
        Vector3 velocity = direction * moveSpeed;
        velocity.y = verticalVelocity;

        characterController.Move(transform.TransformDirection(velocity * Time.fixedDeltaTime));
    }
}

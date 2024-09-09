using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour {

    public Transform orientation;
    public Transform groundCheck;

    float movementSpeedMultiplier = 1f;
    [Header("Ground Movement")]
    public float movementSpeed;
    [SerializeField] float maxVelocityHorizontal;
    [SerializeField] float jumpStrength;
    [SerializeField] float rbDragGrounded;
    float gravity = 9.81f;

    [Header("Air Movement")]
    [SerializeField, Range(0.1f, 1f)] float airMovementSpeedMultiplier;
    [SerializeField, Range(1f, 2f)] float rbDragAirborne;
    [SerializeField] float verticalMovementSpeed;
    [SerializeField] float maxVelocityVertical;




    float horizontalInput;
    float verticalInput;
    bool jumpInput;
    bool descendInput;
    Rigidbody rb;
    Vector3 moveDirection;

    private bool isGravitated;
    private bool isGrounded;

    public enum MovementType {
        Gravitated,
        ZeroGravity
    }

    private MovementType movementType;

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update() {
        InputSystem();

        if (jumpInput) {
            verticalMovementSpeed = Mathf.Abs(verticalMovementSpeed);
        }
        if (descendInput) {
            verticalMovementSpeed = -Mathf.Abs(verticalMovementSpeed);
        }
    }

    private void FixedUpdate() {
        if (Physics.Raycast(groundCheck.position, Vector3.down, 5f)) {
            movementType = MovementType.Gravitated;
        } else {
            movementType = MovementType.ZeroGravity;
        }

        isGrounded = IsGrounded();

        switch (movementType) {
            case MovementType.Gravitated:
                movementSpeedMultiplier = 1f;
                rb.drag = rbDragGrounded;
                rb.useGravity = true;
                if (isGrounded) HandleJump();

                if (rb.velocity.y < 0f) {
                    AddJumpingGravity();
                }

                break;
            case MovementType.ZeroGravity:
                movementSpeedMultiplier = airMovementSpeedMultiplier;
                rb.drag = rbDragAirborne;
                rb.useGravity = false;

                break;
            default:
                break;
        }

        Move();   
    }

    private void InputSystem() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        jumpInput = Input.GetKey(KeyCode.Space);
        descendInput = Input.GetKey(KeyCode.LeftShift);
    }

    private void Move() {
        float regulationFactor = 1f;    // regulates amount of force that's applied. If maxVelocityHorizontal is exceeded, only a minimal force (5%) will be applied.
        float regulationFactorVert = 1f;

        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (Mathf.Abs(rb.velocity.x) >= maxVelocityHorizontal || Mathf.Abs(rb.velocity.z) >= maxVelocityHorizontal) {
            regulationFactor = 0.05f;
        }



        if (movementType != MovementType.ZeroGravity) {
            regulationFactorVert = 0f;
        } else {
            if (Mathf.Abs(rb.velocity.y) >= maxVelocityVertical) {
                regulationFactorVert = 0.05f;
            }
            if (!jumpInput && !descendInput) {
                regulationFactorVert = 0f;
            }
        }

        Vector3 movementForce = moveDirection.normalized * movementSpeed * movementSpeedMultiplier * regulationFactor;
        movementForce += Vector3.up * verticalMovementSpeed * regulationFactorVert;

        rb.AddForce(movementForce, ForceMode.Force);
    }

    private bool IsGrounded() {
        return Physics.Raycast(groundCheck.position, Vector3.down, 0.2f);
    }

    private void HandleJump() { 
        if (jumpInput) {
            rb.AddForce(Vector3.up * jumpStrength, ForceMode.Impulse);
        } 
    }

    private void AddJumpingGravity() {
        rb.AddForce(Vector3.down * gravity, ForceMode.Force);
    }

}

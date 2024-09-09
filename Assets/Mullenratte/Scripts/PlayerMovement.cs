using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour {

    public Transform orientation;
    public Transform groundCheck;

    [Header("Ground Movement")]
    public float movementSpeed;
    [SerializeField] float maxVelocityHorizontal;
    [SerializeField] float jumpStrength;
    float gravity = 9.81f;

    [Header("Air Movement")]
    [SerializeField, Range(0.01f, 1f)] float airMovementSpeedMultiplier;



    float horizontalInput;
    float verticalInput;
    bool jumpInput;
    Rigidbody rb;
    Vector3 moveDirection;

    private bool isGravitated;

    public enum MovementType {
        Gravitated,
        Airborne
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
    }

    private void FixedUpdate() {
        if (!isGravitated) {
            rb.useGravity = false;
        } else {
            rb.useGravity = true;
        }
        if (movementType == MovementType.Gravitated) {
            HandleJump();
        } 
        if (rb.velocity.y < 0f) {
            AddJumpingGravity();
            Debug.Log("gravity affecting rb!");
        }


        Move();


        if (Physics.Raycast(groundCheck.position, Vector3.down, 5f)) {
            isGravitated = true;
        } else {
            isGravitated = false;
        }
        if (Physics.Raycast(groundCheck.position, Vector3.down, 0.1f)) {
            movementType = MovementType.Gravitated;
        } else {
            movementType = MovementType.Airborne;
        }
        //Debug.DrawRay(groundCheck.position, Vector3.down, Color.red);     
    }

    private void InputSystem() {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        jumpInput = Input.GetKey(KeyCode.Space);
    }

    private void Move() {
        float regulationFactor = 1f;    // regulates amount of force that's applied. If maxVelocityHorizontal is exceeded, only a minimal force (5%) will be applied.
       
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (Mathf.Abs(rb.velocity.x) >= maxVelocityHorizontal || Mathf.Abs(rb.velocity.z) >= maxVelocityHorizontal) {
            regulationFactor = 0.05f;
        }

        rb.AddForce(moveDirection.normalized * movementSpeed * regulationFactor, ForceMode.Force);
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

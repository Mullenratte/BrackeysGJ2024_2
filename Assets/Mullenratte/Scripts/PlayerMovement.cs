using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour {

    public static PlayerMovement instance;

    public Transform orientation;
    public Transform groundCheck;

    float movementSpeedMultiplier = 1f;

    [SerializeField] private LayerMask platformLayer;

    [Header("Ground Movement")]
    public float movementSpeed;
    [SerializeField] float jumpStrength;
    [SerializeField] float rbDragGrounded;
    float gravity = 9.81f;
    float onPlatformMaxVelocityX, onPlatformMaxVelocityZ;

    [Header("Air Movement")]
    [SerializeField, Range(0.1f, 1f)] float airMovementSpeedMultiplier;
    [SerializeField, Range(1f, 2f)] float rbDragAirborne;
    [SerializeField] float verticalMovementSpeed;
    [SerializeField] float maxVelocityHorizontal;
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

    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        onPlatformMaxVelocityZ = maxVelocityHorizontal;
        onPlatformMaxVelocityX = maxVelocityHorizontal + Platform.instance.Velocity.x;
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
        if (Physics.Raycast(groundCheck.position, Vector3.down, 5f, platformLayer)) {  
            onPlatformMaxVelocityX = maxVelocityHorizontal + Platform.instance.Velocity.x;                // increase max velocity to account for platform velocity
            movementType = MovementType.Gravitated;
            rb.AddForce(Platform.instance.Velocity);

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



        switch (movementType) {
            case MovementType.Gravitated:
                regulationFactorVert = 0f;

                /* player won't speed up if he's...
                 * ... moving faster towards x than the onPlatformMaxVelocityX (which is the general max horizontal velocity + platform x-velocity)
                 * ... moving faster towards -x than the max on platform velocity in -X-direction (which is Platform.instance.Velocity.x - maxVelocityHorizontal)
                 * ... moving faster towards z or -z than the onPlatformMaxVelocityZ 
                */
                if (rb.velocity.x >= onPlatformMaxVelocityX || rb.velocity.x <= Platform.instance.Velocity.x - maxVelocityHorizontal || Mathf.Abs(rb.velocity.z) >= onPlatformMaxVelocityZ) {
                    regulationFactor = 0.05f;
                }
                break;
            case MovementType.ZeroGravity:
                if (Mathf.Abs(rb.velocity.x) >= maxVelocityHorizontal || Mathf.Abs(rb.velocity.z) >= maxVelocityHorizontal) {
                    regulationFactor = 0.05f;
                }

                if (Mathf.Abs(rb.velocity.y) >= maxVelocityVertical) {
                    regulationFactorVert = 0.05f;
                }

                if (!jumpInput && !descendInput) {
                    regulationFactorVert = 0f;
                }
                break;
            default:
                break;
        }        

        Vector3 movementForce = moveDirection.normalized * movementSpeed * movementSpeedMultiplier * regulationFactor;
        movementForce += Vector3.up * verticalMovementSpeed * regulationFactorVert;

        //Debug.Log(Vector3.Distance(transform.position, TetherSystem.instance.tetherPoint.position));

        if (Vector3.Distance(transform.position, Platform.instance.tetherPoint.position) < Platform.instance.maxRange) {
            rb.AddForce(movementForce, ForceMode.Force);
        } else {
            movementForce = (Platform.instance.tetherPoint.position - transform.position).normalized * Platform.instance.Velocity.x;
            rb.AddForce(movementForce, ForceMode.Force);
        }

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

    public void ChangeMaxVelocity(float amount) {
        maxVelocityHorizontal += amount;
        maxVelocityVertical += amount;
        onPlatformMaxVelocityX += amount;
        onPlatformMaxVelocityZ += amount;
    }

}

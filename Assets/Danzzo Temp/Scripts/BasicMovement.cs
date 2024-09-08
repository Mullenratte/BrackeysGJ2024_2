using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class BasicMovement : MonoBehaviour
{
    public float movementSpeed;

    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Rigidbody rb;

    Vector3 moveDirection;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;   
    }

    // Update is called once per frame
    void Update()
    {
        InputSystem();
        MovePlayer();

    }

    private void InputSystem()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right*horizontalInput;
        
        rb.AddForce(moveDirection.normalized*movementSpeed*10f, ForceMode.Force);
    }
}

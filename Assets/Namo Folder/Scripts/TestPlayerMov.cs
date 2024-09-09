using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayerMov : MonoBehaviour
{
    public float moveSpeed = 10f; // Speed of movement

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // Calculate velocity
        Vector3 movement = new Vector3(x, 0, z) * moveSpeed;

        // Apply velocity
        //rb.velocity = movement;
    }
}

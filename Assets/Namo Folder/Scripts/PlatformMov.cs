using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting;
using UnityEngine;

public class PlatformMov : MonoBehaviour
{
    /* 
      Take a look at the cube named "Cube0" on the Hierarchy, its sliding on the platform and that's obvious, then take a look at it when we change 
    the current script with the script that's located below.
    */

    public float speed = 5f; // Speed of the platform along the x-axis
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    void FixedUpdate()
    {
        // Move the platform along the x-axis
        Vector3 movement = new Vector3(speed * Time.fixedDeltaTime, 0, 0);
        rb.MovePosition(rb.position + movement);

    }


    // !! NOTE : make sure the player AND Items does not have "Rigidbody" component, In case "Rigidbody" is required there is another script below


    // Platform is a PARENT for : player, item
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Item"))
        {

            collision.gameObject.transform.parent = transform;
            print("Enter - Player tagged object is now a child of: " + transform.name);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Item"))
        {

            collision.gameObject.transform.parent = null;
            print("Enter - Player tagged object is now a child of: " + transform.name);
        }
    }



}


/*
 * 
 * public float speed = 5f; // Speed of the platform along the x-axis


    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Move the platform along the x-axis
        Vector3 movement = new Vector3(speed * Time.fixedDeltaTime, 0, 0);
        rb.MovePosition(rb.position + movement);

    }


    // !! NOTE : make sure the player AND Items does not have "Rigidbody" component, In case "Rigidbody" is required there is another script below


    // Platform is a PARENT for : player, item
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Item"))
        {

            other.gameObject.transform.parent = transform;
            print("Enter - Player tagged object is now a child of: " + transform.name);
        }
    }

    // Unparenting the player, item from the platform.
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Item"))
        {
            other.gameObject.transform.parent = null;
            print("Exit");
        }
    }
}
 * 
 * 
 * 
 * 
 * 
 * 
public float speed = 5f; // Speed of the platform along the x-axis
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Make sure the platform is kinematic
    }

    void FixedUpdate()
    {
        // Move the platform along the x-axis
        Vector3 movement = new Vector3(speed * Time.fixedDeltaTime, 0, 0);
        rb.MovePosition(rb.position + movement);

        

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Item"))
        {
            // Ensure the player is parented to the platform
            other.gameObject.transform.parent = transform;
            print("Enter - Player tagged object is now a child of: " + transform.name);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Item"))
        {
            // Unparent the player when they exit the platform
            other.gameObject.transform.parent = null;
            print("Exit - Player tagged object has been unparented.");
        }
    }

 
 
 --------------- Collisions
 
 public float speed = 5f; // Speed of the platform along the x-axis
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = true; // Make sure the platform is kinematic
    }

    void FixedUpdate()
    {
        // Move the platform along the x-axis
        Vector3 movement = new Vector3(speed * Time.fixedDeltaTime, 0, 0);
        rb.MovePosition(rb.position + movement);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Item"))
        {
            // Ensure the player is parented to the platform
            collision.transform.parent = transform;
            print("HI" + transform.name);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Item"))
        {
            // Unparent the player when they exit the platform
            collision.transform.parent = null;
            print("Exit - Player tagged object has been unparented.");
        }
    }
 
 
 
 
 
 
 
 */


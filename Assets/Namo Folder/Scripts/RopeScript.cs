using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class DynamicRope : MonoBehaviour
{
    public Transform player; // Reference to the player
    public Transform platform; // Reference to the moving platform
    public float ropeLength = 10f; // Maximum length of the rope

    private LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = 2; // The rope has two ends
    }

    void Update()
    {
        // Get positions of the player and platform
        Vector3 playerPosition = player.position;
        Vector3 platformPosition = platform.position;

        // Calculate the direction and length between player and platform
        Vector3 direction = playerPosition - platformPosition;
        float distance = direction.magnitude;

        // Adjust the length of the rope
        if (distance > ropeLength)
        {
            direction.Normalize();
            playerPosition = platformPosition + direction * ropeLength;
        }

        // Update the Line Renderer positions
        lineRenderer.SetPosition(0, platformPosition); // Start point of the rope
        lineRenderer.SetPosition(1, playerPosition);   // End point of the rope
    }
}

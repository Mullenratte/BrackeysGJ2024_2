using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float sensX;
    public float sensY;

    public Transform orientation;
    
    float yRotation;
    float xRotation;

    private bool canMoveCamera = true;
    
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMoveCamera)
        {
       float mouseX = Input.GetAxisRaw("Mouse X")*Time.deltaTime*sensX;
       float mouseY = Input.GetAxisRaw("Mouse Y")*Time.deltaTime*sensY;
    
        yRotation += mouseX;
        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        orientation.rotation = Quaternion.Euler(xRotation, yRotation, 0);   // Mullenratte: add xRotation to orientation 
        }
    }

        // Method to enable/disable camera movement
    public void EnableCamera(bool enable)
    {
        canMoveCamera = enable;
    }
}

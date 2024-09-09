using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    private Vector3 lastPosition;

    [SerializeField]
    private LayerMask placementLayerMask;

    public event Action onClicked, onExit;

   
      private void Update()
    {
        if(Input.GetMouseButtonUp(0))
        {
            onClicked?.Invoke();
        }
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            onExit?.Invoke();
        }
    }

    public bool IsPointerOverUi()
    {
       return EventSystem.current.IsPointerOverGameObject(); 
    }
   
    public Vector3 GetSelectedMapPoint()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayerMask))
        {
            lastPosition = hit.point;;
        }
        
        return lastPosition;
    }

 
}

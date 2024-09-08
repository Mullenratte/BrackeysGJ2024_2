using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    GameObject mouseIndicator,cellIndicator;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;
   

    // Update is called once per frame
    void Update()
    {
        UnityEngine.Vector3 mousePosition = inputManager.GetSelectedMapPoint();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
}

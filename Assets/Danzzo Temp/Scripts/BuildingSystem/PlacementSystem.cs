using System;
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
    [SerializeField]
    private ObjectDatabseSO database;
    private int selectedObjectIndex = -1;

    [SerializeField]
    private GameObject gridVisualization;
   
    private void Start()
    {
        StopPlacement();
    }

    public void startPlacement(int ID)
    {
        StopPlacement();
        selectedObjectIndex = database.objectsData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex <0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }
        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        inputManager.onClicked += PlaceStrutre;
        inputManager.onExit += StopPlacement;
    }

    private void PlaceStrutre()
    {
        if(inputManager.IsPointerOverUi())
        {
            return;
        }
        UnityEngine.Vector3 mousePosition = inputManager.GetSelectedMapPoint();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        GameObject newObject= Instantiate(database.objectsData[selectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);
    }

    private void StopPlacement()
    {
        selectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        inputManager.onClicked -= PlaceStrutre;
        inputManager.onExit -= StopPlacement;
    }


    // Update is called once per frame
    void Update()
    {
        if (selectedObjectIndex < 0)
        {
            return;
        }
        
        UnityEngine.Vector3 mousePosition = inputManager.GetSelectedMapPoint();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenu : MonoBehaviour
{
    private bool buildPanelisActvie = false;
    [SerializeField] GameObject BuildPannel;
    
    [SerializeField] PlayerCam playerCam; 


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (!buildPanelisActvie)
            {
                OpenBuildingPannel();
            }
            else
            {
                CloseBuildingPannel();
            }
        }
    
    }

    private void OpenBuildingPannel()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        BuildPannel.SetActive(true);
        buildPanelisActvie = true;
        playerCam.EnableCamera(false);  
    }

    private void CloseBuildingPannel()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        BuildPannel.SetActive(false);
        buildPanelisActvie = false;
        playerCam.EnableCamera(true);  
    }

}

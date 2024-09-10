using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class uiScript : MonoBehaviour
{

    // ------------- Video Resolution ------------------------------

    Resolution[] resolutions;

    public TMP_Dropdown resolutionDropdown;
    
    private void Start()
    {
        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width  + " x "  + resolutions[i].height;
            options.Add(option);

        }

        resolutionDropdown.AddOptions(options);
    }

    // ------------- Loading Scenes ------------------------------
    public void LoadScene(int SceneNumber)
    {
        FindObjectOfType<SoundManager>().PlaySound("Mouse_Click");
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneNumber);
    }

    // ------------- EXIT Game ------------------------------
    public void Exit()
    {
        Application.Quit();
    }

    // ------------- Pause & Resume ------------------------------
    public void PauseAndResume(int timeScale)
    {
        Time.timeScale = timeScale;
        FindObjectOfType<SoundManager>().PlaySound("Mouse_Click");
    }


    // ------------- GAME OVER ------------------------------
    public void GameOver()
    {
        FindObjectOfType<SoundManager>().PlaySound("You_Lost");
    }


    // ------------- Video Quality (LOW,..., HIGH) ------------------------------
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }
    
   
}

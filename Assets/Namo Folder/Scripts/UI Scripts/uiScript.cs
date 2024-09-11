using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
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

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width  + " x "  + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }

        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        
    }

    // ------------- Setting Resolution ------------------------------


    public void SetResolution (int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height,Screen.fullScreen);
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


    // ------------- Volume Slider ------------------------------
    
    public AudioMixer audioMixer;

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume",volume);
    }
    
    // ------------- SFX Slider ------------------------------
    public void SetSfx(float sfxVolume)
    {
        audioMixer.SetFloat("SfxVolume", sfxVolume);
    }

    // ------------- Test (can be deleted) ------------------------------

    public void PlaySound(string soundName)
    {
        FindObjectOfType<SoundManager>().PlaySound(soundName);
    }

}

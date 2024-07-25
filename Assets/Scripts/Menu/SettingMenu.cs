using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingMenu : MonoBehaviour
{
    private Resolution[] resolutions;
    public Dropdown resolutionDropdown;
    public Dropdown graphicDropdown;
    private bool crosshairOn = false;
    [SerializeField]
    protected GameObject crosshair;

    public static SettingMenu Instance { get; private set; }

    private void Awake()
    { 
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        if(resolutionDropdown != null)
        {
            resolutions = Screen.resolutions;

            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();

            int idx = 0;
            int currIdx = 0;

            foreach (Resolution res in resolutions)
            {
                options.Add(res.width + " x " + res.height);

                if (res.width == Screen.currentResolution.width &&
                    res.height == Screen.currentResolution.height)
                {
                    currIdx = idx;
                }
                idx++;
            }
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currIdx;
            resolutionDropdown.RefreshShownValue();
        }

        if(graphicDropdown != null)
        {
            graphicDropdown.value = QualitySettings.GetQualityLevel();
            graphicDropdown.RefreshShownValue();
        }

    }

    public void SetResolution(int resIdx)
    {
        Resolution res = resolutions[resIdx];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetFullscreen(bool x)
    {
        Screen.fullScreen = x;
    }

    public void SetVolume(float volume)
    {
        AudioManager.Instance.SetVolume(volume);
    }

    public void SetGraphics(int level)
    {
        QualitySettings.SetQualityLevel(level);
    }

    public void SetCrosshair(bool x)
    {
        crosshairOn = x;
        if (SceneManager.GetActiveScene().name == "QuestScene")
        {
            crosshair.SetActive(crosshairOn);
        }
        else
        {
            crosshair.SetActive(false);
        }
    }

    public void ShowCrosshair()
    {
        if (SceneManager.GetActiveScene().name == "QuestScene")
        {
            crosshair.SetActive(crosshairOn);
        }
        else
        {
            crosshair.SetActive(false);
        }
    }
}

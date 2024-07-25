using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Rendering.PostProcessing;

public class MenuConfig : MonoBehaviour
{
    public GameObject menu;
    public CinemachineFreeLook menuCam;
    public PostProcessVolume postProcessing;
    public CanvasGroup canvas;
    public CinemachineBrain camBrain;

    public bool IsShown { get; private set; }
    public static MenuConfig Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            IsShown = true;
            DisplayMenu(true);
        }

        DontDestroyOnLoad(this.gameObject);
    }

    // Is start implies that it is the beginning of the game runtime
    public void DisplayMenu(bool displayed, bool isStart = false)
    {
        IsShown = displayed;

        if (displayed)
        {
            canvas.alpha = 1f;
            menu.SetActive(true);
            postProcessing.priority = 20;
            menuCam.m_Priority = 20;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.Background("Menu");
                AudioManager.Instance.StopBackground("TownScene");
            }
            return;
        }
        postProcessing.priority = 0;
        menuCam.m_Priority = 0;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        AudioManager.Instance.Background("ForestAmbient");

        if (!isStart)
        {
            camBrain.m_DefaultBlend.m_Time = 2f;
        }
    }

    public void DisableMenu()
    {
        menu.SetActive(false);
    }
}

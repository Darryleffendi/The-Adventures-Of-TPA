using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    private CursorLockMode prevLockState;
    private bool prevMouseState;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
            Pause(!pauseMenu.activeSelf);
    }

    public void Pause(bool x)
    {
        pauseMenu.SetActive(x);
        
        if (x)
        {
            Time.timeScale = 0f;
            prevLockState = Cursor.lockState;
            prevMouseState = Cursor.visible;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            return;
        }

        Time.timeScale = 1f;
        Cursor.lockState = prevLockState;
        Cursor.visible = prevMouseState;
        settingsMenu.SetActive(false);
    }

    public void MainMenu()
    {
        Pause(false);

        if (SceneManager.GetActiveScene().name != "TownScene")
        {
            SceneChanger.Instance.TownScene();
        }
        MenuConfig.Instance.DisplayMenu(true);
    }

    public void ExitApp()
    {
        Application.Quit();
    }
}

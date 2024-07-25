using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TMPro;

public class GameOver : MonoBehaviour
{
    public CanvasGroup canvas;
    public CanvasGroup canvasMain;
    public CinemachineVirtualCamera vcam;
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        AudioManager.Instance.Background("gameover");
        int score = GameStats.Instance.Score;
        scoreText.text = score.ToString();
        Inventory.Instance.addBalance(score);
        SettingMenu.Instance.ShowCrosshair();
    }

    void Update()
    {
        canvas.alpha -= 1f * Time.deltaTime;

        if (canvas.alpha < 0.5)
           vcam.m_Priority = 0;

        if (Time.time > 3)
        {
            canvasMain.alpha += 0.8f * Time.deltaTime;
        }
    }

    public void Retry()
    {
        SceneChanger.Instance.QuestScene();
    }

    public void Back()
    {
        SceneChanger.Instance.TownScene();
        MenuConfig.Instance.DisplayMenu(false);
        MenuConfig.Instance.DisableMenu();
    }
}

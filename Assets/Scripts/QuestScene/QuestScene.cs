using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestScene : MonoBehaviour
{
    [SerializeField]
    private GameObject rain;
    [SerializeField]
    protected TextMeshProUGUI timerText;
    [SerializeField]
    protected CanvasGroup fade;

    protected int timerSec = 0;
    protected int timerMin = 0;
    protected bool changeMin = false;
    protected bool endGame = false;
    protected int enemiesKilled = 0;

    private AudioManager audioManager;
    public static QuestScene Instance { get; private set; }

    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        
        SettingMenu.Instance.ShowCrosshair();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Physics.IgnoreLayerCollision(6, 7);
        audioManager = AudioManager.Instance;
        audioManager.Background("rain");
        audioManager.Background("music");
    }

    private void Update()
    {
        if (endGame)
        {
            fade.alpha += (1f * Time.deltaTime);

            if (fade.alpha >= 1)
            {
                Time.timeScale = 1f;
                SceneChanger.Instance.GameOver();
            }
            return;
        }

        rain.transform.position = new Vector3(Camera.main.transform.position.x, rain.transform.position.y, Camera.main.transform.position.z);

        timerSec = ((int)Time.time);

        if (timerSec % 25 == 0 && timerSec != 0) changeMin = true;
        if (timerSec % 60 == 0 && changeMin)
        {
            timerMin++;
            changeMin = false;
        }

        timerText.text = timerMin.ToString("D2") + ":" + (timerSec % 60).ToString("D2");
    }

    public void EnemyKill()
    {
        enemiesKilled++;
    }

    public void EndGame()
    {
        AudioManager.Instance.SoundEffect("fly", true);
        AudioManager.Instance.StopBackground("music");
        GameStats.Instance.timeElapsed = (int)Time.time;
        GameStats.Instance.enemiesKilled = enemiesKilled;
        Time.timeScale = 0.3f;
        endGame = true;
    }

}

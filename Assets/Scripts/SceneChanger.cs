using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System.Collections;

public class SceneChanger : MonoBehaviour
{
    public static SceneChanger Instance;
    public GameObject loadingScreen;
    public Image loadingBarFill;
    public TextMeshProUGUI loadingText;
    public Image questImg;
    public Image townImg;

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

    public void QuestScene()
    {
        questImg.enabled = true;
        townImg.enabled = false;
        StartCoroutine(LoadSceneAsync("QuestScene"));
    }

    public void TownScene()
    {
        questImg.enabled = false;
        townImg.enabled = true;
        StartCoroutine(LoadSceneAsync("TownScene"));
    }

    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        
        loadingScreen.SetActive(true);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress);
            loadingBarFill.fillAmount = progressValue;
            loadingText.text = ((int)(operation.progress * 100)).ToString() + " %";
            yield return null;
        }
        loadingScreen.SetActive(false);
    }
}

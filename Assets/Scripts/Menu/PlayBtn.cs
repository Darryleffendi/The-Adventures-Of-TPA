using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class PlayBtn : MonoBehaviour
{
    public CinemachineBrain camBrain;
    private float timeInterval = Mathf.Infinity;
    public CanvasGroup canvas;


    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(
            () =>
            {
                MenuConfig.Instance.DisplayMenu(false, true);

                AudioManager.Instance.SoundEffect("fly");
                AudioManager.Instance.StopBackground("Menu");
                AudioManager.Instance.Background("ForestAmbient");

                timeInterval = Time.time + 4f;
            });
    }

    private void Update()
    {
        if (timeInterval != Mathf.Infinity)
        {
            canvas.alpha -= (1f * Time.deltaTime);

            if (Time.time - timeInterval > 3f)
            {
                MenuConfig.Instance.DisableMenu();

                camBrain.m_DefaultBlend.m_Time = 2f;
                AudioManager.Instance.Background("TownScene");
                timeInterval = Mathf.Infinity;
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestBtn : MonoBehaviour
{
    string questName = " ";
    bool isActive = false;

    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(
            () => {
                if (!isActive) return;

                AudioManager.Instance.Dialog("questEnter");

                if (questName == "Sentinels of the Eternal Prism")
                    AudioManager.Instance.StopBackground("TownScene");
                    AudioManager.Instance.StopBackground("ForestAmbient");
                SceneChanger.Instance.QuestScene();
            }
        );
    }

    public void Initialize(string questName, bool isActive)
    {
        this.isActive = isActive;
        this.questName = questName;
    }
}

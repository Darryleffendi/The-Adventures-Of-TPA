using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStats : MonoBehaviour
{
    public static GameStats Instance;
    [HideInInspector]
    public int enemiesKilled = 0, timeElapsed = 0;
    public int Score
    {
        get { return timeElapsed * 100 + enemiesKilled * 500; }
    }
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
}

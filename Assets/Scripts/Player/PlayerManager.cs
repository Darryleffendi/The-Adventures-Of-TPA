using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [SerializeField]
    private Player knight, dogKnight, wizard;
    [SerializeField]
    private GameObject knightHealth, dogKnightHealth, wizardHealth;
    private List<Player> playerList = new List<Player>();
    private int index = 0;

    // Start is called before the first frame update
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

        playerList.Add(knight);
        playerList.Add(dogKnight);
        playerList.Add(wizard);

        knight.SetPlayerActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerList.Count <= 0)
        {
            // end of game
            QuestScene.Instance.EndGame();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            SwitchPlayer();
        }
    }

    public void SwitchPlayer()
    {
        if (playerList.Count <= 1) return;

        int prevIndex = index;
        index++;
        if (index >= playerList.Count) index = 0;

        Player p = playerList[index];
        p.StopAllCoroutines();
        p.SetPlayerActive(true);
        p.cinemachineCamera.m_Priority = 10;
        showHealth(p, true);
        p.UpdateHealth();
        p.UpdateMana();

        p = playerList[prevIndex];
        p.SetPlayerActive(false);
        p.cinemachineCamera.m_Priority = 0;
        showHealth(p, false);
        p.UpdateHealth();
    }

    private void showHealth(Player p, bool boolean)
    {
        if (p is Wizard)
        {
            wizardHealth.SetActive(boolean);
        }
        else if (p is Knight)
        {
            knightHealth.SetActive(boolean);
        }
        else if (p is DogKnight)
        {
            dogKnightHealth.SetActive(boolean);
        }
    }

    public List<Player> GetPlayers()
    {
        return playerList;
    }

    public void Remove(Player p)
    {
        playerList.Remove(p);
    }

    public Player GetActive()
    {
        foreach (Player p in playerList)
        {
            if (p.GetActive())
                return p;
        }
        return null;
    }
}

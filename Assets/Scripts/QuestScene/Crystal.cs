using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Crystal : MonoBehaviour
{
    [SerializeField]
    protected float maxHp = 0f;
    protected float hp = 0f;
    [SerializeField]
    protected Transform healthBar;

    void Start()
    {
        hp = maxHp;
        UpdateHealth();
    }

    public void DeductHp(float damage)
    {
        hp -= damage;
        UpdateHealth();

        if (hp <= 0)
        {
            // End game
            QuestScene.Instance.EndGame();
        }
    }

    private void UpdateHealth()
    {
        healthBar.localScale = new Vector3((hp / maxHp) * -0.1f, 0.022f, 0.7f);
        if (hp > maxHp * 2 / 3)
        {
            healthBar.GetComponent<Image>().color = Color.green;
        }
        else if (hp > maxHp * 1 / 3)
        {
            healthBar.GetComponent<Image>().color = Color.yellow;
        }
        else
        {
            healthBar.GetComponent<Image>().color = Color.red;
        }
        healthBar.GetComponent<Image>().CrossFadeAlpha(0.1f, 0f, false); ;
    }
}

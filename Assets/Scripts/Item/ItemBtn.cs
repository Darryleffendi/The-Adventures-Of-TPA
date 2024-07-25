using UnityEngine;
using UnityEngine.UI;

public class ItemBtn : MonoBehaviour
{
    public int id;
    public BuffType[] buff;
    public float buffAmount;

    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(
            () => {
                PlayerManager playerManager = PlayerManager.Instance;
                if (playerManager == null)
                {
                    TextHandler.Instance.ItemError();
                    return;
                }
                Player p = playerManager.GetActive();

                foreach (BuffType b in buff)
                {
                    if (b == BuffType.Speed)
                    {
                        p.speedBuff = true;
                        p.buffAmount = buffAmount;
                        p.buffInterval = Time.time + 4f;
                    }
                    else if (b == BuffType.Health)
                    {
                        p.AddHp(p.GetMaxHp() * buffAmount);
                    }
                    else
                    {
                        p.AddMana(p.GetMaxMana() * buffAmount);
                    }
                }

                Destroy(this.gameObject);
            }
        );
    }
}

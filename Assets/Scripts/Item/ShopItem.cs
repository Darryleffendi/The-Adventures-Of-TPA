using UnityEngine.UI;
using UnityEngine;

public class ShopItem : MonoBehaviour
{
    [SerializeField]
    private Item item;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(
            () => {
                if (Inventory.Instance.GetCapacity() >= 8)
                {
                    AudioManager.Instance.Dialog("notEnoughBackpack");
                    return;
                }
                if (Inventory.Instance.deductBalance(item.price))
                {
                    Inventory.Instance.Add(item);
                    Shop.Instance.InitializeShop();
                    AudioManager.Instance.SoundEffect("purchase");
                }
                else
                {
                    AudioManager.Instance.Dialog("insufficientBalance");
                }
            }
        );
    }

    public void SetItem(Item i)
    {
        item = i;
    }
}

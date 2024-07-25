using UnityEngine;
using UnityEngine.UI;

public class CloseButtonShop : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(
            () => { 
                Shop.Instance.ShowShop(false); 
            } 
        );
    }
}

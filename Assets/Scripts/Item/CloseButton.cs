using UnityEngine;
using UnityEngine.UI;

public class CloseButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(
            () => { 
                Inventory.Instance.ShowInventory(false); 
            } 
        );
    }
}

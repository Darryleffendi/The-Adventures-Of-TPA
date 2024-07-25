using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class Shop : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    protected GameObject itemObject, shopObject, balanceObject;
    [SerializeField]
    protected Transform shopContent;
    [SerializeField]
    protected List<Item> items = new List<Item>();
    [SerializeField]
    protected CinemachineFreeLook playerCam;
    [SerializeField]
    protected CinemachineVirtualCamera shopCam;

    private bool shown = false;
    
    public static Shop Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            InitializeShop();
        }
    }

    void Add(Item i)
    {
        items.Add(i);
    }

    void Remove(Item i)
    {
        items.Remove(i);
    }

    public void ShowShop(bool a)
    {
        // If true, then toggle
        if (a)
        {
            a = !shown;
        }

        shopObject.SetActive(a);
        Cursor.visible = a;
        shown = a;

        if (a)
        {
            Cursor.lockState = CursorLockMode.None;
            InitializeShop();
            shopCam.m_Priority = 10;
            playerCam.m_Priority = 0;
        }
        else
        {
            if (Inventory.Instance.GetShown())
            {
                Cursor.visible = true;
            }
            else 
                Cursor.lockState = CursorLockMode.Locked;
            
            shopCam.m_Priority = 0;
            playerCam.m_Priority = 10;
        }
    }

    public void InitializeShop()
    {
        foreach (Transform item in shopContent)
        {
            Destroy(item.gameObject);
        }

        foreach (var item in items)
        {
            GameObject obj = Instantiate(itemObject, shopContent);
            obj.transform.Find("Title").GetComponent<TMPro.TextMeshProUGUI>().text = item.itemName;
            obj.transform.Find("Description").GetComponent<TMPro.TextMeshProUGUI>().text = item.description;
            obj.transform.Find("Price").GetComponent<TMPro.TextMeshProUGUI>().text = item.price.ToString();
            obj.transform.Find("ItemBorder/ItemImg").GetComponent<Image>().sprite = item.itemImg;
            obj.GetComponent<ShopItem>().SetItem(item);
        }

        balanceObject.GetComponent<TMPro.TextMeshProUGUI>().text = FindObjectOfType<Inventory>().GetBalance().ToString();
    }

    public bool GetShown()
    {
        return shown;
    }
}

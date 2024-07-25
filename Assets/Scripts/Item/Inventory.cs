using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    protected GameObject itemObject, inventoryObject, balanceObject, capacityObject;
    [SerializeField]
    protected Transform inventoryContent;

    protected List<Item> items = new List<Item>();
    private bool shown = false;
    private int balance = 200;
    private int capacity = 0;
    
    public static Inventory Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Add(Item i)
    {
        items.Add(i);
        InitializeInventory();
    }

    void Remove(Item i)
    {
        items.Remove(i);
    }

    public void ShowInventory(bool a)
    {
        // If true, then toggle
        if (a)
        {
            a = !shown;
        }

        inventoryObject.SetActive(a);
        Cursor.visible = a;
        shown = a;

        if (a)
        {
            Cursor.lockState = CursorLockMode.None;
            InitializeInventory();
        }
        else
        {
            var shop = FindObjectOfType<Shop>();
            if (shop != null && FindObjectOfType<Shop>().GetShown())
            {
                Cursor.visible = true;
                return;
            }
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    public void InitializeInventory()
    {
        foreach (Transform item in inventoryContent)
        {
            Destroy(item.gameObject);
        }
        capacity = 0;

        foreach (var item in items)
        {
            GameObject obj = Instantiate(itemObject, inventoryContent);
            obj.transform.Find("ItemImg").GetComponent<Image>().sprite = item.itemImg;
            obj.GetComponent<ItemBtn>().id = item.id;
            obj.GetComponent<ItemBtn>().buff = item.buff;
            obj.GetComponent<ItemBtn>().buffAmount = item.buffAmount;
            capacity++;
        }

        balanceObject.GetComponent<TMPro.TextMeshProUGUI>().text = balance.ToString();
        capacityObject.GetComponent<TMPro.TextMeshProUGUI>().text = capacity.ToString() + "/8";
    }

    public int GetBalance()
    {
        return balance;
    }

    public int GetCapacity()
    {
        return capacity;
    }

    public bool deductBalance(int amount)
    {
        if (balance < amount)
        {
            return false;
        }
        balance -= amount;
        return true;
    }

    public void addBalance(int amount)
    {
        if (amount >= 0)
        {
            balance += amount;
        }
    }

    public bool GetShown()
    {
        return shown;
    }
}

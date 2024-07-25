using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;

public class Quest : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    protected GameObject itemObject, itemObjectDisabled, questObject;
    [SerializeField]
    protected Transform questContent;
    [SerializeField]
    protected QuestItem[] quests;
    [SerializeField]
    protected CinemachineFreeLook playerCam;
    [SerializeField]
    protected CinemachineVirtualCamera questCam;

    private bool shown = false;
    
    public static Quest Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    public void ShowQuest(bool a)
    {
        // If true, then toggle
        if (a)
        {
            a = !shown;
        }

        questObject.SetActive(a);
        Cursor.visible = a;
        shown = a;

        if (a)
        {
            Cursor.lockState = CursorLockMode.None;
            InitializeQuest();
            questCam.m_Priority = 10;
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

            questCam.m_Priority = 0;
            playerCam.m_Priority = 10;
        }
    }

    public void InitializeQuest()
    {
        foreach (Transform quest in questContent)
        {
            Destroy(quest.gameObject);
        }

        foreach (var quest in quests)
        {
            GameObject obj;
            if (quest.questEnabled)
            {
                obj = Instantiate(itemObject, questContent);
            }
            else
            {
                obj = Instantiate(itemObjectDisabled, questContent);
            }
             
            obj.transform.Find("Title").GetComponent<TMPro.TextMeshProUGUI>().text = quest.questName;
            obj.transform.Find("Description").GetComponent<TMPro.TextMeshProUGUI>().text = quest.description;
            obj.transform.Find("ItemBorder/ItemImg").GetComponent<Image>().sprite = quest.questImg;
            obj.transform.GetComponent<QuestBtn>().Initialize(quest.questName, quest.questEnabled);
        }
    }

    public bool GetShown()
    {
        return shown;
    }
}

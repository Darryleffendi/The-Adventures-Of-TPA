using UnityEngine;
using UnityEngine.UI;

public class CloseButtonQuest : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(
            () => {
                Quest.Instance.ShowQuest(false);
            }
        );
    }
}

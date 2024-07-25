using UnityEngine;
using UnityEngine.UI;

public class ExitBtn : MonoBehaviour
{
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(
               () =>
               {
                   Application.Quit();
               });
    }
}

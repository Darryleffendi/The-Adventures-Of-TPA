using UnityEngine;

public class TownScene : MonoBehaviour
{
    public static TownScene Instance;

    private void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        if (MenuConfig.Instance.IsShown)
        {
            MenuConfig.Instance.DisplayMenu(true);
        }
        else
            AudioManager.Instance.Background("TownScene");

        SettingMenu.Instance.ShowCrosshair();
    }
}

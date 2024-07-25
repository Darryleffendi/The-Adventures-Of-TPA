using UnityEngine;
using TMPro;

public class TextHandler : MonoBehaviour
{
    [SerializeField]
    protected TextMeshProUGUI shopText, outpostText, shopGreet1, shopGreet2, interactText, itemError;

    public static TextHandler Instance { get; private set; }

    private float errorTime = 0;
    private float timeEnter = 0;
    private float timeInside = 0;
    private bool shopEntered = false;
    private bool outpostEntered = false;
    private bool shopInside = false;

    // Start is called before the first frame update
    void Start()
    {
        shopText.alpha = 0;
        outpostText.alpha = 0;

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        ShopTextEnter();
        OutputTextEnter();
        ShopTextInside();
        if (Time.time > errorTime)
            itemError.alpha = 0;
    }

    public void ShopEnter()
    {
        if (!shopEntered)
        {
            timeEnter = Time.time;
            outpostText.alpha = 0;
            outpostEntered = false;
            shopEntered = true;
        }
    }
    
    private void ShopTextEnter()
    {
        if (!shopEntered) return;

        if (Time.time - timeEnter < 2)
        {
            shopText.alpha = Mathf.Lerp(shopText.alpha, .85f, 2f * Time.deltaTime);
        }
        else if (Time.time - timeEnter > 3)
        {
            shopText.alpha = Mathf.Lerp(shopText.alpha, 0, 6f * Time.deltaTime);
        }
        if (shopText.alpha < 0.01)
        {
            shopEntered = false;
            shopText.alpha = 0;
        }
    }

    public void ShopInside()
    {
        if (Time.time - timeInside < 0.5) return;

        if (!shopInside)
        {
            timeInside = Time.time;
            shopInside = true;
            shopGreet1.alpha = 0.02f;
            shopGreet2.alpha = 0f;
            AudioManager.Instance.Dialog("shopGreet1");
            return;
        }
        else
        {
            timeInside = Time.time;
            shopInside = false;
            shopGreet2.alpha = 0.02f;
            shopGreet1.alpha = 0f;
            AudioManager.Instance.Dialog("shopGreet2");
            return;
        }
    }

    private void ShopTextInside()
    {
        if (shopGreet1.alpha == 0 && shopGreet2.alpha == 0) return;

        if (shopInside)
        {
            if (Time.time - timeInside < 1)
            {
                shopGreet1.alpha = Mathf.Lerp(shopGreet1.alpha, .65f, 2f * Time.deltaTime);
            }
            else if (Time.time - timeInside > 2.5)
            {
                shopGreet1.alpha = Mathf.Lerp(shopGreet1.alpha, 0, 3.5f * Time.deltaTime);
            }
            if (shopGreet1.alpha < 0.01)
            {
                shopGreet1.alpha = 0;
            }
        }
        else
        {
            if (Time.time - timeInside < 1)
            {
                shopGreet2.alpha = Mathf.Lerp(shopGreet2.alpha, .65f, 2f * Time.deltaTime);
            }
            else if (Time.time - timeInside > 2.5)
            {
                shopGreet2.alpha = Mathf.Lerp(shopGreet2.alpha, 0, 3.5f * Time.deltaTime);
            }
            if (shopGreet2.alpha < 0.01)
            {
                shopGreet2.alpha = 0;
            }
        }
    }

    public void OutpostEnter()
    {
        if (!outpostEntered)
        {
            timeEnter = Time.time;
            shopText.alpha = 0;
            shopEntered = false;
            outpostEntered = true;
        }
    }

    private void OutputTextEnter()
    {
        if (!outpostEntered) return;

        if (Time.time - timeEnter < 2)
        {
            outpostText.alpha = Mathf.Lerp(outpostText.alpha, .85f, 2f * Time.deltaTime);
        }
        else if (Time.time - timeEnter > 3)
        {
            outpostText.alpha = Mathf.Lerp(outpostText.alpha, 0, 6f * Time.deltaTime);
        }
        if (outpostText.alpha < 0.01)
        {
            outpostEntered = false;
            outpostText.alpha = 0;
        }
    }

    public void InteractText(bool shown)
    {
        shopGreet1.alpha = 0;
        shopGreet2.alpha = 0;
        
        if (shown)
        {
            interactText.alpha = 0.85f;
        }
        else
        {
            interactText.alpha = 0f;
        }
    }

    public void ItemError()
    {
        itemError.alpha = 1f;
        errorTime = Time.time + 1f;
    }
}

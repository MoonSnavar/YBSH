using UnityEngine;
using UnityEngine.UI;

public class ItemBlock : MonoBehaviour
{
    public ItemBlockManager ItemBlockManager;
    public Text statusText;
    public int price;
    public Text priceText;
    public string itemName;
    public int pointOfShield;
    public static bool canUpdate;
    private bool canBuy;
    private AudioSource buyingSound;

    private void OnEnable()
    {
        buyingSound = GetComponent<AudioSource>();
        priceText.text = price.ToString();
        ChangeStateItemBlock();
    }
    private void Update()
    {
        if(canUpdate)
        {
            ItemBlockManager.ChangeStateAllItems();
            canUpdate = false;
        }

    }

    public void ChangeStateItemBlock()
    {
        int tempCurrency = PlayerPrefs.GetInt("CurrencyPoints");
        if (PlayerPrefs.GetInt(itemName) == 1) //если предмет уже был куплен
        {
            GetComponent<Image>().color = new Color(136 / 255f, 130 / 255f, 185 / 255f, 1f); //светлее
            statusText.text = "Куплено";
            canBuy = false;
        }
        else if (price > tempCurrency) //если не хватает валюты
        {
            GetComponent<Image>().color = new Color(50 / 255f, 43 / 255f, 106 / 255f, 1f); //темнее
            canBuy = false;
            statusText.text = "Недостаточно бумажек";
        }
        else if(price <= tempCurrency) //если предмет доступен к покупке
        {
            GetComponent<Image>().color = new Color(95 / 255f, 88 / 255f, 152 / 255f, 1f);
            canBuy = true;
            statusText.text = "Купить";
        }
    }
    private void OnMouseDown()
    {
        if (canBuy)
        {
            buyingSound.Play();
            PlayerPrefs.SetInt(itemName, 1);             
            int tempCurrencyPoints = PlayerPrefs.GetInt("CurrencyPoints");
            PlayerPrefs.SetInt("CurrencyPoints", tempCurrencyPoints - price);
            //прибавление к очкам защиты
            int tempCountpointsOfShield = PlayerPrefs.GetInt("CountpointsOfShield");
            PlayerPrefs.SetInt("CountpointsOfShield", tempCountpointsOfShield + pointOfShield);
            ItemBlockManager.ChangeStateAllItems();
        }
    }
}

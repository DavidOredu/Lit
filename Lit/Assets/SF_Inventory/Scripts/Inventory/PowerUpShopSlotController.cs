using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// </summary>
public class PowerUpShopSlotController : SlotController
{
    public static Action<Item> OnItemBought;
    public GameObject coins;
    public GameObject darkMatter;
    private void Start()
    {
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        var displayText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        var displayImage = transform.Find("ItemIcon").GetComponent<Image>();
        var strengthFrame = transform.Find("Frame").GetComponent<Image>();
        var itemPrice = transform.Find("BuyButton").Find("ItemPrice").GetComponent<TextMeshProUGUI>();

        if (item)
        {
            displayText.text = item.name;
            displayImage.sprite = item.icon;
            displayImage.color = Color.white;
            strengthFrame.color = item.GetStrengthColor();
            itemPrice.text = "" + item.price;

            if (item.currency == Currency.coins)
            {
                coins.SetActive(true);
                darkMatter.SetActive(false);
            }
            else
            {
                coins.SetActive(false);
                darkMatter.SetActive(true);
            }
        }
        else
        {
            displayText.text = "";
            displayImage.sprite = null;
            displayImage.color = Color.clear;
            itemPrice.text = "-";
        }
    }

    public void buy()
    {
        bool paid;
        paid = item.currency == Currency.coins
            ? DataSaver.saver.payGold(item.price)
            : DataSaver.saver.payPremiumCurrency(item.price);

        // iff successfull:
        if (paid)
        {
            item.AddItemAmount();
            Debug.Log("Paid");
            OnItemBought?.Invoke(item);
            DisplayInfo.Instance.UpdateInfo();
        }
        else
        {
            ErrorDisplayer.instance.DisplayMessage("You dont have enougy gold for that item!");
        }
    }

    private bool payGold(int price)
    {
        int coins = PlayerPrefs.GetInt(PlayerPrefsReferences.coins, 0);
        if (coins < price)
        {
            Debug.Log("You dont have enougy gold for that item!");
            return false;
        }

        coins -= price;
        PlayerPrefs.SetInt(PlayerPrefsReferences.coins, coins);
        return true;
    }
}
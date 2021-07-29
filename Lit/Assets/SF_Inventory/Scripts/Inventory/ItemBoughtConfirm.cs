using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Displays an InfoPanel to show the user that he successfully bought an item!
/// </summary>
public class ItemBoughtConfirm : MonoBehaviour
{
    public GameObject confirmPaymentPanel;
    public Image icon;
    private void OnEnable()
    {
        PowerUpShopSlotController.OnItemBought += ConfirmPayment;
    }

    private void OnDisable()
    {
        PowerUpShopSlotController.OnItemBought -= ConfirmPayment;
    }

    private void ConfirmPayment(Item item)
    {
        confirmPaymentPanel.SetActive(true);
        icon.sprite = item.icon;
    }
}
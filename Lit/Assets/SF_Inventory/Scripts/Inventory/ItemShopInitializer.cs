using System;
using UnityEngine;


/// <summary>
/// Automaticaly initializes the ItemShop with all items saved in the ItemList
/// </summary>
public class ItemShopInitializer : MonoBehaviour
{
    public GameObject shopSlot;

    private void Start()
    {
        InitializeItemShop();
    }


    private void InitializeItemShop()
    {
        foreach (var item in ItemList.instance.list)
        {
            var obj = Instantiate(shopSlot, transform);
            var controller = obj.GetComponent<PowerUpShopSlotController>();
            controller.item = item;
            controller.UpdateInfo();
        }
    }
}
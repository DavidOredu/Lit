using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The user can select a specific amount of 
/// </summary>
public class InventorySlotController : SlotController
{
    public static Action<int> OnSelectItem;

    /// <summary>
    /// uses the item and disables the gameobject from the scene.
    /// only 1 of the same itemType should be allowed to be used in one mission
    /// </summary>
    public void UseItem()
    {
        if (!item) return;
        item.Use();
        Debug.Log("Item Used: " + item.name);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// used for the inventory, to select the item that should be activated in the next mission
    /// Restrictions:
    /// - can not select 2 power ups/ items of same type at the same time
    /// - restricted maximum selected amount --> Inventory.instance.maxSelectedItems
    /// </summary>
    public void SelectItem()
    {
        if (!item) return;
        var isItemActivated = item.IsActivated();
        var selected = transform.Find("Green");
        if (!isItemActivated)
        {
            if (ItemList.instance.HasActiveItem(item.GetType()))
            {
                ErrorDisplayer.instance.DisplayMessage("Can`t select 2 power ups of the same type");
                return;
            }

            if (Inventory.instance.GetSelectedItemAmount() >= Inventory.instance.maxSelectedItems)
            {
                ErrorDisplayer.instance.DisplayMessage("max Power up amount selected!");
                return;
            }

            item.SetActive(true);
            selected.gameObject.SetActive(true);
            // increase selected items
            OnSelectItem?.Invoke(1);
        }
        else
        {
            item.SetActive(false);
            selected.gameObject.SetActive(false);
            // decrease selected items
            OnSelectItem?.Invoke(-1);
        }
    }

    /// <summary>
    /// updates the info in the inventory, where items can be stacked
    /// </summary>
    public void UpdateInfo()
    {
        var displayText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        var displayImage = transform.Find("ItemIcon").GetComponent<Image>();
        var itemAmount = transform.Find("Amount").GetComponent<TextMeshProUGUI>();
        ;
        if (item)
        {
            var itemConfig = item.GetItemSaveName().Split('_');
            displayText.text = item.name;
            displayImage.sprite = item.icon;
            displayImage.color = Color.white;
            itemAmount.text = item.getItemAmount() > 1 ? "" + item.getItemAmount() : "";
            Image strengthFrame = transform.Find("Frame").GetComponent<Image>();
            strengthFrame.color = item.GetStrengthColor();
        }
        else
        {
            displayText.text = "";
            itemAmount.text = "";
            displayImage.sprite = null;
            displayImage.color = Color.clear;
        }
    }

    /// <summary>
    /// updates the info in the CampaignPlay, where only one item per item type can be used
    /// </summary>
    public void UpdateSelectedInfo()
    {
        var displayText = transform.Find("Text").GetComponent<TextMeshProUGUI>();
        var displayImage = transform.Find("ItemIcon").GetComponent<Image>();
        if (item)
        {
            displayText.text = item.name;
            displayImage.sprite = item.icon;
            displayImage.color = Color.white;
        }
        else
        {
            displayText.text = "";
            displayImage.sprite = null;
            displayImage.color = Color.clear;
        }
    }
}
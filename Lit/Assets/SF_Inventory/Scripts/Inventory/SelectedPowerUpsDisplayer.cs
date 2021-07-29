using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// shows the selected power ups of the user, which can then be activated by clicking on the according power up!
/// </summary>
public class SelectedPowerUpsDisplayer : MonoBehaviour
{
    public GameObject inventoryPanel;
    public ItemList itemList;
    [Header("Should be same Nr as displayed Child Slots!")]
    public int maxActivatedPowerUps = 4;
    
    List<Item> list;
    public void Start()
    {
        list = itemList.list;
        UpdatePanelSlotsForUsedItems();
    }

    /// <summary>
    /// updates the slots with the selected items from the user
    /// </summary>
    public void UpdatePanelSlotsForUsedItems()
    {
        var index = 0;
        foreach (var item in list)
        {
            // do not take more items with you when there is no more space
            if (index >= maxActivatedPowerUps)
            {
                return;
            }
            if (!item.IsActivated())
            {
                continue;
            }

            var obj = inventoryPanel.transform.GetChild(index);
            obj.gameObject.SetActive(true);
            var slot= obj.GetComponent<InventorySlotController>();
            slot.item = item;
            slot.UpdateSelectedInfo();
            index++;
        }
        // update the rest of the free spots
        for(var i = index; i < maxActivatedPowerUps; i++)
        {
            var obj = inventoryPanel.transform.GetChild(i);
            obj.gameObject.SetActive(true);
            var slot= obj.GetComponent<InventorySlotController>();
            slot.item = null;
            slot.UpdateSelectedInfo();
        }
    }

}

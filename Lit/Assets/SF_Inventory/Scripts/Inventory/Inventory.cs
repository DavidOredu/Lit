using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// should be assigned directly to inventory panel gameobject
/// initializes the inventory with the items of the user
/// </summary>
public class Inventory : MonoBehaviour
{
    public static Inventory instance;
    public ItemList itemList;
    public GameObject inventorySlot;

    public int maxSelectedItems;
    private int selectedItemCount;
    private List<Item> list;


    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Subscribes to OnSelectItem event and initializes the Inventory
    /// </summary>
    private void OnEnable()
    {
        InventorySlotController.OnSelectItem += ChangeSelectedItemCount;
        selectedItemCount = 0;
        list = itemList.list;
        CreateInventorySlots();
    }

    /// <summary>
    /// Resets the inventory
    /// so it can be reInitialized when the user opens the inventory again.
    /// </summary>
    private void OnDisable()
    {
        foreach (Transform child in transform) {
           Destroy(child.gameObject);
        }
        InventorySlotController.OnSelectItem -= ChangeSelectedItemCount;
    }

    /// <summary>
    /// initializes the empty inventory with the items of the user!
    /// </summary>
    private void CreateInventorySlots()
    {
        foreach (var item in list)
        {
            Debug.Log(item.getItemAmount() + " items of " + item.name);
            if (item.getItemAmount() <= 0) continue;
            var slot = Instantiate(inventorySlot,
                new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity,
                gameObject.transform);
            var slotController = slot.GetComponent<InventorySlotController>();
            slotController.item = item;
            if (item.IsActivated())
            {
                selectedItemCount++;
                slot.transform.Find("Green").gameObject.SetActive(true);
            }

            slotController.UpdateInfo();
        }

        Debug.Log("Alredy selected items : " + selectedItemCount);
    }

    /// <summary>
    /// tracks the number of selected items
    /// user can only have a defined amount of items/PowerUps activated at the same time
    /// </summary>
    /// <param name="val">+1 or -1</param>
    private void ChangeSelectedItemCount(int val)
    {
        selectedItemCount += val;
        Debug.Log("New Item Count = " + selectedItemCount);
    }

    public int GetSelectedItemAmount()
    {
        return selectedItemCount;
    }
}
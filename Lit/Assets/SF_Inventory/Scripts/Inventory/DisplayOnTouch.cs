using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


/// <summary>
/// Special functionallity for item icons
/// Click (and hold) the slot icons to display a seperate window with the Item description
/// Integrated in the InventorySlot and ShopSLot prefab!
/// </summary>
public class DisplayOnTouch : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public SlotController slotController;
    public GameObject itemInfo;
    public TextMeshProUGUI displayText;

    private bool isMessageSet;
    private bool touched;
    private int pointerID;

    /// <summary>
    /// displays the item description if user touches the item icon
    /// </summary>
    public void OnPointerDown(PointerEventData data)
    {
        if (touched) return;
        // initialize message on first click
        if (!isMessageSet)
        {
            SetMessage();
        }

        itemInfo.SetActive(true);
        touched = true;
        pointerID = data.pointerId;
    }

    /// <summary>
    /// if user removes the finger from the icon, the description panel will be disabled again 
    /// </summary>
    public void OnPointerUp(PointerEventData data)
    {
        if (data.pointerId != pointerID) return;
        itemInfo.SetActive(false);
        touched = false;
    }

    private void SetMessage()
    {
        var item = slotController.getItem();
        if (!item) return;
        var message = item.GetItemSaveName();
        if (item.duration != 0)
        {
            displayText.text = message + ". Duration: " + item.duration + " s";
            return;
        }

        displayText.text = message;
        isMessageSet = true;
    }
}
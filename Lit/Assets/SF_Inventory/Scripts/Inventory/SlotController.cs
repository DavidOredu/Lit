using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic SlotController class
/// Inventory and PowerUp controllers inherit from this class
/// </summary>
public class SlotController : MonoBehaviour
{
    
    public Item item;

    public Item getItem()
    {
        return item;
    }
}

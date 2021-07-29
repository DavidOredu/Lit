using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemList : MonoBehaviour
{
    public static ItemList instance;
    public void Awake()
    {
        instance = this;
    }
    public List<Item> list = new List<Item>();


    public void Add(Item item)
    {
        if (list.Contains(item))
        {
            Item oldItem = list.Find(val => val.name == item.name);
            oldItem.AddItemAmount();
        }
        list.Add(item);
    }

    public bool HasActiveItem(string type)
    {
       return list.FirstOrDefault(item => item.GetType() == type && item.IsActivated()) != null;
    }



    public void UseItem(Item item)
    {
        item.DecreaseItemAmount();
        if (item.getItemAmount() <= 0)
        {
            list.Remove(item);
        }
    }
}

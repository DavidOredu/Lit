using UnityEngine;
using System;

public enum Currency
{
    coins,
    darkMatter
}

public class Item : ScriptableObject
{
    [SerializeField] [Header("Once set, do not change again --> used for Saving")]
    protected string itemSavingName;

    [Header("Item Attributes")] public Sprite icon;
    public Currency currency;
    public int price = 0;
    public int count = 0;
    public int duration = 0;

    private bool isActivated = false;

    public new virtual string GetType()
    {
        return "";
    }

    /// <summary>
    /// stat increasement order:
    /// * damage
    /// * health
    /// * speed
    /// * fireRate
    /// </summary>
    public static Action<float, float, float, float> OnPlayerStatIncreasement;
    

    public virtual void Use()
    {
    }

    public void AddItemAmount()
    {
        count++;
        PlayerPrefs.SetInt(itemSavingName, count);
    }

    public void DecreaseItemAmount()
    {
        count--;
        PlayerPrefs.SetInt(itemSavingName, count);
    }

    public void SetItemCount(int nr)
    {
        count = nr;
        PlayerPrefs.SetInt(itemSavingName, count);
    }

    public int getItemAmount()
    {
        count = PlayerPrefs.GetInt(itemSavingName, 0);
        return count;
    }

    /// <summary>
    /// default return is white
    /// for other use case please overwtire this method on the subclasses
    /// </summary>
    /// <returns></returns>
    public virtual Color GetStrengthColor()
    {
        return new Color(1, 1, 1);
    }

    /// <summary>
    /// returns the updated activated status
    /// </summary>
    public void SetActive(bool status)
    {
        isActivated = status;
    }

    /// <summary>
    /// deactivates the item if the last one of the item type is used!
    /// so it will not be displayed as selected in the inventory!
    /// </summary>
    protected void setActivationValue(bool val)
    {
        isActivated = val;
    }

    public string GetItemSaveName()
    {
        return itemSavingName;
    }

    public bool IsActivated()
    {
        return isActivated;
    }
}
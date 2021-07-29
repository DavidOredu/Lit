using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newConsumable", menuName = "Items/Consumable")]
public class Consumable : Item
{
    [Header("Consumable Attributes")]
    public PowerUpAttributes attribute;
    public Strength strength;



    public override void Use()
    {
        DecreaseItemAmount();
        if (getItemAmount() <= 0)
        {
            setActivationValue(false);
        }
        //ActivateEffect();
    }

    public override string GetType()
    {
        Debug.Log(attribute.ToString());
        return attribute.ToString();
    }
    /// <summary>
    /// Example method of how the item effects could be activated
    /// </summary>
    private void ActivateEffect()
    {
        float increasement = 1+ ((float) strength )/ 100.0f;
        if (increasement <= 0) return;
        switch (attribute)
        {
            // TODO Implement powerUpAttributes
            case PowerUpAttributes.dmg: return;
            case PowerUpAttributes.health: return;
            case PowerUpAttributes.speed: return;
            case PowerUpAttributes.fr: return;
            default : Debug.Log("stilll no power up possible for "+attribute);return;
        }
    }
    public override Color GetStrengthColor(){
        switch (strength)
        {
            case Strength.medium: return new Color(0.02f, 0.73f, 0.22f);
            case Strength.high: return new Color(1f, 0.57f, 0f);
            case Strength.ultra: return  new Color(0.76f, 0.18f, 0.14f);
            default: return new Color(1,1,1);
        }
    }
}

public enum PowerUpAttributes
{
    health, dmg,speed,fr ,coin
};
public enum Strength
{
    low = 15, 
    medium= 25,
    high= 50, 
    ultra = 100,
};
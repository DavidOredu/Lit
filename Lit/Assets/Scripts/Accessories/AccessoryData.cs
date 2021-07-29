using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewAccessoryData", menuName = "Data/Accessory Data")]
public class AccessoryData : ScriptableObject
{
    public Accessory[] headAccessories;
    [Space]
    public Accessory[] bodyCloth;
}

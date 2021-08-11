using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "NewDamageData", menuName = "Data/Damage Data")]
// This script is pretty useless now. Should be deleted
public class DamageData : ScriptableObject
{
    // used as an identifier
    public string damageName;
    // the type of damage to know what logic to use on it
    public DamageForm.DamagerType damageType;
    // is the runnner damaged
    public bool damaged = false;
}

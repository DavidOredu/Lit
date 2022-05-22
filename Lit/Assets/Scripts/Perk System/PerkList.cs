using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPerkList", menuName = "Data/Perks/Perk List")]
public class PerkList : ScriptableObject
{
    public List<Perk.PerkNames> perkNames = new List<Perk.PerkNames>();
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PerkHandler : MonoBehaviour
{
    public Racer racer;
    public RacerDamages racerDamages;

    [Header("PERKS")]
    public List<Perk> perks = new List<Perk>();

    public List<Perk> activePerks = new List<Perk>();
    void Start()
    {
        // rewrite perk initialize as follows: in the variable declaration, before going into the functions, create a list of all perks and add all perks to the list. Then in the start function, run a loop on all perks, then check if the current perk is part of the initial active perks using perkList. If it is, then we initialize the perk, else, else. Then if it is added, we add it to our list of active perks.

        List<PerkData> perkList = Resources.LoadAll<PerkData>($"{racer.runner.stickmanNet.currentColor.colorID}/Perks").ToList();

        for (int i = 0; i < perkList.Count; i++)
        {
            Perk newPerk = null;
            switch (perkList[i].perkNameType)
            {
                case Perk.PerkNames.DamageResistance:
                    newPerk = new Perk(perkList[i], racer.racerDamages.damageResistance);
                    break;
                case Perk.PerkNames.IncreasedSpeed:
                    newPerk = new Perk(perkList[i], racer.racerData.topSpeed);
                    break;
                case Perk.PerkNames.IncreasedBlastRadius:
                    newPerk = new Perk(perkList[i], racer.racerData.blastRadiusMultiplier);
                    break;
                case Perk.PerkNames.BurstSpeed:
                    newPerk = new Perk(perkList[i], racer.racerData.burstSpeed);
                    break;
                case Perk.PerkNames.ContactDamage:
                    newPerk = new Perk(perkList[i], racer.racerData.contactDamage);
                    break;
                case Perk.PerkNames.IncreasedDamage:
                    newPerk = new Perk(perkList[i], racer.racerData.contactDamage);
                    break;
                case Perk.PerkNames.IncreasedJumpVelocity:
                    newPerk = new Perk(perkList[i], racer.racerData.maxJumpVelocity);
                    break;
                case Perk.PerkNames.IncreasedJumpCount:
                    newPerk = new Perk(perkList[i], racer.racerData.amountOfJumps);
                    break;
                case Perk.PerkNames.StrengthIncreaseRateBuff:
                    newPerk = new Perk(perkList[i], racer.racerData.strengthIncreaseRate);
                    break;
                case Perk.PerkNames.BurnResistance:
                    break;
                case Perk.PerkNames.FreezeResistance:
                    break;
                case Perk.PerkNames.Mushin:
                    break;
                case Perk.PerkNames.HomingProjectile:
                    break;
                case Perk.PerkNames.ShieldAbsorption:
                    break;
                case Perk.PerkNames.ReducedSpeed:
                    break;
                case Perk.PerkNames.IncreasedGliderSpeed:
                    break;
                case Perk.PerkNames.HaltRacer:
                    break;
                case Perk.PerkNames.SabotageInput:
                    break;
                case Perk.PerkNames.IncreasedPowerupDuration:
                    break;
                default:
                    break;
            }
            perks.Add(newPerk);
        }

        InitializeActivePerks();
        ActivateGeneralPerks();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var perk in activePerks)
            perk.SetPerkActivity(racer.isInNativeMap, racer.isAwakened);

        for (int i = 0; i < perks.Count; i++)
        {
            switch (perks[i].perkName)
            {
                case Perk.PerkNames.DamageResistance:
                    racerDamages.damageResistance = perks[i].HandleActivePerk(racer.racerDamages.damageResistance);
                    break;
                case Perk.PerkNames.IncreasedSpeed:
                    racer.moveVelocityResource = perks[i].HandleActivePerk(racer.moveVelocityResource);
                    break;
                case Perk.PerkNames.IncreasedBlastRadius:
                    racerDamages.blastRadiusMultiplier = perks[i].HandleActivePerk(racerDamages.blastRadiusMultiplier);
                    break;
                case Perk.PerkNames.BurstSpeed:
                    racer.burstSpeed = perks[i].HandleActivePerk(racer.burstSpeed);
                    break;
                case Perk.PerkNames.ContactDamage:
                    racerDamages.contactDamage = perks[i].HandleActivePerk(racerDamages.contactDamage);
                    break;
                case Perk.PerkNames.IncreasedDamage:
                    break;
                case Perk.PerkNames.IncreasedJumpVelocity:
                    racer.jumpVelocity = perks[i].HandleActivePerk(racer.jumpVelocity);
                    break;
                case Perk.PerkNames.IncreasedJumpCount:
                    racer.amountOfJumps = perks[i].HandleActivePerk(racer.amountOfJumps);
                    break;
                case Perk.PerkNames.StrengthIncreaseRateBuff:
                    break;
                case Perk.PerkNames.BurnResistance:
                    break;
                case Perk.PerkNames.FreezeResistance:
                    break;
                case Perk.PerkNames.Mushin:
                    break;
                case Perk.PerkNames.HomingProjectile:
                    break;
                case Perk.PerkNames.ShieldAbsorption:
                    break;
                case Perk.PerkNames.ReducedSpeed:
                    break;
                case Perk.PerkNames.IncreasedGliderSpeed:
                    break;
                case Perk.PerkNames.HaltRacer:
                    break;
                case Perk.PerkNames.SabotageInput:
                    break;
                case Perk.PerkNames.IncreasedPowerupDuration:
                    break;
                default:
                    break;
            }
        }
    }

    private void InitializeActivePerks()
    {
        PerkList allowedPerks = Resources.Load<PerkList>($"{racer.runner.stickmanNet.currentColor.colorID}/PerkList");

        for (int i = 0; i < perks.Count; i++)
        {
            if (allowedPerks.perkNames.Contains(perks[i].perkName))
            {
                activePerks.Add(perks[i]);
            }
        }

    }
    private void ActivateGeneralPerks()
    {
        foreach (var perk in activePerks)
        {
            if (perk.perkData.perkActivity != PerkData.PerkActivity.General) { continue; }

            switch (perk.perkData.perkState)
            {
                case PerkData.PerkState.Base:
                    perk.ActivatePerk();
                    break;
                case PerkData.PerkState.Hyper:
                    if (perk.perkState == PerkData.PerkState.Base) { continue; }
                    perk.ActivatePerk();
                    break;
                case PerkData.PerkState.Ultimate:
                    if (perk.perkState != PerkData.PerkState.Ultimate) { continue; }
                    perk.ActivatePerk();
                    break;
            }
        }
    }
}

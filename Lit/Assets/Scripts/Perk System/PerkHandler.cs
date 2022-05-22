using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PerkHandler : MonoBehaviour
{
    public Racer racer;
    public RacerDamages racerDamages;

    [Header("PERKS")]
    public Perk DamageResistance;
    public Perk IncreasedSpeed;
    public Perk IncreasedBlastRadius;
    public Perk BurstSpeed;
    public Perk ContactDamage;
    public Perk IncreasedDamage;
    public Perk IncreasedJumpVelocity;
    public Perk IncreasedJumpCount;
    public Perk StrengthIncreaseRateBuff;
    public Perk BurnResistance;
    public Perk FreezeResistance;
    public Perk Mushin;
    public Perk HomingProjectile;
    public Perk ShieldAbsorption;
    public Perk ReducedSpeed;
    public Perk IncreasedGliderSpeed;
    public Perk HaltRacer;
    public Perk SabotageInput;
    public Perk IncreasedPowerupDuration;

    public List<Perk> perks = new List<Perk>();
    void Start()
    {
        
        PerkList perkList = Resources.Load<PerkList>($"{racer.runner.stickmanNet.currentColor.colorID}/Perks/PerkList");
        
        List<PerkData> perkData = Resources.LoadAll<PerkData>($"{racer.runner.stickmanNet.currentColor.colorID}/Perks").ToList();

        Dictionary<Perk.PerkNames, PerkData> myPerks = new Dictionary<Perk.PerkNames, PerkData>();
        foreach (var perkDatum in perkData)
        {
            myPerks.Add(perkDatum.perkNameType, perkDatum);
        }

        if (perkList.perkNames.Contains(Perk.PerkNames.DamageResistance))
        {
            DamageResistance = new Perk(myPerks[Perk.PerkNames.DamageResistance], racer.racerData.damageResistance);
            perks.Add(DamageResistance);
        }
        if (perkList.perkNames.Contains(Perk.PerkNames.IncreasedSpeed))
        {
            IncreasedSpeed = new Perk(myPerks[Perk.PerkNames.IncreasedSpeed], racer.racerData.topSpeed);
            perks.Add(IncreasedSpeed);
        }
        if (perkList.perkNames.Contains(Perk.PerkNames.IncreasedBlastRadius))
        {
            IncreasedBlastRadius = new Perk(myPerks[Perk.PerkNames.IncreasedBlastRadius], racer.racerData.blastRadiusMultiplier);
            perks.Add(IncreasedBlastRadius);
        }
        if (perkList.perkNames.Contains(Perk.PerkNames.BurstSpeed))
        {
            BurstSpeed = new Perk(myPerks[Perk.PerkNames.BurstSpeed], racer.racerData.burstSpeed);
            perks.Add(BurstSpeed);
        }
        if (perkList.perkNames.Contains(Perk.PerkNames.ContactDamage))
        {
            ContactDamage = new Perk(myPerks[Perk.PerkNames.ContactDamage], racer.racerData.contactDamage);
            perks.Add(ContactDamage);
        }
        if (perkList.perkNames.Contains(Perk.PerkNames.IncreasedDamage))
        {
            IncreasedDamage = new Perk(myPerks[Perk.PerkNames.IncreasedDamage], racer.racerData.contactDamage);
            perks.Add(IncreasedDamage);
        }
        if (perkList.perkNames.Contains(Perk.PerkNames.IncreasedJumpVelocity))
        {
            IncreasedJumpVelocity = new Perk(myPerks[Perk.PerkNames.IncreasedJumpVelocity], racer.racerData.maxJumpVelocity);
            perks.Add(IncreasedJumpVelocity);
        }
        if (perkList.perkNames.Contains(Perk.PerkNames.IncreasedJumpCount))
        {
            IncreasedJumpCount = new Perk(myPerks[Perk.PerkNames.IncreasedJumpCount], racer.racerData.amountOfJumps);
            perks.Add(IncreasedJumpCount);
        }
        if (perkList.perkNames.Contains(Perk.PerkNames.StrengthIncreaseRateBuff))
        {
            StrengthIncreaseRateBuff = new Perk(myPerks[Perk.PerkNames.StrengthIncreaseRateBuff], racer.racerData.strengthIncreaseRate);
            perks.Add(StrengthIncreaseRateBuff);
        }

        ActivateGeneralPerks();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var perk in perks)
            perk.SetPerkActivity(racer.isInNativeMap, racer.isAwakened);

        racerDamages.damageResistance = DamageResistance.HandleActivePerk(racerDamages.damageResistance);
        racer.moveVelocityResource = IncreasedSpeed.HandleActivePerk(racer.moveVelocityResource);
        racerDamages.blastRadiusMultiplier = IncreasedBlastRadius.HandleActivePerk(racerDamages.blastRadiusMultiplier);
        racer.burstSpeed = BurstSpeed.HandleActivePerk(racer.burstSpeed);
        racerDamages.contactDamage = ContactDamage.HandleActivePerk(racerDamages.contactDamage);
        racer.jumpVelocity = IncreasedJumpVelocity.HandleActivePerk(racer.jumpVelocity);
        racer.amountOfJumps = IncreasedJumpCount.HandleActivePerk(racer.amountOfJumps);
    }

    private void ActivateGeneralPerks()
    {
        foreach (var perk in perks)
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

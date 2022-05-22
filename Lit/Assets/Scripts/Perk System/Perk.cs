using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that holds information and logic for perks on racers.
/// </summary>
[System.Serializable]
public class Perk
{
    public PerkNames perkName;
    public PerkData perkData;
    public dynamic valueToAffect;
    public float perkValue;
    public float perkChance;
    private dynamic originalValue;

    public bool isPerkActive;

    public PerkData.PerkState perkState;

    private List<PerkData.PerkState> perkCallTracker;
    public Perk(PerkData perkData, dynamic originalValue)
    {
        this.perkData = perkData;
        isPerkActive = false;

        perkValue = perkData.perkValue;
        perkChance = perkData.perkChance;

        this.originalValue = originalValue;
        perkCallTracker = new List<PerkData.PerkState>();
    }

    public void ActivatePerk()
    {
        isPerkActive = true;
    }
    public void ApplyPerk(dynamic valueToAffect)
    {
        RunPerkAction(valueToAffect);
    }
    /// <summary>
    /// Calls the perk action, if the perk is active. A 'Once' call type perk calls one time and a 'Continuous' type calls continuously, with a given count.
    /// </summary>
    /// <param name="valueToAffect">The value of the object assigned.</param>
    /// <param name="callCount">The call count. In we use a continuous call perk. Use 'Mathf.Infinity' if you want a perk to call infinitely.</param>
    /// <returns></returns>
    public dynamic HandleActivePerk(dynamic valueToAffect, int callCount = 1)
    {
        if(perkData == null) { return valueToAffect; }
        dynamic value = valueToAffect;
        switch (perkData.perkCallRate)
        {
            case PerkData.PerkCallRate.Once:
                value = RunPerkAction(valueToAffect);
                isPerkActive = false;
                break;
            case PerkData.PerkCallRate.Continuous:
                for (int i = 0; i < callCount; i++)
                    value = RunPerkAction(valueToAffect);
                break;
        }
        return value;
    }

    public dynamic RemovePerk(dynamic valueToAffect)
    {
        dynamic value = ReversePerkAction(valueToAffect);
        isPerkActive = false;

        return value;
    }
    private dynamic RunPerkAction(dynamic valueToAffect)
    {
        if (!isPerkActive) { return valueToAffect; }
        CheckPerkState();

        dynamic value = valueToAffect;

        switch (perkData.perkType)
        {
            case PerkData.PerkType.numeric:
                if (!CheckPerkChance()) { return valueToAffect; }
                perkCallTracker.Add(perkState);
                switch (perkData.perkNature)
                {
                    case PerkData.PerkNature.Positive:
                        value += Utils.PercentageValue(originalValue, perkValue);
                        break;
                    case PerkData.PerkNature.Negative:
                        value -= Utils.PercentageValue(originalValue, perkValue);
                        break;
                }
                break;
            case PerkData.PerkType.boolean:
                if (CheckPerkChance())
                {
                    if (perkData.perkValue == 0)
                        value = false;
                    else if (perkData.perkValue == 1)
                        value = true;
                    else
                        Debug.LogError("Perk Value is neither 0 or 1 for a boolean type perk. Set value to be either 0 or 1.");
                }
                else
                {
                    if (perkData.perkValue == 0)
                        value = true;
                    else if (perkData.perkValue == 1)
                        value = false;
                    else
                        Debug.LogError("Perk Value is neither 0 or 1 for a boolean type perk. Set value to be either 0 or 1.");
                }
                break;
            case PerkData.PerkType.Percentage:
                if (!CheckPerkChance()) { return valueToAffect; }
                perkCallTracker.Add(perkState);
                switch (perkData.perkNature)
                {
                    case PerkData.PerkNature.Positive:
                        value += perkValue;
                        break;
                    case PerkData.PerkNature.Negative:
                        value -= perkValue;
                        value = Mathf.Max(0, value);
                        break;
                }
                break;
        }
        return value;
    }
    private dynamic ReversePerkAction(dynamic valueToAffect)
    {
        dynamic value = valueToAffect;

        switch (perkData.perkType)
        {
            case PerkData.PerkType.numeric:
                foreach (var state in perkCallTracker)
                {
                    switch (perkData.perkNature)
                    {
                        case PerkData.PerkNature.Positive:
                            value -= Utils.PercentageValue(originalValue, GetPerkValue(state));
                            break;
                        case PerkData.PerkNature.Negative:
                            value += Utils.PercentageValue(originalValue, GetPerkValue(state));
                            break;
                    }
                }
                break;
            case PerkData.PerkType.boolean:
                value = originalValue;
                break;
            case PerkData.PerkType.Percentage:
                foreach (var state in perkCallTracker)
                {
                    switch (perkData.perkNature)
                    {
                        case PerkData.PerkNature.Positive:
                            value -= GetPerkValue(state);
                            value = Mathf.Max(0, value);
                            break;
                        case PerkData.PerkNature.Negative:
                            value += GetPerkValue(state);
                            break;
                    }
                }
                break; 
        }
        perkCallTracker.Clear();
        return value;
    }
    private void CheckPerkState()
    {
        switch (perkState)
        {
            case PerkData.PerkState.Base:
                perkValue = perkData.perkValue;
                perkChance = perkData.perkChance;
                break;
            case PerkData.PerkState.Hyper:
                perkValue = GetPerkValue(PerkData.PerkState.Hyper);
                perkChance = GetPerkChance(PerkData.PerkState.Hyper);
                break;
            case PerkData.PerkState.Ultimate:
                perkValue = GetPerkValue(PerkData.PerkState.Ultimate);
                perkChance = GetPerkChance(PerkData.PerkState.Ultimate);
                break;
        }
    }
    private float GetPerkValue(PerkData.PerkState perkState)
    {
        float value = 0;
        switch (perkState)
        {
            case PerkData.PerkState.Base:
                value = perkData.perkValue;
                break;
            case PerkData.PerkState.Hyper:
                value = Utils.PercentageIncreaseOrDecrease(perkData.perkValue, perkData.hyperMultiplier, Utils.AlterType.Increase);
                break;
            case PerkData.PerkState.Ultimate:
                value = Utils.PercentageIncreaseOrDecrease(perkData.perkValue, perkData.ultimateMultiplier, Utils.AlterType.Increase);
                break;
            default:
                break;
        }
        return value;
    }
    private float GetPerkChance(PerkData.PerkState perkState)
    {
        float value = 0;
        switch (perkState)
        {
            case PerkData.PerkState.Base:
                value = perkData.perkChance;
                break;
            case PerkData.PerkState.Hyper:
                value = Utils.PercentageIncreaseOrDecrease(perkData.perkChance, perkData.hyperChanceMultiplier, Utils.AlterType.Increase);
                break;
            case PerkData.PerkState.Ultimate:
                value = Utils.PercentageIncreaseOrDecrease(perkData.perkChance, perkData.ultimateChanceMultiplier, Utils.AlterType.Increase);
                break;
        }
        return value;
    }
    public bool SetPerkActivity(bool inNativeMap, bool isAwakened)
    {
        var formerPerkState = perkState;

        if (inNativeMap && isAwakened)
            perkState = PerkData.PerkState.Ultimate;
        else if (inNativeMap || isAwakened)
            perkState = PerkData.PerkState.Hyper;
        else
            perkState = PerkData.PerkState.Base;

        return formerPerkState != perkState;
    }
    public bool CheckPerkChance()
    {
        if (perkChance == 100) { return true; }
        else if (perkChance == 0) { return false; }
        else if (Utils.RandomValue(0, 100) <= perkChance) { return true; }
        else { return false; }
    }

    public enum PerkNames
    {
        DamageResistance,
        IncreasedSpeed,
        IncreasedBlastRadius,
        BurstSpeed,
        ContactDamage,
        IncreasedDamage,
        IncreasedJumpVelocity,
        IncreasedJumpCount,
        StrengthIncreaseRateBuff,
        BurnResistance,
        FreezeResistance,
        Mushin,
        HomingProjectile,
        ShieldAbsorption,
        ReducedSpeed,
        IncreasedGliderSpeed,
        HaltRacer,
        SabotageInput,
        IncreasedPowerupDuration,
    }
}


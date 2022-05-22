using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Logic holder for damage information for all runners.
/// </summary>
[System.Serializable]
public struct RunnerDamagesOperator
{
    public DamageForm fire;
    public DamageForm frost;
    public DamageForm poison;
    public DamageForm light;
    public DamageForm lightning;
    public DamageForm wind;
    public DamageForm magic;
    public DamageForm shadow;
    public DamageForm laser;
    public DamageForm deathLaser;
    public DamageForm knockout;

    public int activeDamageCount;

    public List<DamageForm> Damages;
    public void InitDamages()
    {
        fire = new DamageForm("fire", false, DamageForm.DamagerType.Fire);
        frost = new DamageForm("frost", false, DamageForm.DamagerType.Frost);
        poison = new DamageForm("poison", false, DamageForm.DamagerType.PoisonGas);
        light = new DamageForm("light", false, DamageForm.DamagerType.Light);
        lightning = new DamageForm("lightning", false, DamageForm.DamagerType.Lightning);
        wind = new DamageForm("wind", false, DamageForm.DamagerType.Wind);
        magic = new DamageForm("magic", false, DamageForm.DamagerType.Magic);
        shadow = new DamageForm("shadow", false, DamageForm.DamagerType.Shadow);
        laser = new DamageForm("laser", false, DamageForm.DamagerType.Laser);
        deathLaser = new DamageForm("deathLaser", false, DamageForm.DamagerType.DeathLaser);
        knockout = new DamageForm("knockout", false, DamageForm.DamagerType.knockout);

        Damages = new List<DamageForm>
        {
            shadow,
            fire,
            frost,
            poison,
            light,
            lightning,
            wind,
            magic,
            laser,
            deathLaser,
            knockout
        };
    }
    public void AddDamage(int damageInt, DamageForm damage)
    {
        Damages[damageInt].damageInt = damage.damageInt;
        Damages[damageInt].damageName = damage.damageName;
        Damages[damageInt].damagerType = damage.damagerType;
        Damages[damageInt].damagePercentage = damage.damagePercentage;
        Damages[damageInt].damageRate = damage.damageRate;
        Damages[damageInt].canResistDamage = damage.canResistDamage;
        Damages[damageInt].damaged = true;

        if (damage.racer != null)
        {
            Damages[damageInt].racer = damage.racer;
            Damages[damageInt].ultimateDamage = damage.racer.isAwakened && damage.racer.isInNativeMap;
            Damages[damageInt].hyperDamage = damage.racer.isAwakened || damage.racer.isInNativeMap;
        }
        activeDamageCount++;
    }
    public void RemoveAllDamages()
    {
        foreach (var damage in DamageList())
        {
            damage.damaged = false;
            damage.damageInt = 8;
            damage.damagePercentage = 0;
            damage.damageRate = 0;
            damage.hyperDamage = false;
            damage.ultimateDamage = false;
            damage.racer = null;
        }
        activeDamageCount = 0;
    }
    public bool SimilarDamages(RunnerDamagesOperator otherRunnerDamages)
    {
        foreach (var previousDamage in Damages)
        {
            var previousDamageIndex = Damages.IndexOf(previousDamage);
            if(Damages[previousDamageIndex].damaged && otherRunnerDamages.Damages[previousDamageIndex].damaged)
            {
                return true;
            }
            else
            {
                continue;
            }
        }
        return false;
    }
    public bool IsDamaged()
    {
        //    Damages = new[] { isBurning, isFrozen, isPoisoned, isBlinded, isElectrocuted, isBlownAway, isZapped };
        foreach (var damage in Damages)
        {
            if (damage.damaged == true)
            {
                return true;
            }
            else
            {
                continue;
            }
        }
        return false;
    }
    public List<DamageForm> DamageList()
    {
        //    Damages = new[] { isBurning, isFrozen, isPoisoned, isBlinded, isElectrocuted, isBlownAway, isZapped };
        var damagesHad = new List<DamageForm>();
        foreach (var damage in Damages)
        {
            if(damage.damaged == true)
            {
                damagesHad.Add(damage);
            }
            else
            {
                continue;
            }
        }
        return damagesHad;
    }
}

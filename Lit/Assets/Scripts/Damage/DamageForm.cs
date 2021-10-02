using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DamageForm 
{
    public DamageForm(string damageName, bool damaged, DamagerType damagerType, Racer racer = null, int damageInt = 8, float damageStrength = 0, bool hyperDamage = false, bool ultimateDamage = false)
    {
        this.damageName = damageName;
        this.damaged = damaged;
        this.damagerType = damagerType;
        this.damageInt = damageInt;
        this.racer = racer;
        this.damageStrength = damageStrength;
        this.hyperDamage = hyperDamage;
        this.ultimateDamage = ultimateDamage;
    }

    public string damageName;
    public DamagerType damagerType;

    public int damageInt;
    public bool damaged;
    public Racer racer;
    public float damageStrength;
    public bool hyperDamage;
    public bool ultimateDamage;

        public enum DamagerType
    {
        Laser,
        DeathLaser,
        Fire,
        Frost,
        PoisonGas,
        Light,
        Lightning,
        Wind,
        Magic,
        Shadow,
        knockout,
    }
}

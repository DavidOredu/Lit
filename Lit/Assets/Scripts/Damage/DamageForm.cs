using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DamageForm 
{
    public DamageForm(string damageName, bool damaged, DamagerType damagerType, float damageStrength = 0)
    {
        this.damageName = damageName;
        this.damaged = damaged;
        this.damagerType = damagerType;
        this.damageStrength = damageStrength;
    }

    public string damageName;
    public DamagerType damagerType;
    public bool damaged;
    public float damageStrength;

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

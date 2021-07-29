using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DamageForm 
{
    public DamageForm(string damageName, bool damaged, DamagerType damagerType, float damageTime = 0f, float negativeGravityScale = 0f)
    {
        this.damageName = damageName;
        this.damaged = damaged;
        this.damagerType = damagerType;
        this.damageTime = damageTime;
        this.negativeGravityScale = negativeGravityScale;
    }

    public string damageName;
    public DamagerType damagerType;
    public bool damaged;
    public float damageTime;
    public float negativeGravityScale;

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
    }
}

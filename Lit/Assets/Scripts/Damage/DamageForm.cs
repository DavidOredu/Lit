using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class DamageForm 
{
    public DamageForm(string damageName, bool damaged, DamagerType damagerType, Racer racer = null, int damageInt = 8, float damagePercentage = 0, float damageRate = 0, bool hyperDamage = false, bool ultimateDamage = false, bool canResistDamage = true)
    {
        this.damageName = damageName;
        this.damaged = damaged;
        this.damagerType = damagerType;
        this.damageInt = damageInt;
        this.racer = racer;
        this.damagePercentage = damagePercentage;
        this.damageRate = damageRate;
        this.hyperDamage = hyperDamage;
        this.ultimateDamage = ultimateDamage;
        this.canResistDamage = canResistDamage;
    }
    /// <summary>
    /// The name of the damge as a string.
    /// </summary>
    public string damageName;
    /// <summary>
    /// The damage type using the enum "damage type".
    /// </summary>
    public DamagerType damagerType;
    /// <summary>
    /// The damage color code relating to runner colors and laser color codes.
    /// </summary>
    public int damageInt;
    /// <summary>
    /// Is the damage active?
    /// </summary>
    public bool damaged;
    /// <summary>
    /// The racer that caused the damage. Returns null if a racer did not cause a damage.
    /// </summary>
    public Racer racer;
    /// <summary>
    /// The rate at which the runner's strength reduces.
    /// </summary>
    public float damageRate;
    /// <summary>
    /// The percentage amount of dealt damage, relating to the maximum strength of the runner being damaged. This is a normalized value (i.e from 0 to 1).
    /// </summary>
    public float damagePercentage;
    /// <summary>
    /// Is the racer doing damage in an awakened state?
    /// </summary>
    public bool hyperDamage;
    /// <summary>
    /// Is the racer doing damage in an awakened state and in his home map?
    /// </summary>
    public bool ultimateDamage;
    /// <summary>
    /// Is the current damage resistable to a degree by the affected racer?
    /// </summary>
    public bool canResistDamage;

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

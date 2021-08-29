using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPowerupData", menuName = "Data/Powerup Data")]
public class PowerupData : ScriptableObject
{
    [Header("SPEED BOOST")]
    public float speedUpValue = 30f;

    [Header("SHIELD")]
    public float shieldStartLifetime = 7f;
    public float shieldParticleDuration = 7f;

    [Header("PROJECTILE")]
    public float projectileSpeed = 45f;
    public int projectileDamageStrength = 6;

    [Header("BEAM")]
    public int beamDamageStrength = 6;
}

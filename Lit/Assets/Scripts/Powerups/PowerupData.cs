using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewPowerupData", menuName = "Data/Powerup Data")]
public class PowerupData : ScriptableObject
{
    [Header("SPAWNING")]
    public AnimationCurve ammoCurve;

    [Header("SPEED BOOST")]
    public float speedUpPercentageIncrease = .5f;

    [Header("ELEMENT FIELD")]
    public float fieldDamagePercentage = .6f;
    public float fieldDamageRate = .6f;
    public float fieldExplosiveForce = 50f;
    public ForceMode2D fieldForceMode = ForceMode2D.Force;

    [Header("PROJECTILE")]
    public float projectileSpeed = 45f;
    public float projectileDamagePercentage = .6f;
    public float projectileDamageRate = .6f;

    [Header("MINE")]
    public float mineDamagePercentage = .6f;
    public float mineDamageRate = .6f;
    public float mineExplosiveRadiusDecreasePercentage = 0.5f;
    public float mineExplosiveForce = 200f;
    public float mineExplosiveRadius = 12f;
    public float mineUpwardsModifier = 1f;
    public ForceMode2D mineForceMode = ForceMode2D.Impulse;

    [Header("BEAM")]
    public float beamDamagePercentage = .6f;
    public float beamDamageRate = .6f;

    [Header("BOMB")]
    public float bombThrowForce = 3f;
    public float bombDamagePercentage = .9f;
    public float bombDamageRate = .9f;
    public float bombExplosiveForce = 200f;
    public float bombExplosiveRadius = 12f;
    public float bombUpwardsModifier = 1f;
    public ForceMode2D bombForceMode = ForceMode2D.Impulse;
}

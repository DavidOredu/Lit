using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDifficultyData", menuName = "Data/Difficulty Data")]
public class D_DifficultyData : RacerData
{
    [Header("OPPONENT STRENGTH")]
    public int maxStrength = 20;

    [Header("ANIMATION CURVES")]
    public AnimationCurve strengthToTopSpeedCurve;
    public AnimationCurve speedToJumpVelocityCurve;

    [Header("MOVE STATE")]
    public float topSpeed = 3f;
    public float timeZeroToMax = 3f;
    public float timeMaxToZero = 4f;
    public float timeBrakeToZero = 1f;
    public float maxJumpVelocity = 10;

    [Header("IDLE STATE")]
    public float minIdleTime = .5f;
    public float maxIdleTime = 3f;

    [Header("DAMAGE KNOCKDOWN STATE")]
    public float knockoutTime = 4f;
    public float ultimateDamageKnockoutTime = 10f;

    [Header("REVIVED STATE")]
    public float invulnerabilityTimer = 5f;

    [Header("ACTION STATE")]
    public float actionForce = 20f;

    [Header("AWAKENED STATE")]
    public int requiredLitPlatformsToAwaken = 5;
    public int awakenCount = 1;

    [Header("LIT VARIABLES")]
    public float jumpAddition;
    public float litSpeedUpPercentage = .5f;
    public float litSlowDownPercentage = .3f;
    public float overdriveIncreasePercentage = 1.5f;
    public float otherOnLitIncreasePercentage = .5f;

    [Header("KNOCKBACK VARIABLES")]
    public Vector2 knockbackVelocity;
    public Vector2 maxKnockbackVelocity;

    [Header("CHECK VARIABLES")]
    public float playerDefenseCheckRadius = .5f;
    public float playerAttackCheckRadius = .5f;
    public float groundCheckRadius = 0.3f;
    public float higherPlatformCheckDistance = .6f;
    public float ledgeCheckDistance = .5f;
    public float obstacleCheckRadius = .8f;
    public float projectileCheckRadius = .8f;
    public float jumpCheckRadius = .5f;
    public float litCheckDistance = .5f;
    public float powerCheckDistance = .5f;
    public Vector2 higherPlatformCheckDirection = Vector2.up;
    public Vector2 ledgeCheckDirection = Vector2.down;
    public Vector2 powerCheckDirection = Vector2.down;
    public LayerMask whatIsGround;
    public LayerMask whatIsLit;
    public LayerMask whatIsPower;
    public LayerMask whatToJumpTo;
    public LayerMask whatIsPlayer;
    public LayerMask whatIsObstacle;
    public LayerMask whatIsProjectile;
    public LayerMask whatIsSlope;

    [Header("POWERUP VARIABLES")]
    public float powerupRadius = .5f;
    public LayerMask whatToDamage;

    [Header("PROBABILITIES")]
    public AnimationCurve jumpToLitProbabilityCurve;
    public AnimationCurve jumpToLitIfLitProbabilityCurve;
    public AnimationCurve jumpToLitIfDarkProbabilityCurve;
    public AnimationCurve jumpToPowerProbabilityCurve;
    public AnimationCurve jumpToBaseProbabilityCurve;
    public AnimationCurve boosterPowerPlatformProbabilityCurve;
    public AnimationCurve defensivePowerPlatformProbabilityCurve;
}

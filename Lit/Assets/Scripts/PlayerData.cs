﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : RacerData
{
    /// <summary>
    /// The name of the runner. This name will be used in dialogue in the game's story, and will be used in the multiplayer section the discern players.
    /// </summary>
    [Header("PROFILE")]
    public string playerName;
    /// <summary>
    /// The maximum strength of the runner. The strength variable is used to resist damage by runner's and obstacles. It also determines the top speed of the runner.
    /// </summary>
    [Header("PLAYER STRENGTH")]
    public int maxStrength = 20;

    /// <summary>
    /// The relationship between strength and top speed. This governs the dynamic ratios strength has to top speed.
    /// </summary>
    [Header("ANIMATION CURVES")]
    public AnimationCurve strengthToTopSpeedCurve;
    /// <summary>
    /// [Deprecated] The relationship between speed and jump. This used to measure to jump amount a runner should have depending on speed, but I found the game gets less fluid that way.
    /// </summary>
    public AnimationCurve speedToJumpVelocityCurve;

    /// <summary>
    /// The runner's top speed. This determines the highest speed that can be achieved by a runner.
    /// </summary>
    [Header("MOVE STATE")]
    public float topSpeed = 24;
    /// <summary>
    /// The acceleration rate of the player. How much time will it take to reach the top speed from 0?
    /// </summary>
    public float timeZeroToMax = 2.5f;
    /// <summary>
    /// A slower deceleration rate of the player. How much time will it take to reach the 0 from the current speed?
    /// </summary>
    public float timeMaxToZero = 4f;
    /// <summary>
    /// A faster deceleration rate of the player. How much time will it take to reach the 0 from the current speed?
    /// </summary>
    public float timeBrakeToZero = 1f;

    /// <summary>
    /// The highest jump velocity possible.
    /// </summary>
    [Header("JUMP STATE")]
    public float maxJumpVelocity = 10;
    /// <summary>
    /// How many jumps can the runner reach?
    /// </summary>
    public int amountOfJumps = 1;

    /// <summary>
    /// The added force obtained by using the action platform.
    /// </summary>
    [Header("ACTION STATE")]
    public float actionForce = 20f;

    [Header("IN AIR STATE")]
    public float coyoteTime = 0.2f;
    public float variableJumpHeightMultiplier = 0.5f;

    [Header("DAMAGE KNOCKDOWN STATE")]
    public float knockoutTime = 4f;
    public float ultimateDamageKnockoutTime = 10f;

    [Header("REVIVED STATE")]
    public float invulnerabilityTimer = 5f;

    [Header("AWAKENED STATE")]
    public int requiredLitPlatformsToAwaken = 5;
    public int awakenCount = 1;

    //  [Header("Land State")]
    //  public bool spawnDust;

    [Header("LIT VARIABLES")]
    public float litSpeedUpPercentage = .5f;
    public float litSlowDownPercentage = .3f;
    public float jumpAddition;
    public float overdriveIncreasePercentage = 1.5f;
    public float otherOnLitIncreasePercentage = .5f;

    [Header("CHECK VARIABLES")]
    public float groundCheckRadius = 0.3f;
    public float litCheckDistance = .3f;
    public float powerCheckDistance = .5f;
    public LayerMask whatIsGround;
    public LayerMask whatIsLit;
    public LayerMask whatIsPower;
    public LayerMask whatIsSlope;

    [Header("KNOCKBACK VARIABLES")]
    public Vector2 knockbackVelocity;
    public Vector2 maxKnockbackVelocity;

    [Header("POWERUP VARIABLES")]
    public float powerupRadius = .5f;
    public LayerMask whatToDamage;

    [Header("COLOR VARIABLES")]
    [Range(0, 10)]
    public int colorCode;

}

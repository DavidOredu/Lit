using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("PROFILE")]
    public string playerName;

    [Header("PLAYER STRENGTH")]
    public int maxStrength = 20;
    public AnimationCurve strengthToTopSpeedCurve;
    public AnimationCurve speedToJumpVelocityCurve;

    [Header("MOVE STATE")]
    public float topSpeed = 24;
    public float timeZeroToMax = 2.5f;
    public float timeMaxToZero = 4f;
    public float timeBrakeToZero = 1f;

    [Header("JUMP STATE")]
    public float maxJumpVelocity = 15;
    public int amountOfJumps = 1;

    [Header("ACTION STATE")]
    public float actionForce = 20f;

    [Header("IN AIR STATE")]
    public float coyoteTime = 0.2f;
    public float variableJumpHeightMultuplier = 0.5f;

    [Header("STUN STATE")]
    public float laserStunTime = 4f;
    public float wallStunTime = 3f;

    [Header("REVIVED STATE")]
    public float invulnerabilityTimer = 5f;

    [Header("AWAKENED STATE")]
    public int requiredLitPlatformsToAwaken = 5;
    public int awakenCount = 1;

    //  [Header("Land State")]
    //  public bool spawnDust;

    [Header("LIT VARIABLES")]
    public float litSpeedUpLimit;
    public float litSlowDownLimit;
    public float jumpAddition;
    public float otherOnLitIncreaseValue;
    public float litSpeedAlterRate = 6f;

    [Header("CHECK VARIABLES")]
    public float groundCheckRadius = 0.3f;
    public float litCheckDistance = .3f;
    public float powerCheckDistance = .5f;
    public LayerMask whatIsGround;
    public LayerMask whatIsLit;
    public LayerMask whatIsPower;

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

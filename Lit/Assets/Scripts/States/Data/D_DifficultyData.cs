using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDifficultyData", menuName = "Data/Difficulty Data")]
public class D_DifficultyData : ScriptableObject
{
    [Header("MOVE STATE")]
    public float topSpeed = 3f;
    public float timeZeroToMax = 3f;
    public float timeMaxToZero = 4f;
    public float timeBrakeToZero = 1f;
    public float maxJumpVelocity = 24;

    [Header("IDLE STATE")]
    public float minIdleTime = .5f;
    public float maxIdleTime = 3f;

    [Header("STUN STATE")]
    public float laserStunTime = 4f;
    public float wallStunTime = 3f;

    [Header("LIT VARIABLES")]
    public float jumpAddition;
    public float litSpeedUpLimit;
    public float litSlowDownLimit;
    public float otherOnLitIncreaseValue;
    public float resetSpeedTime;

    [Header("KNOCKBACK VARIABLES")]
    public Vector2 knockbackVelocity;
    public Vector2 maxKnockbackVelocity;

    [Header("CHECK VARIABLES")]
    public float groundCheckRadius = 0.3f;
    public float jumpCheckRadius = .3f;
    public float litCheckDistance = .5f;
    public float powerCheckDistance = .5f;
    public LayerMask whatIsGround;
    public LayerMask whatIsLit;
    public LayerMask whatIsPower;
    public LayerMask whatToJumpTo;

    [Header("TRANSFORM VARIABLES")]
    public Vector2 jumpCheckPosition;

    [Header("POWERUP VARIABLES")]
    public float powerupRadius = .5f;
    public LayerMask whatToDamage;
}

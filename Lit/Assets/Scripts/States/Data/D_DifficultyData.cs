using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newDifficultyData", menuName = "Data/Difficulty Data")]
public class D_DifficultyData : RacerData
{
    [Header("AI VARIABLES")]
    public float reactionTime = 4f;

    [Header("CHECK VARIABLES")]
    public float playerDefenseCheckRadius = .5f;
    public float playerAttackCheckRadius = .5f;
    public float higherPlatformCheckDistance = .6f;
    public float ledgeCheckDistance = .5f;
    public float obstacleCheckRadius = .8f;
    public float projectileCheckRadius = .8f;
    public float jumpCheckRadius = .5f;
    public Vector2 higherPlatformCheckDirection = Vector2.up;
    public Vector2 ledgeCheckDirection = Vector2.down;
    public Vector2 powerCheckDirection = Vector2.down;
    public LayerMask whatToJumpTo;
    public LayerMask whatIsPlayer;
    public LayerMask whatIsObstacle;
    public LayerMask whatIsProjectile;

    [Header("PROBABILITIES")]
    public AnimationCurve jumpToLitProbabilityCurve;
    public AnimationCurve jumpToLitIfLitProbabilityCurve;
    public AnimationCurve jumpToLitIfDarkProbabilityCurve;
    public AnimationCurve jumpToPowerProbabilityCurve;
    public AnimationCurve jumpToBaseProbabilityCurve;
    public AnimationCurve boosterPowerPlatformProbabilityCurve;
    public AnimationCurve defensivePowerPlatformProbabilityCurve;
}

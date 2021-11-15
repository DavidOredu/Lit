using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "AwakenedData", menuName = "Data/Awakened Data")]
public class AwakenedData : ScriptableObject
{
    [Header("RED")]
    public float redExplosiveRadiusPercentageIncrease = 0.5f;
    public float redDamageStrength;
    public float redSpeedIncreasePercentage = 2f;

    [Header("BLUE")]
    public float blueSpeedIncreasePercentage = 1.5f;
    public float blueOppositionSpeedReductionValue;

    [Header("YELLOW")]
    public GameObject yellowTrailEffect;
    public float yellowSpeedIncreasePercentage;
    public float yellowTeleportationPositionXPercentage = 1f;

    [Header("PURPLE")]
    public float purpleOrbSpeed = 40f;
    public float purpleOrbRadius = 40f;
    public float purpleOrbLifeTime = 7f;
    public float purpleBeamDamageStrength = 10f;
    public LayerMask purpleWhatToHit;

    [Header("CYAN")]
    public float cyanExplosiveForcePercentageIncrease = .6f;
    public float cyanExplosiveRadiusPercentageDecrease = .4f;
}

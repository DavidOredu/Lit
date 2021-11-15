using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewObstacleData", menuName = "Data/Obstacle Data")]
public class ObstacleData : ScriptableObject
{
    [Header("BREAKABLE OBSTACLES")]
    public float speedReductionPercentage = .1f;

    [Header("LASER")]
    public float laserDamagePercentage = .1f;
    public float laserDamageRate;

}

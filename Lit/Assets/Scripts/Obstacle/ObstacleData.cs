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
    public float laserDamageRate = .5f;
    public float timeToStartLaser = 2f;
    public float laserLifeTime = 3f;

    [Header("FINAL WALL")]
    public float finalWallLaserStartTime = 5f;

    [Header("TRIGGERED LASER")]
    public float triggeredLaserStartTime = 3f;
}

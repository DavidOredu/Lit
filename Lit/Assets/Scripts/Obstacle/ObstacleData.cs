using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewObstacleData", menuName = "Data/Obstacle Data")]
public class ObstacleData : ScriptableObject
{
    [Header("BREAKABLE OBSTACLES")]
    public float speedReduction;

    [Header("LASER")]
    public int laserDamageStrength;

}

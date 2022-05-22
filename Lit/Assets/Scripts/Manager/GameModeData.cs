using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameModeData", menuName = "Data/Game Mode Data")]
public class GameModeData : ScriptableObject
{
    [Header("GENERAL")]
    public float logicUpdateTime = 0.016f;
    [Header("CLASSIC DEATHMATCH")]
    public GameObject deathLaserPrefab;
    public Vector3 deathLaserSpawnPoint;
    public AnimationCurve deathLaserVelocityCurve;
    public AnimationCurve deathLaserYFollowCurve;
}

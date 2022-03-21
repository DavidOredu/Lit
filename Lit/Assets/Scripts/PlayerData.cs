using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : RacerData
{ 
    [Header("IN AIR STATE")]
    public float coyoteTime = 0.2f;
    public float variableJumpHeightMultiplier = 0.5f;

    [Header("COLOR VARIABLES")]
    [Range(0, 10)]
    public int colorCode;
}

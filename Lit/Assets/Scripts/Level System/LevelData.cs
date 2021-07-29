using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevelData", menuName = "Data/LevelData")]
public class LevelData : ScriptableObject
{
    [Header("LEVEL")]
    // current level of the player
    public int level = 1;
    // experience obtained throughout the game
    [Header("XPs")]
    public int experience = 0;
    // quantity used to upgrade player stats
    public int XP = 0;

    // attribute value used to set the progress bar value, the value of the current experience foreach attribute
    [Header("ATTRIBUTE EXPERIENCES")]
    public int topSpeedExperience = 0;
    public int accelerationValueExperience = 0;
    public int strengthValueExperience = 0;
    public int litAbilityValueExperience = 0;
}

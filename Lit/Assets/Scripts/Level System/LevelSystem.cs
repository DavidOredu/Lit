using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LevelSystem
{
    private int level;
    private int experience;
    
    private LevelData levelData;

    private PlayerAttribute topSpeedAttribute;
    private PlayerAttribute accelerationAttribute;
    private PlayerAttribute strengthAttribute;
    private PlayerAttribute litAbilityAttribute;

   [SerializeField] public List<PlayerAttribute> playerAttributes = new List<PlayerAttribute>();


    public event EventHandler OnExperienceChanged;
    public event EventHandler OnLevelChanged;

    public LevelSystem(LevelData levelData)
    {
        this.levelData = levelData;
        level = this.levelData.level;

        topSpeedAttribute = new PlayerAttribute(levelData.topSpeedExperience, RankNames.experienceToNextLevel[level - 1]);
        accelerationAttribute = new PlayerAttribute(levelData.accelerationValueExperience, RankNames.experienceToNextLevel[level - 1]);
        strengthAttribute = new PlayerAttribute(levelData.strengthValueExperience, RankNames.experienceToNextLevel[level - 1]);
        litAbilityAttribute = new PlayerAttribute(levelData.litAbilityValueExperience, RankNames.experienceToNextLevel[level - 1]);

        playerAttributes.Add(topSpeedAttribute);
        playerAttributes.Add(accelerationAttribute);
        playerAttributes.Add(strengthAttribute);
        playerAttributes.Add(litAbilityAttribute);
    }
    public void Update()
    {
        foreach (var attribute in playerAttributes)
        {
            attribute.Update();
        }
    }
    public void AddExperienceInLevelData(int amount)
    {
        levelData.experience += amount;
        levelData.XP += amount;
    }
    public void LevelUp()
    {
        if (playerAttributes[0].isFull && playerAttributes[1].isFull && playerAttributes[2].isFull && playerAttributes[3].isFull && !IsMaxLevel())
        {
            level++;
            if (!IsMaxLevel())
            {
                for (int i = 0; i < playerAttributes.Count; i++)
                {
                    playerAttributes[i].experience = 0;
                    playerAttributes[i].experienceToNextLevel = RankNames.experienceToNextLevel[level - 1];
                    playerAttributes[i].isFull = false;
                }
            }
            levelData.topSpeedExperience = topSpeedAttribute.experience;
            levelData.accelerationValueExperience = accelerationAttribute.experience;
            levelData.strengthValueExperience = strengthAttribute.experience;
            levelData.litAbilityValueExperience = litAbilityAttribute.experience;
            levelData.level = level;
            OnLevelChanged?.Invoke(this, EventArgs.Empty);
        }
       
    }
    public void AddExperience(PlayerAttribute playerAttribute, int amount) 
    {
        if (!IsMaxLevel())
        {
            playerAttribute.experience += amount;
               
            
            //    levelData.experience = experience;
            levelData.topSpeedExperience = topSpeedAttribute.experience;
            levelData.accelerationValueExperience = accelerationAttribute.experience;
            levelData.strengthValueExperience = strengthAttribute.experience;
            levelData.litAbilityValueExperience = litAbilityAttribute.experience;
            OnExperienceChanged?.Invoke(this, EventArgs.Empty);
            Debug.Log("Has invoked on experience changed in level system");
        }
       
    }
    public int GetLevelNumber()
    {
        return level;
    }
    public int GetExperience(PlayerAttribute playerAttribute)
    {
        return playerAttribute.experience;
    }
    public int GetValueOfLevel(int level)
    {
        if(level <= RankNames.numbersOfUpgradesForLevel.Length + 1)
        {
            return RankNames.numbersOfUpgradesForLevel[level - 1];
        }
        else
        {
            //Level invalid
            Debug.LogError("Level Invalid: " + level);
            return 100;
        }
    }
    public int GetExperienceToNextLevel(int level)
    {
        if (level <= RankNames.experienceToNextLevel.Length + 1)
        {
            return RankNames.experienceToNextLevel[level - 1];
        }
        else
        {
            //Level invalid
            Debug.LogError("Level Invalid: " + level);
            return 100;
        }
    }
    public bool IsMaxLevel()
    {
      return IsMaxLevel(level);
    }
    public bool IsMaxLevel(int level)
    {
        return level == RankNames.experienceToNextLevel.Length;
    }
    
    public float GetAttributeNormalized(PlayerAttribute playerAttribute)
    {
        if (IsMaxLevel())
        {
            return 1f;
        }
        else
        {
            return playerAttribute.normalizedValue;
        }
    }
    public float GetExperienceNormalized()
    {
        if (IsMaxLevel())
        {
            return 1f;
        }
        else
        {
            return (float)experience / GetExperienceToNextLevel(level);
        }
    }
}

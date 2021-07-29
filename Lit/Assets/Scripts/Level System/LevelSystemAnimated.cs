using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[Serializable]
public class LevelSystemAnimated
{
    private int level;
    public int amountOfBuyingXP;


    //  private int experienceToNextLevel;

    public event EventHandler OnExperienceChanged;
    public event EventHandler OnLevelChanged;

    private LevelSystem levelSystem;
    public bool isAnimating;

    private float updateTimer;
    private float updateTimerMax;

    public List<int> playerAttributesExperiences = new List<int>();
    public LevelSystemAnimated(LevelSystem levelSystem)
    {
        SetLevelSystem(levelSystem);
        updateTimerMax = .016f;

    }
    public void Update()
    {
        if (isAnimating)
        {
            //Check if its time to update
            updateTimer += Time.deltaTime;
            while (updateTimer >= updateTimerMax)
            {
                //time to update
                updateTimer -= updateTimerMax;

                UpdateAddExperience();
            }
        }
        for (int i = 0; i < playerAttributesExperiences.Count; i++)
        {
            Debug.Log(level + " " + playerAttributesExperiences[i]);
        }
    }
    private void UpdateAddExperience()
    {
        //if (level < levelSystem.GetLevelNumber())
        //{
        // //   AddExperience();
        //}
        //if
        //{
            //if(playerAttributesExperiences[0] < levelSystem.GetExperience(levelSystem.playerAttributes[0])) 
            //    {
            //    AddExperience(0); 
            //    }
            //else if (playerAttributesExperiences[1] < levelSystem.GetExperience(levelSystem.playerAttributes[1]))
            //{
            //    AddExperience(1);
            //}
            //else if (playerAttributesExperiences[2] < levelSystem.GetExperience(levelSystem.playerAttributes[2]))
            //{
            //    AddExperience(2);
            //}
            //else if (playerAttributesExperiences[3] < levelSystem.GetExperience(levelSystem.playerAttributes[3]))
            //{
            //    AddExperience(3);
            //}
            //else
            //{
            //    isAnimating = false;
            //}
            //local level equals the target level
            for (int i = 0; i < playerAttributesExperiences.Count; i++)
            {
                if (playerAttributesExperiences[i] < levelSystem.GetExperience(levelSystem.playerAttributes[i]))
                {
                    AddExperience(i);
                    
                }       
                if(i == 3 && playerAttributesExperiences[i] < levelSystem.GetExperience(levelSystem.playerAttributes[i]))
                {
                    return;
                }
                else if(i == 3 && !(playerAttributesExperiences[0] < levelSystem.GetExperience(levelSystem.playerAttributes[0])) && !(playerAttributesExperiences[1] < levelSystem.GetExperience(levelSystem.playerAttributes[1])) && !(playerAttributesExperiences[2] < levelSystem.GetExperience(levelSystem.playerAttributes[2])) && !(playerAttributesExperiences[3] < levelSystem.GetExperience(levelSystem.playerAttributes[3])))
                {
                    isAnimating = false;
                }
            }
     //   }
    }
 
    //public void AddExperience()
    //{
    //    if (playerAttributesExperiences[0] >= levelSystem.GetExperienceToNextLevel(level) && playerAttributesExperiences[1] >= levelSystem.GetExperienceToNextLevel(level) && playerAttributesExperiences[2] >= levelSystem.GetExperienceToNextLevel(level) && playerAttributesExperiences[3] >= levelSystem.GetExperienceToNextLevel(level) && !levelSystem.IsMaxLevel())
    //    {
    //        level++;
    //        for (int i = 0; i < playerAttributesExperiences.Count; i++)
    //        {
    //            playerAttributesExperiences[i] = 0;
    //        }

    //        OnLevelChanged?.Invoke(this, EventArgs.Empty);
    //        Debug.Log("has invoked level changed in level system animated");
    //    }

    //    OnExperienceChanged?.Invoke(this, EventArgs.Empty);
    //    Debug.Log("has invoked experience changed in level system animated");
    //}
    public void LevelUp()
    {
        if (playerAttributesExperiences[0] >= levelSystem.GetExperienceToNextLevel(level) && playerAttributesExperiences[1] >= levelSystem.GetExperienceToNextLevel(level) && playerAttributesExperiences[2] >= levelSystem.GetExperienceToNextLevel(level) && playerAttributesExperiences[3] >= levelSystem.GetExperienceToNextLevel(level) && !levelSystem.IsMaxLevel())
        {
            level++;
            for (int i = 0; i < playerAttributesExperiences.Count; i++)
            {
                playerAttributesExperiences[i] = 0;
            }
            amountOfBuyingXP = levelSystem.GetExperienceToNextLevel(level) / levelSystem.GetValueOfLevel(level);
            OnLevelChanged?.Invoke(this, EventArgs.Empty);
            Debug.Log("has invoked level changed in level system animated");
        }
        else if (levelSystem.IsMaxLevel())
        {
            level = 21;
            OnLevelChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    public void AddExperience(int x)
    {
        playerAttributesExperiences[x] += RankNames.sliderSpeedIncreaseRate[level - 1];
        

        OnExperienceChanged?.Invoke(this, EventArgs.Empty);
        Debug.Log("has invoked experience changed in level system animated");
    }
    public void SetLevelSystem(LevelSystem levelSystem)
    {
        this.levelSystem = levelSystem;

        level = levelSystem.GetLevelNumber();
        //       experience = levelSystem.GetExperience();
        amountOfBuyingXP = levelSystem.GetExperienceToNextLevel(level) / levelSystem.GetValueOfLevel(level);
        for (int i = 0; i < levelSystem.playerAttributes.Count; i++)
        {
            playerAttributesExperiences.Add(levelSystem.playerAttributes[i].experience);
        }
        
      //  experienceToNextLevel = levelSystem.GetExperienceToNextLevel();

        levelSystem.OnExperienceChanged += LevelSystem_OnExperienceChanged;
        levelSystem.OnLevelChanged += LevelSystem_OnLevelChanged;
    }

    private void LevelSystem_OnLevelChanged(object sender, EventArgs e)
    {
        isAnimating = true;
    }

    private void LevelSystem_OnExperienceChanged(object sender, EventArgs e)
    {
        isAnimating = true;
    }
    public int GetLevelNumber()
    {
        return level;
    }
    
    //public float GetExperienceNormalized()
    //{
    //    if (levelSystem.IsMaxLevel())
    //    {
    //        return 1f;
    //    }
    //    else
    //    {
    //        return (float)experience / levelSystem.GetExperienceToNextLevel(level);
    //    }
    //}
    public float GetAttributeNormalized(int experience)
    {
        if (levelSystem.IsMaxLevel())
        {
            return 1f;
        }
        else
        {
            return (float)experience / levelSystem.GetExperienceToNextLevel(level);
        }
    }
}

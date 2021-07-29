using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerAttribute
{
    // the normalized value of the attribute, used for setting the level of the bar
    [Range(0, 1)]
    public float normalizedValue;

    // the real value of experience
    public int experience;

    // bool to tell if the bar is full
    public bool isFull = false;

    // the general experience to next level required
    public int experienceToNextLevel;

    public PlayerAttribute(int experience, int experienceToNextLevel)
    {
  //      this.levelSystem = levelSystem;
        this.experience = experience;
        this.experienceToNextLevel = experienceToNextLevel;
        normalizedValue = (float)experience / experienceToNextLevel;
    }

    public void Update()
    {
        normalizedValue = (float)experience / experienceToNextLevel;
        if (normalizedValue == Mathf.Infinity)
            normalizedValue = 1;
        if (normalizedValue == 1)
            isFull = true;
    }
    
   
}

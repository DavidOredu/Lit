using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class RankNames
{
    private const string l1 = "Rookie";
    private const string l2 = " ";
    private const string l3 = " ";
    private const string l4 = " ";
    private const string l5 = " ";
    private const string l6 = " ";
    private const string l7 = " ";
    private const string l8 = " ";
    private const string l9 = " ";
    private const string l10 = " ";
    private const string l11 = " ";
    private const string l12 = " ";
    private const string l13 = " ";
    private const string l14 = " ";
    private const string l15 = " ";
    private const string l16 = " ";
    private const string l17 = " ";
    private const string l18 = " ";
    private const string l19 = " ";
    private const string l20 = "Pundit";
    private const string l21 = "KiNG Of Lit!";

    public static string[] Rank = new[] { l1, l2, l3, l4, l5, l6, l7, l8, l9, l10, l11, l12, l13, l14, l15, l16, l17, l18, l19, l20, l21 };
    // Used to debug. This experience to next level system is not standard...refer to lower part of code to use the preferred function for the job
    public static int[] experienceToNextLevel = new[] { 100,  400, 500, 1000, 1500, 1500, 1750, 1750, 2500, 2500, 3000, 3000, 3500, 4000, 4000, 4000, 4000, 6000, 7000, 38000, 0};
    public static int[] numbersOfUpgradesForLevel = new[] { 5, 5, 5, 5, 6, 6, 7, 7, 10 ,10, 10, 10, 10, 10, 10, 10, 10, 12, 14, 38, 1};
    public static int[] sliderSpeedIncreaseRate = new[] { 1, 2, 3, 4, 6, 6, 7, 7, 8, 8,  10, 10, 12, 15, 15, 15, 20, 24, 50, 50 };

    public static int GetTotalExperienceInGame()
    {
        int num = 0;
        foreach(var i in experienceToNextLevel)
        {
            num += i;
        }
        return num * 4;
    }
    public static int GetExperienceToNextLevel(int level)
    {
        //TODO: Formula to get experience to next level for current level goes here... for now we return an arbitrary value
        return 0;
    }
    public static int GetNumberOfUpgradesForLevel(int level)
    {
        // Returns the number of taps required to fill an XP bar. The value gotten further dividing the XP to next level gives the purchasing value of the XP
        return GetExperienceToNextLevel(level) / 20;
    }
    public static int GetSliderIncreaseRate(int level)
    {
        // Amount which XP bars increase by according to each level.This is used to scale the XP bar increase speed to the current level
        return GetExperienceToNextLevel(level) / 50;
    }
   

}

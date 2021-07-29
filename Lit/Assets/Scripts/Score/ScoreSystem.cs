using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Score system for all game modes found in Lit!
public class ScoreSystem
{
    public float currentScoreMultiplier;

    public enum ScoreType
    {
        ClassicDeathmatchScore,
        PowerBattleScore,
        DominationScore,
        QuickPlayAndArcadeScore,
        SurvivalScore
    }
    
    public static class ClassicDeathmatchScore
    {
        // Scores contained in a classic deathmatch game
        // Scoring in classic deathmatches should include the 1st place runner, xp score increase, the second and third places as runner-ups with their own individual prizes
        // therefore position scores
        public static int positionScore;
        // scores from lit platforms... unstable
        public static int litPlatformCount;
        // scores from others on lit... unstable
        public static int othersOnLitCount;
    }
    public static class PowerBattleScore
    {
        // Scores contained in a powerbattle game
        // power battle scoring mostly includes runners powerup-wise cases
        // for this, we need to count scores for amount of affected players by you... since the number of runners is fixed for every game, we'll just need to trigger a bool to give the color of the runner affected
        // and also, count scores for how many times a certain player is affected
        public static Dictionary<StickmanNet, int> runnerColorBoolsNet = new Dictionary<StickmanNet, int>();
    }
    public static class SurvivalScore
    {
        // Scores contained in a survival game
    }
    
    public static class DominationScore
    {
        //Scores contained in a domination game

        public static int position;
        public static int litPlatformCount;
        public static int othersOnLitPlatformCount;
        public static int powerupsEarned;
        public static int playersAffectedCount;

        public static int[] dominationScores = new[] { position, litPlatformCount, othersOnLitPlatformCount, powerupsEarned, playersAffectedCount };
        public static void ResetDominationScores()
        {
            position = 0;
            litPlatformCount = 0;
            othersOnLitPlatformCount = 0;
            powerupsEarned = 0;
            playersAffectedCount = 0;
        }
    }
    public static class QuickplayAndArcadeScore
    {
        // Scores contained in both quickplay and arcade game modes, since both are basically the same
        // No experience is gotten from quick play and arcade games... instead only coins and player position will be counted
        // count the player position, and give him the amount of coins issued accordingly
        public static int position;
    }
}

using DapperDino.Tutorials.Lobby;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Level
{
    [Header("MANUAL SET")]
    public GameModes.Modes levelGameMode;

    public int starsToUnlock;
    public int numberOfRounds;
    [Range(0, 8)]
    public int numberOfOpponentsInLevel;

    [Header("AUTOMATIC SET")]
    public int levelNumber;
    public Button.ButtonClickedEvent OnClick;
    [Range(0, 3)]
    public int stars;
    public int levelPos;
    public int buttonMap;

    public MapSet levelScene;

    public bool isUnlocked = false;

   
    public void ChangeGameMode()
    {
        GameManager.instance.currentGameMode = levelGameMode;
    }
    //public void Init()
    //{
    //    switch (levelGameMode)
    //    {
    //        case GameModes.Modes.idle:
    //            // No logic required
    //            break;
    //        case GameModes.Modes.quickPlay:
    //            scoreType = ScoreSystem.ScoreType.QuickPlayAndArcadeScore;
    //            break;
    //        case GameModes.Modes.classicDeathmatch:
    //            scoreType = ScoreSystem.ScoreType.ClassicDeathmatchScore;
    //            break;
    //        case GameModes.Modes.powerBattle:
    //            break;
    //        case GameModes.Modes.domination:
    //            break;
    //        case GameModes.Modes.survival:
    //            break;
    //        case GameModes.Modes.arcade:
    //            break;
    //        case GameModes.Modes.tutorial:
    //            break;
    //        default:
    //            break;
    //    }
    //}
    
}

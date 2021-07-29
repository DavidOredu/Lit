using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Testing : MonoBehaviour
{
    [SerializeField] private LevelWindow levelWindow;
    [SerializeField] private PlayerLobby player;

    private LevelData levelData;


    private LevelSystem levelSystem;
    private LevelSystemAnimated levelSystemAnimated;
    private void Awake()
    {
        levelData = Resources.Load<LevelData>("LitLevelData");

        levelSystem = new LevelSystem(levelData);
        levelWindow.SetLevelSystem(levelSystem);
        


        levelSystemAnimated = new LevelSystemAnimated(levelSystem);
        levelWindow.SetLevelSystemAnimated(levelSystemAnimated);
        player.SetLevelSystemAnimated(levelSystemAnimated);
    }

    private void FixedUpdate()
    {
        levelSystem.Update();
        levelSystemAnimated.Update();
    }
}

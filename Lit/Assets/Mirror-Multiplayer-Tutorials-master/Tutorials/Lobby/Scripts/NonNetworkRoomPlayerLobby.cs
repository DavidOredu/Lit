using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DapperDino.Mirror.Tutorials.Lobby;
using DapperDino.Tutorials.Lobby;
using System;

public class NonNetworkRoomPlayerLobby : RoomPlayerLobby
{
    private LevelManager levelManager;
    private MapHandler mapHandler;

    private void Awake()
    {
        levelManager = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<LevelManager>();
    }
    private void Start()
    {
        playerData = Resources.Load<PlayerData>("PlayerData");
        CmdSetDisplayName(playerData.playerName);

        startGameButton.interactable = true;
        Room.RoomPlayers.Add(this);

        if (!isLeader)
            roomPlayerType = Racer.RacerType.Opponent;
        else
            roomPlayerType = Racer.RacerType.Player;

        if (roomPlayerType == Racer.RacerType.Opponent && Room.freeColors.Count != 0)
        {
            currentColorCode = Room.freeColors[UnityEngine.Random.Range(0, Room.freeColors.Count)];
        }

        if (!isLeader)
            gameObject.SetActive(false);

        Room.RoomPlayers[0].currentColorCode = playerData.colorCode;
        var myIndex = Room.RoomPlayers.IndexOf(this);

        Room.freeColors.Remove(Room.RoomPlayers[myIndex].currentColorCode);


    }

    protected override void UpdateDisplay()
    {
        base.UpdateDisplay();

    }
    public void StartGame()
    {
        levelManager.selectedLevel.level.ChangeGameMode();
        mapHandler = new MapHandler(levelManager.selectedLevel.level.levelScene, levelManager.selectedLevel.level.numberOfRounds);
        
        Room.ServerChangeScene(mapHandler.StartMap(levelManager.selectedLevel.level.levelPos - 1));
   }
}

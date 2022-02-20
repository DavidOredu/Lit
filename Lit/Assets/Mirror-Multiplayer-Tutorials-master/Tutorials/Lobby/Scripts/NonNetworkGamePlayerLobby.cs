using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DapperDino.Mirror.Tutorials.Lobby;

public class NonNetworkGamePlayerLobby : GamePlayerLobby
{
    protected List<Opponent> opponentList = new List<Opponent>();
    
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    public override void OnStopClient()
    {
        base.OnStopClient();
    }
    [ContextMenu("UpdateDisplay")]
    public override void UpdateDisplay()
    {
        if(gamePlayerType == Racer.RacerType.Opponent) { return; }
        if (!isLocalPlayer)
        {
            foreach (var player in Room.GamePlayers)
            {
                if (player.isLocalPlayer && player.gamePlayerType == Racer.RacerType.Player)
                {
                    player.UpdateDisplay();
                    break;
                }
            }

            return;
        }
        GameManager.allStickmenColors.Reverse();
        for (int i = 0; i < Room.GamePlayers.Count; i++)
        {
            GameManager.allStickmenColors[i].code = Room.GamePlayers[i].colorCode;

            if (GameManager.allStickmenColors[i].GetComponent<Racer>().currentRacerType == Racer.RacerType.Opponent)
            {
                var opponent = GameManager.allStickmenColors[i].GetComponent<Opponent>();

                if (!opponentList.Contains(opponent))
                {
                    opponent.ChangeDifficulty(Room.GamePlayers[i].difficulty);
                    opponentList.Add(opponent);
                }
            }
        }
        GameManager.allStickmenColors.Reverse();
        base.UpdateDisplay();
    }
}

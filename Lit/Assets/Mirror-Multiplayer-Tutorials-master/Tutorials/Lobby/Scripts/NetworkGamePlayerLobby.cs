using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DapperDino.Mirror.Tutorials.Lobby;
public class NetworkGamePlayerLobby : GamePlayerLobby
{
    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    public override void UpdateDisplay()
    {
        if (!isLocalPlayer)
        {
            foreach (var player in Room.GamePlayers)
            {
                if (player.isLocalPlayer)
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
        }
        GameManager.allStickmenColors.Reverse();
        base.UpdateDisplay();
    }
}

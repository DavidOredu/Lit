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

    public override void Update()
    {
        base.Update();
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
        for (int i = 0; i < Room.GamePlayers.Count; i++)
        {
            players = new List<StickmanNet>(Room.players);
            players.Reverse();
            players[i].code = Room.GamePlayers[i].colorCode;


        }
        base.UpdateDisplay();
    }
}

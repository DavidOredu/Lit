using DapperDino.Mirror.Tutorials.Lobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPowerup : MonoBehaviour
{
    GamePlayerLobby gamePlayer;
    public PowerupBehaviour powerupBehaviour { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        gamePlayer = transform.root.gameObject.GetComponent<GamePlayerLobby>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UsePowerup()
    {
        if (gamePlayer == null)
            gamePlayer = transform.root.gameObject.GetComponent<GamePlayerLobby>();
        if(powerupBehaviour == null) { return; }
        if (!gamePlayer.racer.canUsePowerup) { return; }
        if (gamePlayer.powerup.isSelectable)
        {
            gamePlayer.powerup.SelectedStart(gamePlayer.racer);
        }
        else
        {
            powerupBehaviour.ActivatePowerup();
        }
        gamePlayer.powerup = null;
        powerupBehaviour = null;
        
    }
}

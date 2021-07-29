using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameModes 
{
    public enum Modes
    {
        Idle,
        QuickPlay,
        ClassicDeathmatch,
     //   deathmatchKnockOut,  TODO: Check if mode is good to add
        PowerBattle,
        Domination,
        Survival,
        Arcade,
        Tutorial
    //        deathmatchKnockOut TODO: Yep, it's good to add. Definitely add it
    }

    public void classicDeathmatch(GameObject deathLaser)
    {
        GameObject deathLaserInstance = GameObject.Instantiate(deathLaser);
    }

}

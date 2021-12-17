using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class PowerupInformation 
{
    public PowerupInformation(Powerup powerup, float duration)
    {
        this.powerup = powerup;
        this.duration = duration;
    }
    public Powerup powerup;
    public float duration;
}

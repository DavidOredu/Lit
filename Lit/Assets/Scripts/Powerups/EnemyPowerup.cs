using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPowerup : MonoBehaviour
{
    Opponent opponent;
    public PowerupBehaviour powerupBehaviour { get; set; }
    // Start is called before the first frame update
    void Start()
    {
        opponent = transform.root.gameObject.GetComponent<Opponent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UsePowerup()
    {
        if (opponent == null)
            opponent = transform.root.GetComponent<Opponent>();
        if(powerupBehaviour == null) { return; }
        powerupBehaviour.ActivatePowerup();

        opponent.powerup = null;
        powerupBehaviour = null;
        
    }
}

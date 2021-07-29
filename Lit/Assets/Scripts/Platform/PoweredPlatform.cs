using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PoweredPlatform : MonoBehaviour
{
    public enum Power
    {
        RollUnder

    }

    public Power currentPower;

    private Racer racer;
    private Runner runner;

    private void Start()
    {
        
    }

    private void Update()
    {
   
    }

    public void DefinePower(Racer racer)
    {
        switch (currentPower)
        {
            case Power.RollUnder:
                RollUnder(racer);
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.CompareTag("Player") || other.collider.CompareTag("Opponent"))
        {
            racer = other.gameObject.GetComponent<Racer>();
            runner = new Runner(null, null, null, racer);
        }
    }


    public void RollUnder(Racer racer)
    {
        if(racer != null)
        {
            switch (racer.currentRacerType)
            {
                case Racer.RacerType.Player:
                    racer.StateMachine.ChangeState(racer.playerSlideState);
                    break;
                case Racer.RacerType.Opponent:
                    racer.StateMachine.ChangeState(racer.opponentSlideState);
                    break;
                default:
                    break;
            }
            
        }     
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelowMap : MonoBehaviour
{
    private Collider2D[] cols;
    // Start is called before the first frame update
    void Start()
    {
        cols = GetComponents<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Opponent"))
        {
            var racer = other.GetComponent<Racer>();
            ResetRunner(racer);
        }
    }
    void ResetRunner(Racer racer)
    {
        racer.movementVelocity = 0;
        racer.transform.position = racer.checkpoint.transform.position;

        switch (racer.currentRacerType)
        {
            case Racer.RacerType.Player:
                racer.StateMachine.ChangeDamagedState(racer.playerRevivedState);
                break;
            case Racer.RacerType.Opponent:
                racer.StateMachine.ChangeDamagedState(racer.opponentRevivedState);
                break;
            default:
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}

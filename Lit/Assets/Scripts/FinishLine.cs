using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player") || other.CompareTag("Opponent"))
        {
            var racer = other.GetComponent<Racer>();
            if (!gameManager.finishedRacers.Contains(racer))
            {
                gameManager.finishedRacers.Insert(gameManager.finishedRacers.Count, racer);

                switch (racer.currentRacerType)
                {
                    case Racer.RacerType.Player:
                        racer.StateMachine.ChangeState(racer.playerWinState);
                        break;
                    case Racer.RacerType.Opponent:
                        racer.StateMachine.ChangeState(racer.opponentWinState);
                        break;
                    default:
                        break;
                }
                gameManager.GameCompleted();
            }
        }
    }
}

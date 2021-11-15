using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private GameManager gameManager;
    [SerializeField] private float lineDistance;
    public static bool hasCrossedLine = false;

    public BoxCollider2D finishArea;
    public LayerMask whatIsRunner;
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
                        racer.StateMachine.ChangeState(racer.playerQuickHaltState);
                        break;
                    case Racer.RacerType.Opponent:
                        racer.StateMachine.ChangeState(racer.opponentQuickHaltState);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    private bool hasCrossed()
    {
        return Physics2D.Raycast(transform.position, new Vector2(transform.position.x, transform.position.y + lineDistance), whatIsRunner);
    }
    // Update is called once per frame
    void Update()
    {
        if (hasCrossed())
            hasCrossedLine = true;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + lineDistance));
    }
}

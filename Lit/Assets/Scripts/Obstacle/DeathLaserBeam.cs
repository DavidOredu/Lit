using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathLaserBeam : Laser
{
    public float killWaitTime;

    public override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);
        var racer = other.collider.GetComponent<Racer>();
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Opponent")
                {
                    gravityScaleTemp = racer.RB.gravityScale;
                    racer.isAffectedByDeathLaser = true;
                    racer.RB.gravityScale = -negativeGravityScale;
                    switch (racer.currentRacerType)
                    {
                        case Racer.RacerType.Player:
                            var comp = racer.gameObject.GetComponent<Player>();
                            comp.StateMachine.ChangeState(comp.playerElectrocutedState);
                            break;
                        case Racer.RacerType.Opponent:
                            var comp1 = racer.gameObject.GetComponent<Opponent>();
                            comp1.StateMachine.ChangeState(comp1.opponentElectrocutedState);
                            break;
                        default:
                            break;
                    }
                    StartCoroutine(KillRunner(gravityScaleTemp, racer));
                //    StartCoroutine(LaserDown(gravityScaleTemp, null, playerNN, null));
                }
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();

        
    }

    
    IEnumerator KillRunner(float temp, Racer player)
    {
        yield return new WaitForSeconds(killWaitTime);

        if (player != null && player.isAffectedByDeathLaser)
        {
            player.RB.gravityScale = 0;
            player.isAffectedByLaser = false;
            switch (player.currentRacerType)
            {
                case Racer.RacerType.Player:
                    var comp = player.gameObject.GetComponent<Player>();
                    comp.StateMachine.ChangeState(comp.playerDeadState);
                    break;
                case Racer.RacerType.Opponent:
                    var comp1 = player.gameObject.GetComponent<Opponent>();
                    comp1.StateMachine.ChangeState(comp1.opponentDeadState);
                    break;
                default:
                    break;
            }
        }
    }
}

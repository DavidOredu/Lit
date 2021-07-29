using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{
    public Collider2D body { get; private set; }
    public SpriteRenderer image {get;private set; }

    public Racer racer { get; private set; }
    public ParticleSystem pSystem { get; private set; }

    protected RunnerDamagesOperator runnerDamages;
    public bool hasElapsed { get; set; }

    public float laserDamageTime = 3.5f;
    public float negativeGravityScale = 1.5f;

    protected float gravityScaleTemp;
    // Start is called before the first frame update
    public virtual void Start()
    {
        runnerDamages.InitDamages();
        body = GetComponent<Collider2D>();
        image = GetComponent<SpriteRenderer>();
        pSystem = GetComponent<ParticleSystem>();

        hasElapsed = false;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        if (racer != null && racer.isAffectedByDeathLaser)
        {
            StopCoroutine(LaserDown(gravityScaleTemp, racer));
            switch (racer.currentRacerType)
            {
                case Racer.RacerType.Player:
                    var comp = racer.gameObject.GetComponent<Player>();
                    comp.StateMachine.ChangeState(comp.playerDeadState);
                    break;
                case Racer.RacerType.Opponent:
                    var comp1 = racer.gameObject.GetComponent<Opponent>();
                    comp1.StateMachine.ChangeState(comp1.opponentDeadState);
                    break;
                default:
                    break;
            }
            
        }
    }

    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        var racer = other.collider.GetComponent<Racer>();
        if ((other.gameObject.tag == "Player" || other.gameObject.tag == "Opponent") && !racer.isInvulnerable)
        {
            runnerDamages.laser.damaged = true;
            runnerDamages.laser.negativeGravityScale = -negativeGravityScale;
            runnerDamages.laser.damageTime = laserDamageTime;
            racer.transform.SendMessage("DamageRunner", runnerDamages);
        }
        
    }
    public virtual IEnumerator LaserDown(float temp, Racer racer)
    {
        switch (racer.currentRacerType)
        {
            case Racer.RacerType.Player:
                yield return new WaitForSeconds(racer.playerData.laserStunTime);
                break;
            case Racer.RacerType.Opponent:
                yield return new WaitForSeconds(racer.difficultyData.laserStunTime);
                break;
            default:
                break;
        }
        
        
        if (racer != null && racer.isAffectedByLaser)
        {
            racer.RB.gravityScale = temp;
            racer.isAffectedByLaser = false;

            switch (racer.currentRacerType)
            {
                case Racer.RacerType.Player:
                    var comp = racer.gameObject.GetComponent<Player>();
                    comp.StateMachine.ChangeState(comp.playerStunState);
                    break;
                case Racer.RacerType.Opponent:
                    var comp1 = racer.gameObject.GetComponent<Opponent>();
                    comp1.StateMachine.ChangeState(comp1.opponentStunState);
                    break;
                default:
                    break;
            }
            
        }
        hasElapsed = true;
    }

    
}

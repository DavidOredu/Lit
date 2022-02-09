using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class holding the properties and logic of in-game obstacles, traps, debuffs, etc.
/// </summary>
public class Obstacle : MonoBehaviour
{
    public ObstacleType currentObstacleType;

    private RunnerDamagesOperator runnerDamages;
    private ObstacleData obstacleData;

    public Timer laserLifeTimer;
    public Timer laserStartTimer;
    public bool isLaserActive;

    public float timeToStartLaser = 2f;
    public float laserLifeTime = 3f;

    private BeamProjectileScript beamProjectileScript;
    // Start is called before the first frame update
    void Start()
    {
        obstacleData = Resources.Load<ObstacleData>("ObstacleData");
        runnerDamages.InitDamages();

        laserStartTimer = new Timer(timeToStartLaser);
        laserLifeTimer = new Timer(laserLifeTime);

        laserStartTimer.SetTimer();
        laserLifeTimer.SetTimer();

        if (beamProjectileScript == null)
            beamProjectileScript = GetComponent<BeamProjectileScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(currentObstacleType == ObstacleType.LaserBeam)
        {
            if(beamProjectileScript.canExtend && !laserLifeTimer.isTimeUp)
            {
                laserLifeTimer.UpdateTimer();
            }
            else if (laserLifeTimer.isTimeUp)
            {
                beamProjectileScript.canExtend = false;
                laserLifeTimer.ResetTimer();
            }

            if(!beamProjectileScript.canExtend && !laserStartTimer.isTimeUp)
            {
                laserStartTimer.UpdateTimer();
            }
            else if (laserStartTimer.isTimeUp)
            {
                beamProjectileScript.canExtend = true;
                laserStartTimer.ResetTimer();
            }

            isLaserActive = beamProjectileScript.canExtend;
        }
    }
    private void OnCollisionEnter2D(Collision2D other)
    {
        switch (currentObstacleType)
        {
            #region Breakable Obstacles
            case ObstacleType.Breakable:
                if (other.collider.CompareTag("Player") || other.collider.CompareTag("Opponent"))
                {
                    Racer racer = other.collider.GetComponent<Racer>();

                    racer.movementVelocity -= obstacleData.speedReductionPercentage;
                    BreakObstacle();

                    // remove this destruction when animation is made
                    Destroy(gameObject);
                    // place a camera shake effect
                    // tell the player to turn the move state bool to false
                    // tell the player to play a stumble animation
                    // when the animation is done check if is grounded or in air, then turn the appropriate bool true. This should be done in the stumble animation
                    // a better way should be used, like an animation then destroyed afterwards, this is just for testing edit: i meant for the death of this gameobject that is
                }
                break;
            #endregion

            #region Laser Orbs
            case ObstacleType.LaserOrb:
                
                if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Opponent"))
                {
                    var racer = other.collider.gameObject.GetComponent<Racer>();
                    Utils.SetDamageVariables(runnerDamages, null, 8, obstacleData.laserDamagePercentage, obstacleData.laserDamageRate, racer.gameObject);
                    // TODO: play particle hit animation
                    ExplodeLaserOrb();
                    Debug.Log("Runner has collided with laser orb. Runner is taking damage!");
                    Destroy(gameObject);
                    
                }
                break;
            #endregion

            #region Released Laser Barricade
            case ObstacleType.ReleasedLaserBarricade:
                if(other.gameObject.tag == "Player" || other.gameObject.tag == "Opponent")
                {
                    var racer = other.collider.GetComponent<Racer>();
                    racer.movementVelocity -= obstacleData.speedReductionPercentage;
                    DestroyBarricade();
                    Destroy(gameObject);
                }
                break;
            #endregion

            case ObstacleType.RotatingLaserBeam:
                break;

            #region Wall Obstacle
            case ObstacleType.Wall:
                if (other.collider.CompareTag("Player") || other.collider.CompareTag("Opponent"))
                {
                    Racer racer = other.collider.GetComponent<Racer>();
                    switch (racer.currentRacerType)
                    {
                        case Racer.RacerType.Player:
                            if (racer.StateMachine.CurrentState != racer.playerSlideState)
                            {
                                racer.StateMachine.ChangeState(racer.playerKnockbackState);
                            }
                            break;
                        case Racer.RacerType.Opponent:
                            if (racer.StateMachine.CurrentState != racer.opponentSlideState)
                            {
                                racer.StateMachine.ChangeState(racer.opponentKnockbackState);
                            }
                            break;
                        default:
                            break;
                    }
                }
                break;
            #endregion

            #region Final Wall Obstacle
            case ObstacleType.FinalWall:
                if (other.gameObject.tag == "Player" || other.gameObject.tag == "Opponent")
                {
                    // tell the player that the final wall means the end of the line
                }
                    break;
            #endregion

            #region Hard Platform Obstacle
            case ObstacleType.HardPlatform:
                if (other.gameObject.tag == "Player" || other.gameObject.tag == "Opponent")
                {
                    // take to impact knockdown state
                }
                    break;
            #endregion

            #region Death Laser
            case ObstacleType.DeathLaser:
                if (other.gameObject.tag == "Player" || other.gameObject.tag == "Opponent")
                {
                    var racer = other.collider.GetComponent<Racer>();
                    runnerDamages.deathLaser.damaged = true;
                    runnerDamages.deathLaser.damageInt = 9;
                    racer.transform.SendMessage("DamageRunner", runnerDamages);
                }
                    break;
            #endregion
        }
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        switch (currentObstacleType)
        {
            #region Released Laser Barricade
            case ObstacleType.ReleasedLaserBarricade:
                if (other.gameObject.tag == "Player" || other.gameObject.tag == "Opponent")
                {
                    // trigger the laser beam after some time
                }
                    break;
            #endregion

            #region Final Wall Obstacle
            case ObstacleType.FinalWall:
                if (other.gameObject.tag == "Player" || other.gameObject.tag == "Opponent")
                {
                    // get component of animator on the final wall
                    // close the wall
                }
                    break;
                #endregion
        }
    }
    public void BreakObstacle()
    {
        // play destroy animation
        // destroy gamobject
    }
    public void ExplodeLaserOrb()
    {
        // play laser explosion animation
        gameObject.SetActive(false);
        //destroy gameobject
    }
    public void DestroyBarricade()
    {
        // play barricade destruction animation
        // destroy gameobject
    }
    public enum ObstacleType
    {
        Breakable,
        LaserOrb,
        ReleasedLaserBarricade,
        RotatingLaserBeam,
        Wall,
        FinalWall,
        /// <summary>
        /// Platforms within base platforms that cause damage when collided with.
        /// </summary>
        HardPlatform,
        DeathLaser,
        LaserBeam,
    }
}

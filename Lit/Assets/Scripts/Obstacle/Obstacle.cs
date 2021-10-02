using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    public ObstacleType currentObstacleType;

    private RunnerDamagesOperator runnerDamages;
    private ObstacleData obstacleData;
    // Start is called before the first frame update
    void Start()
    {
        obstacleData = Resources.Load<ObstacleData>("ObstacleData");
        runnerDamages.InitDamages();
    }

    // Update is called once per frame
    void Update()
    {
        
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

                    racer.movementVelocity -= obstacleData.speedReduction;
                    BreakObstacle();
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
                
                if ((other.gameObject.tag == "Player" || other.gameObject.tag == "Opponent") && !other.collider.GetComponent<Racer>().isInvulnerable)
                {
                    var racer = other.collider.GetComponent<Racer>();
                    runnerDamages.laser.damaged = true;
                    runnerDamages.laser.damageStrength = obstacleData.laserDamageStrength;
                    racer.transform.SendMessage("DamageRunner", runnerDamages);
                    // TODO: play particle hit animation
                    ExplodeLaserOrb();
                    Destroy(gameObject);
                }
                break;
            #endregion

            #region Released Laser Barricade
            case ObstacleType.ReleasedLaserBarricade:
                if(other.gameObject.tag == "Player" || other.gameObject.tag == "Opponent")
                {
                    var racer = other.collider.GetComponent<Racer>();
                    racer.movementVelocity -= obstacleData.speedReduction;
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
        HardPlatform,
        DeathLaser,
    }
}

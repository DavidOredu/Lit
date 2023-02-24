using UnityEngine;

/// <summary>
/// Class holding the properties and logic of in-game obstacles, traps, debuffs, etc.
/// </summary>
public class Obstacle : MonoBehaviour, IDamageable
{
    public ObstacleType currentObstacleType;

    private RunnerDamagesOperator runnerDamages;
    private ObstacleData obstacleData;

    private Timer laserLifeTimer;
    private Timer laserStartTimer;

    private GameObject finalWallGO;
    private Timer triggeredLaserStartTimer;
    public bool isLaserActive { get; private set; }

    [Header("FIXED LASER BEAM")]
    public float fixedLaserBeamDistance;
    public Vector2 direction;

    private BeamProjectileScript beamProjectileScript;
    // Start is called before the first frame update
    void Start()
    {
        obstacleData = Resources.Load<ObstacleData>("ObstacleData");
        runnerDamages.InitDamages();

        // For a Laser Beam...
        if (currentObstacleType == ObstacleType.LaserBeam)
        {

            laserStartTimer = new Timer(obstacleData.timeToStartLaser);
            laserLifeTimer = new Timer(obstacleData.laserLifeTime);

            laserStartTimer.SetTimer();
            laserLifeTimer.SetTimer();

            beamProjectileScript = GetComponent<BeamProjectileScript>();

            beamProjectileScript.damagePercentage = obstacleData.laserDamagePercentage;
            beamProjectileScript.damageRate = obstacleData.laserDamageRate;
            beamProjectileScript.damageType = 8;
        }
        // For a Fixed Laser Beam...
        else if (currentObstacleType == ObstacleType.FixedLaserBeam)
        {
            beamProjectileScript = GetComponent<BeamProjectileScript>();

            beamProjectileScript.damagePercentage = obstacleData.laserDamagePercentage;
            beamProjectileScript.damageRate = obstacleData.laserDamageRate;
            beamProjectileScript.damageType = 8;
            beamProjectileScript.extensionType = BeamProjectileScript.BeamExtensionType.Fixed;
            beamProjectileScript.extensionLimit = fixedLaserBeamDistance;
            beamProjectileScript.directionToHit = direction;
        }
        // For a Final Wall...
        else if (currentObstacleType == ObstacleType.FinalWall)
        {
            triggeredLaserStartTimer = new Timer(obstacleData.finalWallLaserStartTime);
            triggeredLaserStartTimer.SetTimer();
            finalWallGO = transform.Find("FinalWallLaserGFX").gameObject;
        }
        // For a Released Laser Barricade...
        else if (currentObstacleType == ObstacleType.ReleasedLaserBarricade)
        {
            triggeredLaserStartTimer = new Timer(obstacleData.triggeredLaserStartTime);
            triggeredLaserStartTimer.SetTimer();

            beamProjectileScript = GetComponent<BeamProjectileScript>();

            beamProjectileScript.damagePercentage = obstacleData.laserDamagePercentage;
            beamProjectileScript.damageRate = obstacleData.laserDamageRate;
            beamProjectileScript.damageType = 8;
            beamProjectileScript.canExtend = false;
            beamProjectileScript.extensionSpeed = new Vector3(beamProjectileScript.extensionSpeed.x, .5f, beamProjectileScript.extensionSpeed.z);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        SwitchLaserBeam();
    }
    private void SwitchLaserBeam()
    {
        // For a Laser Beam...
        if (currentObstacleType == ObstacleType.LaserBeam)
        {
            if (beamProjectileScript.canExtend && !laserLifeTimer.isTimeUp)
            {
                laserLifeTimer.UpdateTimer();
            }
            else if (laserLifeTimer.isTimeUp)
            {
                beamProjectileScript.canExtend = false;
                laserLifeTimer.ResetTimer();
            }

            if (!beamProjectileScript.canExtend && !laserStartTimer.isTimeUp)
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
        else if (currentObstacleType == ObstacleType.FinalWall)
        {
            if (isLaserActive)
            {
                if (triggeredLaserStartTimer.isTimeUp)
                {
                    // Release the wall
                    finalWallGO.SetActive(true);
                }
                else
                {
                    triggeredLaserStartTimer.UpdateTimer();
                }
            }
        }
        // For a Released Laser Barricade...
        else if (currentObstacleType == ObstacleType.ReleasedLaserBarricade)
        {
            if (isLaserActive)
            {
                if (triggeredLaserStartTimer.isTimeUp)
                {
                    beamProjectileScript.canExtend = true;
                }
                else
                {
                    triggeredLaserStartTimer.UpdateTimer();
                }
            }
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

                    racer.strength -= obstacleData.speedReductionPercentage;
                    racer.movementVelocity -= obstacleData.speedReductionPercentage;
                    racer.racerDamages.canIncreaseStrength = true;
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
                    Utils.SetDamageVariables(runnerDamages, null, 8, obstacleData.laserDamagePercentage, obstacleData.laserDamageRate, other.collider.gameObject);
                    // TODO: play particle hit animation
                    ExplodeLaserOrb();
                    Debug.Log("Runner has collided with laser orb. Runner is taking damage!");
                    gameObject.SetActive(false);

                }
                break;
            #endregion

            #region Released Laser Barricade
            case ObstacleType.ReleasedLaserBarricade:
                if (other.gameObject.tag == "Player" || other.gameObject.tag == "Opponent")
                {
                    var racer = other.collider.GetComponent<Racer>();
                    racer.movementVelocity -= obstacleData.speedReductionPercentage;
                    DestroyBarricade();
                    Destroy(gameObject);
                }
                break;
            #endregion

            case ObstacleType.FixedLaserBeam:
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
                if (other.collider.CompareTag("Player") || other.collider.CompareTag("Opponent"))
                {
                    if (other.relativeVelocity.x >= 12f)
                    {
                        Racer racer = other.collider.GetComponent<Racer>();
                        switch (racer.currentRacerType)
                        {
                            case Racer.RacerType.Player:
                                racer.StateMachine.ChangeState(racer.playerKnockOutState);
                                break;
                            case Racer.RacerType.Opponent:
                                racer.StateMachine.ChangeState(racer.opponentKnockOutState);
                                break;
                        }
                    }
                    if (other.relativeVelocity.y >= 15f)
                    {

                    }
                    // check the velocity of the collision with: other.relativeVelocity and check if this velocity is greater than certain threshold value. If it is, take to impact knockdown state and let the state decide if the player is grounded or not
                    // if the player is grounded, take to hard fall state and reduce speed, if is not grounded, player must have collided with platform body. Take to the fall state and also reduce speed
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
                if (other.CompareTag("Player") || other.CompareTag("Opponent"))
                {
                    // trigger the laser beam after some time
                    if (!isLaserActive)
                    {
                        isLaserActive = true;
                    }
                }
                break;
            #endregion

            #region Final Wall Obstacle
            case ObstacleType.FinalWall:
                if (other.CompareTag("Player") || other.CompareTag("Opponent"))
                {
                    if (!isLaserActive)
                    {
                        isLaserActive = true;
                    }
                    if (finalWallGO.activeSelf)
                    {
                        if (other.CompareTag("Player") || other.CompareTag("Opponent"))
                        {
                            Utils.SetDamageVariables(runnerDamages, null, 9, 1f, 1f, other.gameObject, false);
                            GameManager.instance.activeRacers.Remove(other.GetComponent<Racer>());
                        }
                    }
                    // get component of animator on the final wall
                    // close the wall
                }
                break;
            #endregion

            #region Death Laser
            case ObstacleType.DeathLaser:
                if (other.CompareTag("Player") || other.CompareTag("Opponent"))
                {
                    Utils.SetDamageVariables(runnerDamages, null, 9, 1f, 1f, other.gameObject, false);
                    GameManager.instance.activeRacers.Remove(other.GetComponent<Racer>());
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

    public void Damage()
    {
        switch (currentObstacleType)
        {
            case ObstacleType.Breakable:
                BreakObstacle();
                break;
            case ObstacleType.LaserOrb:
                ExplodeLaserOrb();
                break;
            case ObstacleType.ReleasedLaserBarricade:
                DestroyBarricade();
                break;
        }
    }

    public enum ObstacleType
    {
        Breakable,
        LaserOrb,
        ReleasedLaserBarricade,
        FixedLaserBeam,
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

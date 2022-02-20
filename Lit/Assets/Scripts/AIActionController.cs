using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to make actions based on detections made by the AISensors in the opponent.
/// </summary>
public class AIActionController : MonoBehaviour
{
    public Opponent aIRacer;
    //public System.Type[] types = new System.Type[] { typeof(Racer) };
    //public List<System.Type> types1 = new List<System.Type>() { typeof(Racer), typeof(YieldInstruction), typeof(tk2dAnimatedSprite), typeof(RunnerDamagesOperator) };

    [Header("PROBABILITIES")]
    public Probability<bool> jumpToLitProbability;
    public Probability<bool> jumpToLitIfIsLitProbability;
    public Probability<bool> jumpToLitIfIsDarkProbability;
    public Probability<bool> jumpToPowerProbability;
    public Probability<bool> jumpToBaseProbability;
    public Probability<bool> defensivePowerPlatformUseProbability;

    private List<bool> boolList = new List<bool> { true, false };

    private bool hasObtainedProbability = false;
    // Start is called before the first frame update
    void Start()
    {
        #region Probabilities Initialization
        jumpToLitProbability = new Probability<bool>(aIRacer.difficultyData.jumpToLitProbabilityCurve);
        jumpToLitIfIsLitProbability = new Probability<bool>(aIRacer.difficultyData.jumpToLitIfLitProbabilityCurve);
        jumpToLitIfIsDarkProbability = new Probability<bool>(aIRacer.difficultyData.jumpToLitIfDarkProbabilityCurve);
        jumpToPowerProbability = new Probability<bool>(aIRacer.difficultyData.jumpToPowerProbabilityCurve);
        jumpToBaseProbability = new Probability<bool>(aIRacer.difficultyData.jumpToBaseProbabilityCurve);
        defensivePowerPlatformUseProbability = new Probability<bool>(aIRacer.difficultyData.defensivePowerPlatformProbabilityCurve);
        jumpToLitProbability.InitDictionary(boolList);
        jumpToLitIfIsLitProbability.InitDictionary(boolList);
        jumpToLitIfIsDarkProbability.InitDictionary(boolList);
        jumpToPowerProbability.InitDictionary(boolList);
        jumpToBaseProbability.InitDictionary(boolList);
        defensivePowerPlatformUseProbability.InitDictionary(boolList);
        #endregion
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        foreach (var pair in aIRacer.aISensors)
        {
            if (pair.Value.hasNewDetection)
            {
                switch (pair.Key)
                {
                    case Entity.Sensors.HigherPlatformSensor:
                        ProcessHigherPlatformDetection(pair.Value);
                        break;
                    case Entity.Sensors.ObstacleSensor:
                        ProcessObstacleDetection(pair.Value);
                        break;
                    case Entity.Sensors.LedgeSensor:
                        ProcessLedgeDetection(pair.Value);
                        break;
                    case Entity.Sensors.ProjectileSensor:
                        ProcessProjectileDetection(pair.Value);
                        break;
                    case Entity.Sensors.PlayerDefenseSensor:
                        ProcessPlayerDefenseDetection(pair.Value);
                        break;
                    case Entity.Sensors.PlayerAttackSensor:
                        ProcessPlayerAttackDetection(pair.Value);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    #region Sensor Actions
    /// <summary>
    /// Action to run if we sense a platform on a higher plane.
    /// </summary>
    /// <param name="sensor">The sensor object that detected the higher platform.</param>
    private void ProcessHigherPlatformDetection(AISensor sensor)
    {
        if (sensor.sensorType != AISensor.SensorType.Linear)
        {
            if (sensor.IsNewDetectionNull()) { return; }

            if (sensor.CompareNewDetectionTag("LitPlatform"))
            {
                LitPlatformNetwork litPlatform = sensor.GetComponentFromDetection<LitPlatformNetwork>(sensor.newDetection);


                // run probability based on intelligence
                bool canJumpToLit = jumpToLitProbability.ProbabilityGenerator();


                bool jump;
                // check if we are the dark runner
                if (aIRacer.runner.stickmanNet.currentColor.colorID == 0)
                {
                    var canJumpToLitIfDark = jumpToLitIfIsDarkProbability.ProbabilityGenerator();
                    jump = canJumpToLit && canJumpToLitIfDark;
                }
                else
                {
                    // check if litplatform is lit
                    if (litPlatform.isLit)
                    {
                        var canJumpToLitIfLit = jumpToLitIfIsLitProbability.ProbabilityGenerator();
                        jump = canJumpToLit && canJumpToLitIfLit;
                    }
                    else
                    {
                        jump = canJumpToLit;
                    }
                }

                // determine action of ai
                JumpAction(jump);

                // accept new detection
                sensor.AcceptNewDetection();
                Debug.Log(jump);
                Debug.Log("Has accepted detection for higher platform sensor!");
                Debug.Log(sensor.newDetection.collider.name);
            }
            else if (sensor.CompareNewDetectionTag("PowerPlatform"))
            {
                PoweredPlatform poweredPlatform = sensor.GetComponentFromDetection<PoweredPlatform>(sensor.newDetection);
                PoweredPlatform.PowerPlatformAidType platformAidType = poweredPlatform.currentPowerAidType;

                bool jump = default;
                // check if is defensive or booster power platform
                switch (platformAidType)
                {
                    case PoweredPlatform.PowerPlatformAidType.Defensive:
                        if (poweredPlatform.currentPower == PoweredPlatform.Power.Action)
                        {
                            // run probability check
                            jump = jumpToPowerProbability.ProbabilityGenerator();
                        }
                        break;
                    case PoweredPlatform.PowerPlatformAidType.Booster:
                        // run probability check
                        jump = jumpToPowerProbability.ProbabilityGenerator();
                        break;
                    default:
                        break;
                }

                // if is defensive, check if is 'action' power platform

                // determine action of ai
                JumpAction(jump);

                // accept new detection
                sensor.AcceptNewDetection();
                Debug.Log(jump);
                Debug.Log("Has accepted detection for higher platform sensor!");
                Debug.Log(sensor.newDetection.collider.name);

            }
            else if (sensor.CompareNewDetectionTag("BasePlatform"))
            {
                // run probability
                bool jump = jumpToBaseProbability.ProbabilityGenerator();
                // determine action of ai
                JumpAction(jump);
                // accept new detection
                sensor.AcceptNewDetection();
                Debug.Log(jump);
                Debug.Log("Has accepted detection for higher platform sensor!");
                Debug.Log(sensor.newDetection.collider.name);
            }

        }
        else
        {
            Debug.LogError("Please change sensor type 'linear' type sensor for correct execution.");
        }
    }
    /// <summary>
    /// Action to run if we sense an obstacle.
    /// </summary>
    /// <param name="sensor">The sensor object that detected the obstacle.</param>
    private void ProcessObstacleDetection(AISensor sensor)
    {
        if (sensor.sensorType == AISensor.SensorType.Radial)
        {
            if (sensor.IsNewDetectionNull()) { return; }

            var obstacle = sensor.GetComponentFromDetection<Obstacle>(sensor.newDetection);
            Debug.Log($"obstacle object: {obstacle}, {obstacle.currentObstacleType}");
            DetermineObstacleTypeLogic(sensor, obstacle);
            Debug.Log("End of obstacle logic!");

        }
        else
        {
            Debug.LogError("Please change sensor type to 'radial' type sensor for correct execution.");
        }
    }
    /// <summary>
    /// Action to run if we detect a ledge i.e no longer detecting ground.
    /// </summary>
    /// <param name="sensor">The sensor object that no longer detected ground.</param>
    private void ProcessLedgeDetection(AISensor sensor)
    {
        if (sensor.sensorType == AISensor.SensorType.Linear)
        {
            if (sensor.IsNewDetectionNull())
            {
                //  JumpAction(true);
            }
        }
        else
        {
            Debug.LogError("Please change sensor type to 'linear' type sensor for correct execution.");
        }
    }
    /// <summary>
    /// Action to run if we detect a projectile.
    /// </summary>
    /// <param name="sensor">The sensor object that detected the projectile.</param>
    private void ProcessProjectileDetection(AISensor sensor)
    {
        if (sensor.sensorType == AISensor.SensorType.Radial)
        {
            if (!sensor.IsNewDetectionNull())
            {
                // TODO: refer to "runner abilities" note in Desktop to view logic
            }
        }
        else
        {
            Debug.LogError("Please change sensor type 'radial' type sensor for correct execution.");
        }
    }
    /// <summary>
    /// Action to run if our runner is vulnerable to attack, and detects another runner is offensive.
    /// </summary>
    /// <param name="sensor">The sensor object that detected an offensive runner.</param>
    private void ProcessPlayerDefenseDetection(AISensor sensor)
    {
        if (sensor.sensorType == AISensor.SensorType.Radial)
        {
            // for instances where other players are in an offensive mode e.g awakened state, using element field powerup
            if (!sensor.IsNewDetectionNull())
            {
                foreach (var detection in sensor.newDetection)
                {
                    Racer racer = sensor.GetComponentFromDetection<Racer>(detection);
                    if (racer.isAwakened)
                    {
                        Debug.Log("Throwing defensive action!");
                    }
                    else if (FindActivePowerup(racer, "ElementField"))
                    {
                        Debug.Log("Throwing defensive action!");
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Please change sensor type 'radial' type sensor for correct execution.");
        }
    }
    /// <summary>
    /// Action to run if we detect a vulnerable runner while we are offensive or can be offensive.
    /// </summary>
    /// <param name="sensor">The sensor object that detected a vulnerable runner.</param>
    private void ProcessPlayerAttackDetection(AISensor sensor)
    {
        if (sensor.sensorType == AISensor.SensorType.Radial)
        {
            // for instances where other players are vulnerable to attack, decides if should use powerup to attack, awakened state or action power platform
            if (!sensor.IsNewDetectionNull())
            {
                foreach (var detection in sensor.newDetection)
                {
                    Racer racer = sensor.GetComponentFromDetection<Racer>(detection);
                    if (!racer.isInvulnerable)
                    {
                        if (aIRacer.GamePlayer.powerup != null)
                        {
                            if (aIRacer.GamePlayer.powerup.powerupType == Powerup.PowerupType.offensive)
                            {
                                //  aIRacer.GamePlayer.enemyPowerup.UsePowerup();
                                Debug.Log("Throwing offensive action!");
                            }
                        }
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Please change sensor type 'radial' type sensor for correct execution.");
        }
    }
    #endregion

    #region Other Functions
    private void JumpAction(bool jump)
    {
        if (jump)
        {
            aIRacer.StateMachine.ChangeState(aIRacer.opponentJumpState);
            Debug.Log("AI decided to jump to as action!");
        }
    }
    private void DetermineObstacleTypeLogic(AISensor sensor, Obstacle obstacle)
    {
        switch (obstacle.currentObstacleType)
        {
            case Obstacle.ObstacleType.Breakable:
                if (aIRacer.isOnPower)
                {
                    if (aIRacer.poweredPlatform.currentPower == PoweredPlatform.Power.Action)
                    {
                        aIRacer.canUsePowerPlatform = defensivePowerPlatformUseProbability.ProbabilityGenerator();
                        aIRacer.UsePowerPlatform(false);
                        Debug.Log("Has used power platform!");
                        sensor.AcceptNewDetection();
                        Debug.Log("Has accepted detection for higher platform sensor!");
                        Debug.Log(sensor.newDetection.name);
                    }
                }
                break;
            case Obstacle.ObstacleType.LaserOrb:
                break;
            case Obstacle.ObstacleType.ReleasedLaserBarricade:
                break;
            case Obstacle.ObstacleType.RotatingLaserBeam:
                break;
            case Obstacle.ObstacleType.Wall:
                Debug.Log("Starting wall obstacle behaviour!");
                if (aIRacer.poweredPlatform != null)
                {
                    if (aIRacer.poweredPlatform.currentPower == PoweredPlatform.Power.RollUnder)
                    {
                        aIRacer.canUsePowerPlatform = defensivePowerPlatformUseProbability.ProbabilityGenerator();
                        Debug.Log(aIRacer.canUsePowerPlatform);
                        aIRacer.UsePowerPlatform(false);
                        // if (aIRacer.canUsePowerPlatform)
                        {
                            sensor.AcceptNewDetection();
                            Debug.Log("Has accepted detection for higher platform sensor!");
                            Debug.Log(sensor.newDetection.name);
                        }

                    }
                }
                break;
            case Obstacle.ObstacleType.FinalWall:
                break;
            case Obstacle.ObstacleType.HardPlatform:
                break;
            case Obstacle.ObstacleType.DeathLaser:
                break;
            case Obstacle.ObstacleType.LaserBeam:
                if (aIRacer.poweredPlatform != null)
                {
                    if (aIRacer.poweredPlatform.currentPower == PoweredPlatform.Power.Stop)
                    {
                        if (!hasObtainedProbability)
                        {
                            aIRacer.canUsePowerPlatform = defensivePowerPlatformUseProbability.ProbabilityGenerator();
                            hasObtainedProbability = true;
                            if (aIRacer.canUsePowerPlatform)
                            {

                            }
                            else
                            {
                                sensor.AcceptNewDetection();
                            }
                        }
                        else
                        {
                            if (obstacle.isLaserActive)
                            {
                                aIRacer.UsePowerPlatform(true);
                            }
                            else
                            {
                                hasObtainedProbability = false;
                                aIRacer.canUsePowerPlatform = false;
                                sensor.AcceptNewDetection();
                            }
                        }
                    }
                }
                break;
            default:
                break;
        }
    }
    private bool FindActivePowerup(Racer racer, string powerupName)
    {
        foreach (var powerupInformation in racer.powerupController.activePowerups)
        {
            if (powerupInformation.powerup.name == powerupName)
            {
                return true;
            }
        }
        return false;
    }
    #endregion

    #region Debug Detectors
    [ContextMenu("GetHigherPlatformNewDetection")]
    public void GetHigherPlatformNewDetection()
    {
        Debug.Log(aIRacer.higherPlatformSensor.newDetection.collider.name);
    }
    [ContextMenu("GetHigherPlatformCurrentDetection")]
    public void GetHigherPlatformCurrentDetection()
    {
        Debug.Log(aIRacer.higherPlatformSensor.currentDetection.collider.name);
    }
    [ContextMenu("GetObstacleNewDetection")]
    public void GetObstacleNewDetection()
    {
        Debug.Log(aIRacer.obstacleSensor.newDetection.name);
    }
    [ContextMenu("GetObstacleCurrentDetection")]
    public void GetObstacleCurrentDetection()
    {
        Debug.Log(aIRacer.obstacleSensor.currentDetection.name);
    }
    [ContextMenu("GetLedgeNewDetection")]
    public void GetLedgeNewDetection()
    {
        if (aIRacer.ledgeSensor.newDetection.collider != null)
            Debug.Log(aIRacer.ledgeSensor.newDetection.collider.name);
        else
        {
            Debug.Log("Null");
        }
    }
    [ContextMenu("GetLedgeCurrentDetection")]
    public void GetLedgeCurrentDetection()
    {
        if (aIRacer.ledgeSensor.newDetection.collider != null)
            Debug.Log(aIRacer.ledgeSensor.currentDetection.collider.name);
        else
        {
            Debug.Log("Null");
        }
    }
    #endregion
}

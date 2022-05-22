using System.Collections.Generic;
using UnityEngine;

public class Entity : Racer
{
    public AIActionController actionController;

    [Header("DIFFICULTY")]
    public Difficulty currentDifficulty;
    public D_Entity entityData;
    public int reviveCount;
    
    //private Transform ledgeCheck;
    [Header("CHECK TRANSFORMS")]
    [SerializeField]
    private Transform playerCheck;
    [SerializeField]
    private Transform ledgeCheck;
    [SerializeField]
    private Transform jumpCheck;
    [SerializeField]
    private Transform obstacleCheck;

    [Header("SENSORS")]
    public AISensor powerPlatformSensor;
    public AISensor higherPlatformSensor;
    public AISensor obstacleSensor;
    public AISensor ledgeSensor;
    public AISensor projectileSensor;
    public AISensor playerDefenseSensor;
    public AISensor playerAttackSensor;
    public Dictionary<Sensors, AISensor> aISensors = new Dictionary<Sensors, AISensor>();

    public bool canUsePowerPlatform { get; set; }
    protected bool hasUsedPower;
    public PoweredPlatform oldPowerPlatform;

    public Probability<bool> boosterPowerPlatformUseProbability;

    public override void Awake()
    {
        base.Awake();

        actionController = GetComponent<AIActionController>();
    }
    public override void Start()
    {
        base.Start();
        boosterPowerPlatformUseProbability = new Probability<bool>(difficultyData.boosterPowerPlatformProbabilityCurve, new List<bool> { true, false });

        powerPlatformSensor = new AISensor(GroundCheck, difficultyData.whatIsPower, AISensor.SensorType.Linear, difficultyData.powerCheckDistance, difficultyData.powerCheckDirection);
        higherPlatformSensor = new AISensor(jumpCheck, difficultyData.whatToJumpTo, AISensor.SensorType.Linear, difficultyData.higherPlatformCheckDistance, difficultyData.higherPlatformCheckDirection);
        obstacleSensor = new AISensor(obstacleCheck, difficultyData.whatIsObstacle, AISensor.SensorType.Radial, sensorRadius: difficultyData.obstacleCheckRadius);
        ledgeSensor = new AISensor(ledgeCheck, difficultyData.whatIsGround, AISensor.SensorType.Linear, difficultyData.ledgeCheckDistance, difficultyData.ledgeCheckDirection);
        projectileSensor = new AISensor(playerCenter, difficultyData.whatIsProjectile, AISensor.SensorType.Radial, sensorRadius: difficultyData.projectileCheckRadius);
        playerDefenseSensor = new AISensor(playerCheck, difficultyData.whatIsPlayer, AISensor.SensorType.Radial, sensorRadius: difficultyData.playerDefenseCheckRadius, detectAll: true);
        playerAttackSensor = new AISensor(playerCheck, difficultyData.whatIsPlayer, AISensor.SensorType.Radial, sensorRadius: difficultyData.playerAttackCheckRadius, detectAll: true);

        aISensors.Add(Sensors.HigherPlatformSensor, higherPlatformSensor);
        aISensors.Add(Sensors.ObstacleSensor, obstacleSensor);
        aISensors.Add(Sensors.LedgeSensor, ledgeSensor);
        aISensors.Add(Sensors.ProjectileSensor, projectileSensor);
        aISensors.Add(Sensors.PlayerDefenseSensor, playerDefenseSensor);
        aISensors.Add(Sensors.PlayerAttackSensor, playerAttackSensor);
    }

    public override void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
        StateMachine.DamagedState.LogicUpdate();
        StateMachine.AwakenedState.LogicUpdate();
        base.Update();
    }
    
    public override void FixedUpdate()
    {
        StateMachine.CurrentState.PhysicsUpdate();
        StateMachine.DamagedState.PhysicsUpdate();
        StateMachine.AwakenedState.PhysicsUpdate();
        base.FixedUpdate();
  
        if (CheckIfHasPowerup())
        {
            // 
            GamePlayer.enemyPowerup.UsePowerup();
        }
        poweredPlatform = PowerPlatform();

    }

    public override void LateUpdate()
    {
        StateMachine.CurrentState.LateUpdate();
        StateMachine.DamagedState.LateUpdate();
        StateMachine.AwakenedState.LateUpdate();
        base.LateUpdate();
        
    }
 
    public override void OnCollisionEnter2D(Collision2D other)
    {
        StateMachine.CurrentState.OnCollisionEnter(other);
        StateMachine.DamagedState.OnCollisionEnter(other);
        StateMachine.AwakenedState.OnCollisionEnter(other);
        base.OnCollisionEnter2D(other);
    }
    
    public override void OnCollisionStay2D(Collision2D other)
    {
        StateMachine.CurrentState.OnCollisionStay(other);
        StateMachine.DamagedState.OnCollisionStay(other);
        StateMachine.AwakenedState.OnCollisionStay(other);
        base.OnCollisionStay2D(other);
    }
    public override void OnCollisionExit2D(Collision2D other)
    {
        StateMachine.CurrentState.OnCollisionExit(other);
        StateMachine.DamagedState.OnCollisionExit(other);
        StateMachine.AwakenedState.OnCollisionExit(other);
        base.OnCollisionExit2D(other);

        if (other.collider.CompareTag("PowerPlatform"))
        {
            hasUsedPower = false;
        }
    }
    public virtual void CurrentStateAnimationTrigger()
    {
        StateMachine.CurrentState.AnimationTrigger();
    }
    public virtual void DamageStateAnimationTrigger()
    {
        StateMachine.DamagedState.AnimationTrigger();
    }
    public virtual void CurrentStateAnimationFinishTrigger()
    {
        StateMachine.CurrentState.AnimationFinishTrigger();
    }
    public virtual void DamageStateAnimationFinishTrigger()
    {
        StateMachine.DamagedState.AnimationFinishTrigger();
    }
    #region Set Functions
    public virtual void SetAllVariables()
    {
        runner.player = GetComponent<Opponent>();
        runner.stickman = GetComponent<Stickman>();
    }
    public virtual void SetAIDifficulty()
    {
        switch (currentDifficulty)
        {
            case Difficulty.Easy:
                difficultyData = Resources.Load<D_DifficultyData>("DifficultyData/Easy");
                break;
            case Difficulty.Normal:
                difficultyData = Resources.Load<D_DifficultyData>("DifficultyData/Normal");
                break;
            case Difficulty.Hard:
                difficultyData = Resources.Load<D_DifficultyData>("DifficultyData/Hard");
                break;
            default:
                break;
        }
        racerData = difficultyData;
    }
    public void ChangeDifficulty(Difficulty newDifficulty)
    {
        currentDifficulty = newDifficulty;
        SetAIDifficulty();
    }
    #endregion


    #region Check Functions
    
    public virtual Collider2D[] CheckIfCanJump()
    {
       return Physics2D.OverlapCircleAll(jumpCheck.position, difficultyData.jumpCheckRadius, difficultyData.whatToJumpTo);
    }
    public bool CheckIfHasPowerup()
    {
        return GamePlayer.powerup != null;
    }
    
    #endregion
    //public virtual bool CheckLedge()
    //{
    //    return Physics2D.Raycast(ledgeCheck.position, Vector2.down, entityData.ledgeCheckDistance, entityData.whatIsGround);
    //}
    //public virtual bool CheckWall()
    //{
    //    return Physics2D.Raycast(wallCheck.position, aliveGO.transform.right, entityData.wallCheckDistance, entityData.whatIsGround);
    //}
   
 
    public virtual bool CheckPlayerInTooCloseAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, entityData.tooCloseAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInMinAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, entityData.minAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInMaxAgroRange()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, entityData.maxAgroDistance, entityData.whatIsPlayer);
    }

    public virtual bool CheckPlayerInCloseRangeAction()
    {
        return Physics2D.Raycast(playerCheck.position, transform.right, entityData.closeRangeActionDistance, entityData.whatIsPlayer);
    }

    public enum Difficulty
    {
        Easy,
        Normal,
        Hard,
    }
    public enum Sensors
    {
        HigherPlatformSensor,
        ObstacleSensor,
        LedgeSensor,
        ProjectileSensor,
        PlayerDefenseSensor,
        PlayerAttackSensor,
    }
    public override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        //Gizmos.DrawLine(wallCheck.position, wallCheck.position + (Vector3)(Vector2.right * facingDirection * entityData.wallCheckDistance));
        //Gizmos.DrawLine(ledgeCheck.position, ledgeCheck.position + (Vector3)(Vector2.down * entityData.ledgeCheckDistance));

        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.tooCloseAgroDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.closeRangeActionDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.minAgroDistance), 0.2f);
        Gizmos.DrawWireSphere(playerCheck.position + (Vector3)(Vector2.right * entityData.maxAgroDistance), 0.2f);

        Gizmos.DrawWireSphere(jumpCheck.position, difficultyData.jumpCheckRadius);

    }
}

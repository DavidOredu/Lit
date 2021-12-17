 using DapperDino.Mirror.Tutorials.Lobby;
using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

/// <summary>
/// Class that determines the entire behaviour of all racers, whether player or opponent.
/// </summary>
public class Racer : NetworkBehaviour
{
    #region Variables

    #region State Objects
    public FiniteStateMachine StateMachine;
    #endregion

    #region Player State Objects
    public PlayerNetwork_IdleState playerIdleState { get; set; }
    public PlayerNetwork_MoveState playerMoveState { get; set; }
    public PlayerNetwork_JumpState playerJumpState { get; set; }
    public PlayerNetwork_InAirState playerInAirState { get; set; }
    public PlayerNetwork_LandState playerLandState { get; set; }
    public PlayerNetwork_KnockbackState playerKnockbackState { get; set; }
    public PlayerNetwork_SlideState playerSlideState { get; set; }
    public PlayerNetwork_ActionState playerActionState { get; set; }
    public PlayerNetwork_QuickHaltState playerQuickHaltState { get; set; }
    public PlayerNetwork_FullStopState playerFullStopState { get; set; }
    public PlayerNetwork_ShadowedState playerShadowedState { get; set; }
    public PlayerNetwork_BurningState playerBurningState { get; set; }
    public PlayerNetwork_FrozenState playerFrozenState { get; set; }
    public PlayerNetwork_ChokingState playerChokingState { get; set; }
    public PlayerNetwork_BlindedState playerBlindedState { get; set; }
    public PlayerNetwork_ElectrocutedState playerElectrocutedState { get; set; }
    public PlayerNetwork_BlownState playerBlownState { get; set; }
    public PlayerNetwork_CursedState playerCursedState { get; set; }
    public PlayerNetwork_LazeredState playerLazeredState { get; set; }
    public PlayerNetwork_StunState playerStunState { get; set; }
    public PlayerNetwork_DeadState playerDeadState { get; set; }
    public PlayerNetwork_DamageKnockDownState playerDamageKnockDownState { get; set; }
    public PlayerNetwork_RevivedState playerRevivedState { get; set; }
    public PlayerNetwork_AwakenedState playerAwakenedState { get; set; }
    public PlayerNetwork_HoverGlideState playerHoverGlideState { get; set; }
    public PlayerNetwork_SlideGlideState playerSlideGlideState { get; set; }
    public PlayerNetwork_NullState playerNullState { get; set; }

    public List<State> playerDamageStates;
    public Dictionary<Vector2, State> playerDynamicDamageStates;
    #endregion

    #region Opponent State Objects
    public Enemy_MoveState opponentMoveState { get; set; }
    public Enemy_IdleState opponentIdleState { get; set; }
    public Enemy_InAirState opponentInAirState { get; set; }
    public Enemy_LandState opponentLandState { get; set; }
    public Enemy_JumpState opponentJumpState { get; set; }
    public Enemy_KnockbackState opponentKnockbackState { get; set; }
    public Enemy_QuickHaltState opponentQuickHaltState { get; set; }
    public Enemy_FullStopState opponentFullStopState { get; set; }
    public Enemy_SlideState opponentSlideState { get; set; }
    public Enemy_ActionState opponentActionState { get; set; }
    public Enemy_ShadowedState opponentShadowedState { get; set; }
    public Enemy_BurningState opponentBurningState { get; set; }
    public Enemy_FrozenState opponentFrozenState { get; set; }
    public Enemy_ChokingState opponentChokingState { get; set; }
    public Enemy_BlindedState opponentBlindedState { get; set; }
    public Enemy_ElectrocutedState opponentElectrocutedState { get; set; }
    public Enemy_BlownState opponentBlownState { get; set; }
    public Enemy_CursedState opponentCursedState { get; set; }
    public Enemy_LazeredState opponentLazeredState { get; set; }
    public Enemy_StunState opponentStunState { get; set; }
    public Enemy_DeadState opponentDeadState { get; set; }
    public Enemy_DamageKnockDownState opponentDamageKnockDownState { get; set; }
    public Enemy_RevivedState opponentRevivedState { get; set; }
    public Enemy_AwakenedState opponentAwakenedState { get; set; }
    public Enemy_HoverGlideState opponentHoverGlideState { get; set; }
    public Enemy_SlideGlideState opponentSlideGlideState { get; set; }
    public Enemy_NullState opponentNullState { get; set; }
    public List<State> opponentDamageStates;
    public Dictionary<Vector2, State> opponentDynamicDamageStates;
    #endregion
    
    public Dictionary<RacerType, List<State>> damageStates;
    public Dictionary<RacerType, Dictionary<Vector2, State>> dynamicDamageStates;

    // The type of racer the object is... is it a player, or an opponent
    public RacerType currentRacerType;
    // The player data and stats holder for the player object, in case its a player racer
    public PlayerData playerData;
    // The opponent data and stats holder for the opponent object, in case its an opponent racer
    public D_DifficultyData difficultyData;
    public AwakenedData awakenedData;
    public PowerupData powerupData;


    public event Action OnLitPlatformChanged;

    protected NetworkIdentity networkIdentity;
    protected LitPlatformHandler litHandler;

    public bool[,] damageMatrix;

    #region State Variables
    // move variables related to the player 
    [Header("Move State")]
    public float moveVelocityResource;
    public float movementVelocity;
    public float normalizedMovementVelocity;
    public float recentNormalizedMovementVelocity;
    public float strengthResource;
    public float strength;
    public float recentStrength;
    public int awakenCount;
    public float normalizedStrength;
    protected float acceleration;
    protected float deceleration;
    protected float brakeRatePerSec;
    public float gravityScaleTemp { get; private set; }

    // jump variables 
    [Header("Jump State")]
    public float jumpVelocityResource;
    public float jumpVelocity;

    public Vector2 slopeNormalPerpendicular { get; set; }

    public bool isOnAnotherLit { get; private set; }
    public bool isOnLit { get; private set; }
    public bool isStayingOnLit { get; private set; }
    public bool hasJustLeftLitplatform { get; private set; }
    public bool canAlterSpeed { get; private set; }
    public bool timeToReset { get; private set; }
    public bool isOnPower { get; private set; }
    public bool isInvulnerable { get; set; } = false;
    public bool canUsePowerup { get; set; } = true;
    public bool isAwakened = false;
    public bool overdrive { get; private set; }
    public bool canAwaken { get; set; }
    public bool canSpawnDust { get; set; } = false;
    public bool isOnSlope { get; set; }
    public bool reducingStrength { get; private set; }
    public bool canWalkOnSlope { get; set; }
    private bool isFastOnPlatform = false;
    private bool isSlowOnPlatform = false;

    public Runner runner;
    #endregion

    public LitPlatformNetwork litPlatform { get; private set; }
    public Collider2D currentPlatform { get; private set; }
    protected PoweredPlatform poweredPlatform;

    public Image testImage;

    public int otherIsOnLitCount { get; private set; }
    public int highestOtherIsOnLitCount { get; private set; } = 0;

   // public Powerup powerup { get; set; } = null;
    public RunnerEffects runnerEffects;
    public VFXConnector VFXConnector;

    #region Components
    public Animator Anim { get; private set; }
    // public GameManager GM { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public CapsuleCollider2D thisCollider { get; private set; }
    public PlayerInputHandlerNetwork InputHandler { get; set; }
    #endregion
    #region Check Variables   
    public Transform GroundCheck;
    public Transform litCheck;
    public Transform projectileArm;
    public Transform bombArm;
    public Checkpoint checkpoint;

    public List<LitPlatformNetwork> litPlatforms = new List<LitPlatformNetwork>();

    private Vector2 colliderSize;
    private float slopeDownAngle;
    private float slopeDownAngleOld;
    private float slopeSideAngle;

    [SerializeField]
    private PhysicsMaterial2D noFriction;
    [SerializeField]
    private PhysicsMaterial2D fullFriction;
    
    [SerializeField]
    private float slopeCheckDistance = .5f;
    [SerializeField]
    private float maxSlopeAngle;
    #endregion
    #region Other Variables
    public Vector2 CurrentVelocity { get; private set; }
    public float currentPosition { get; private set; }
    public float currentPositionPercentage { get; set; }
    public int Rank { get; set; }

    protected Vector2 workspace;
    protected bool isDead;

    public int FacingDirection { get; private set; }
    #endregion
    public GameObject powerupManager { get; set; }
    public PowerupController powerupController { get; private set; }
    public GamePlayerLobby GamePlayer { get; set; }

    public RunnerDamagesOperator myDamages;
    public RunnerFeedbacks runnerFeedbacks;

    #endregion

    #region Functions

    #region Initialization Functions
    public virtual void Awake()
    {
        StateMachine = new FiniteStateMachine();

        if (playerData == null)
            playerData = Resources.Load<PlayerData>("PlayerData");
        Anim = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
        thisCollider = GetComponent<CapsuleCollider2D>();
        litHandler = GetComponent<LitPlatformHandler>();
        runner.player = this;
        runner.stickmanNet = GetComponent<StickmanNet>();
        gravityScaleTemp = RB.gravityScale;
        BlackOverrider.OnBlackOverride += BlackOverrider_OnBlackOverride;
        myDamages.InitDamages();

        playerDynamicDamageStates = new Dictionary<Vector2, State>
        {
            //   {new //Vector2(1,2), /* relationship between fire and ice : 1 and 2 */},
        };


        // relationship between fire and poison gas : 1 and 3
        // relationship between fire and light : 1 and 4
        // relationship between fire and lightning : 1 and 5

    }
    public virtual void BlackOverrider_OnBlackOverride()
    {
        // logic to override if color code is 0 or black
        Debug.Log("Has run onblack override code");
        ChangeStatsWhileOnLit();
    }
    [Client]
    public virtual void Start()
    {
        damageStates = new Dictionary<RacerType, List<State>>
        {
            { RacerType.Player, playerDamageStates },
            {RacerType.Opponent, opponentDamageStates },
        };

        dynamicDamageStates = new Dictionary<RacerType, Dictionary<Vector2, State>>
        {
            {RacerType.Player, playerDynamicDamageStates },
            {RacerType.Opponent, opponentDynamicDamageStates },
        };
        FacingDirection = 1;
        // testImage.gameObject.SetActive(false);
        //  testImage.gameObject.SetActive(false);

        isOnAnotherLit = false;
        overdrive = false;
        canAlterSpeed = false;
        hasJustLeftLitplatform = false;
        colliderSize = thisCollider.size;
        switch (currentRacerType)
        {
            case RacerType.Player:
                awakenCount = playerData.awakenCount;
                break;
            case RacerType.Opponent:
                awakenCount = difficultyData.awakenCount;
                break;
            default:
                break;
        }



        var runnerEffectComp = transform.Find("RunnerEffects").GetComponent<VisualEffect>();
        runnerEffects = new RunnerEffects(runnerEffectComp);

        runnerEffects.runnerVFX.Stop();
    }
    #endregion

    #region Update Functions
    [Client]
    public virtual void Update()
    {
      //  ChangeStatsWhileOnLit();
        ResetOverdrive();
        CheckAwakenStateRequirements();
        if (myDamages.IsDamaged())
        {
            foreach (var damage in myDamages.DamageList())
            {
                Debug.Log("Damage type is: " + damage.damageName);
            }
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            VFXConnector.ChangeVFXProperties(runner.stickmanNet.code);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            runnerEffects.runnerVFX.Play();
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            runnerEffects.runnerVFX.Stop();
        }
        if (awakenedData == null || awakenedData != Resources.Load<AwakenedData>($"{runner.stickmanNet.code}/AwakenedData"))
            awakenedData = Resources.Load<AwakenedData>($"{runner.stickmanNet.code}/AwakenedData");
        if (powerupData == null || powerupData != Resources.Load<PowerupData>($"{runner.stickmanNet.code}/PowerupData"))
            powerupData = Resources.Load<PowerupData>($"{runner.stickmanNet.code}/PowerupData");
        if (powerupController == null)
        {
            if (powerupManager != null)
                powerupController = powerupManager.GetComponent<PowerupController>();
        }
                 

    }
    [Client]
    public virtual void LateUpdate()
    {
        if (!hasAuthority && currentRacerType == RacerType.Player) { return; }
        IncreaseSpeedWhenOtherIsOnLit();
        if (OnLitPlatform() && DetectedLitPlatform() == litPlatform)
        {
            isStayingOnLit = true;
        }
        else
        {
            isStayingOnLit = false;
        }

        isOnPower = OnPowerPlatform();
        CurrentVelocity = RB.velocity;
    }
    public virtual void FixedUpdate()
    {
        if (!hasAuthority && currentRacerType == RacerType.Player) { return; }
        SlopeCheck();
        CheckIfOnAnotherLit();
        CheckOtherPlayersOnLit();
        otherIsOnLitCount = CheckOtherPlayersOnLit();
        CheckHighestOtherOnLitCount();

        currentPosition = transform.position.x;

        #region Collision Effects

        if (OnLitPlatform())
        {
        }
        if (!OnLitPlatform() && litPlatform != null && hasJustLeftLitplatform)
        {
            isOnLit = false;
            hasJustLeftLitplatform = false;
            timeToReset = true;
        }
        if (timeToReset && !isOnLit)
            Reseter();
        #endregion

        #region Resolve animation curves

        // determine if the racer is a player or opponent to assign their respective variables
        switch (currentRacerType)
        {
            case RacerType.Player:
                normalizedStrength = strength / playerData.maxStrength;
                normalizedMovementVelocity = CurrentVelocity.x / playerData.topSpeed;
                break;
            case RacerType.Opponent:
                normalizedStrength = strength / difficultyData.maxStrength;
                normalizedMovementVelocity = CurrentVelocity.x / difficultyData.topSpeed;
                break;
            default:
                break;
        }
        
        if (myDamages.IsDamaged())
            StartCoroutine(DamageEffectsOnStrength());
        if (!(normalizedStrength >= 1) && reducingStrength)
        {
            //var newMoveVelocityNormalized = playerData.strengthToTopSpeedCurve.Evaluate(normalizedStrength);
            //moveVelocityResource = newMoveVelocityNormalized * playerData.topSpeed;
        }
        //   var newJumpVelocityNormalized = playerData.speedToJumpVelocityCurve.Evaluate(normalizedMovementVelocity);
        //   jumpVelocity = newJumpVelocityNormalized * playerData.maxJumpVelocity;
        switch (currentRacerType)
        {
            case RacerType.Player:
                if (normalizedMovementVelocity > 1)
                {
                    jumpVelocity = 1f * playerData.maxJumpVelocity; // was 1.25f
                }
                // In case we want variable jump velocities relating to the runner's x veloicty, uncomment this

                //else
                //{
                //    var flooredMoveVelocity = Mathf.Floor(normalizedMovementVelocity);
                //    var afterDecimal = normalizedMovementVelocity - flooredMoveVelocity;
                //    var newJumpVelocityNormalized2 = playerData.speedToJumpVelocityCurve.Evaluate(afterDecimal);
                //    jumpVelocity = (newJumpVelocityNormalized2 + flooredMoveVelocity) * playerData.maxJumpVelocity;
                //}
                break;
            case RacerType.Opponent:
                if (normalizedMovementVelocity > 1)
                {
                    jumpVelocity = 1f * difficultyData.maxJumpVelocity; // was 1.25f
                }
                break;
        }
        #endregion
    }

    #endregion

    #region Set Functions
    /// <summary>
    /// Function to set the velocity of the rigidbody2d component directly, on the x-axis.
    /// PS: In my experience, this disables (to a degree) unity physics as the variables are manually set. But I believe that should be so for the purpose of this game.
    /// </summary>
    /// <param name="velocity"></param>
    public virtual void SetVelocityX(float velocity)
    {
          workspace.Set(velocity, CurrentVelocity.y);
          RB.velocity= workspace;
        //  if (CurrentVelocity.x >= playerData.topSpeed)
        RB.velocity = workspace;

        CurrentVelocity = RB.velocity;
    }
    /// <summary>
    /// Function to set the velocity of the rigidbody2d component directly, on the y-axis.
    /// PS: This way of doing jumping should be cool for 2d games since we want to jump instantaneously when we deliver input.
    /// </summary>
    /// <param name="velocity"></param>
    public virtual void SetVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity);
        RB.velocity = workspace;
        CurrentVelocity = RB.velocity;
    }
    /// <summary>
    /// Function to accelerate the runner till he reaches his top speed.
    /// </summary>
    /// <param name="racerType">The current type of player: Player or Opponent</param>
    public virtual void SetAccelerations(RacerType racerType)
    {
        switch (racerType)
        {
            case RacerType.Player:
                movementVelocity += acceleration * Time.deltaTime;
                //           jumpVelocity += accelRatePerSec * Time.deltaTime;
                float knockbackAccelRate = acceleration;
                playerData.knockbackVelocity.x += knockbackAccelRate * Time.deltaTime;
                playerData.knockbackVelocity.y += knockbackAccelRate * Time.deltaTime;

                playerData.knockbackVelocity.x = Mathf.Min(playerData.knockbackVelocity.x, playerData.maxKnockbackVelocity.x);
                playerData.knockbackVelocity.y = Mathf.Min(playerData.knockbackVelocity.y, playerData.maxKnockbackVelocity.y);
                movementVelocity = Mathf.Min(movementVelocity, moveVelocityResource);
                //            jumpVelocity = Mathf.Min(jumpVelocity, jumpVelocityResource);
                break;
            case RacerType.Opponent:
                movementVelocity += acceleration * Time.deltaTime;
                //            jumpVelocity += accelRatePerSec * Time.deltaTime;
                float knockbackAccelRateO = acceleration * 0.667f;
                difficultyData.knockbackVelocity.x += knockbackAccelRateO * Time.deltaTime;
                difficultyData.knockbackVelocity.y += knockbackAccelRateO * Time.deltaTime;

                difficultyData.knockbackVelocity.x = Mathf.Min(difficultyData.knockbackVelocity.x, difficultyData.maxKnockbackVelocity.x);
                difficultyData.knockbackVelocity.y = Mathf.Min(difficultyData.knockbackVelocity.y, difficultyData.maxKnockbackVelocity.y);
                movementVelocity = Mathf.Min(movementVelocity, moveVelocityResource);
                //            jumpVelocity = Mathf.Min(jumpVelocity, jumpVelocityResource);
                break;
            default:
                break;
        }



    }
    /// <summary>
    /// Function to decelerate a runner, under given conditions, till he reaches 0 velocity.
    /// </summary>
    public virtual void SetDecelerations()
    {
        movementVelocity += deceleration * Time.deltaTime;

        movementVelocity = Mathf.Max(movementVelocity, 0);

    }
    /// <summary>
    /// Function to bring a runner to a halt faster than the decelerate function.
    /// </summary>
    public virtual void SetBrakes()
    {
        movementVelocity += brakeRatePerSec * Time.deltaTime;
        //    jumpVelocity = 0f;
        movementVelocity = Mathf.Max(movementVelocity, 0);
    }
    /// <summary>
    /// Function to bring a runner to a halt instantly.
    /// </summary>
    public virtual void SetStop()
    {
        movementVelocity = 0f;
        //    jumpVelocity = 0f;
    }
    #endregion

    #region Collision Functions
    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (OnLitPlatform())
        {
            if (other.collider.CompareTag("LitPlatform"))
            {
                currentPlatform = other.collider;

                if (!isOnLit)
                {
                    timeToReset = false;
                    canAlterSpeed = true;
                    hasJustLeftLitplatform = false;
                    //   if (!hasAuthority) { return; }
                    litPlatform = other.gameObject.GetComponent<LitPlatformNetwork>();

                    OnLitPlatformChanged?.Invoke();

                    networkIdentity = other.gameObject.GetComponent<NetworkIdentity>();

                    // try to light up the platform
                    if (!litPlatform.isLit || runner.stickmanNet.currentColor.colorID == 0)
                    {
                        if (GetComponent<NetworkIdentity>().hasAuthority || currentRacerType == RacerType.Opponent)
                        {

                            litHandler.UpdateColor();
                            Debug.Log("Trying to run Update Color Logic!");
                            litPlatform.colorStateCode.colorID = runner.stickmanNet.currentColor.colorID;
                            litPlatform.isLit = true;
                            // litPlatform.colorStateCode.colorID = runner.stickmanNet.currentColor.colorID;
                            //  litHandler.litPlatform.colorStateCode.colorID = runner.stickmanNet.currentColor.colorID;
                        }


                    }
                   
                    // add litplatform to list of your own litplatforms
                    if (!litPlatforms.Contains(litPlatform) && litPlatform.colorStateCode.colorID == runner.stickmanNet.currentColor.colorID)
                    {
                        litPlatforms.Add(litPlatform);

                    }
                    // if we are the dark runner, override the former color to ours
                    if (runner.stickmanNet.currentColor.colorID == 0)
                    {
                        BlackOverrider.instance.Override();
                    }
                    isOnLit = true;

                    // speed up or slow down
                    WaitToChangeStats(litPlatform);
                }

            }
        }

        if (other.collider.CompareTag("PowerPlatform"))
        {
            currentPlatform = other.collider;
            isOnPower = true;
            //     testImage.gameObject.SetActive(true); 
            poweredPlatform = other.gameObject.GetComponent<PoweredPlatform>();

        }
        if (other.collider.CompareTag("BasePlatform"))
        {
            currentPlatform = other.collider;
        }
    }
    public virtual void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.CompareTag("LitPlatform"))
        {
           
        }
    }
    public virtual void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("LitPlatform"))
        {
            currentPlatform = null;
            hasJustLeftLitplatform = true;
        }
        if (other.collider.CompareTag("PowerPlatform"))
        {
            currentPlatform = null;
        }
        if (other.collider.CompareTag("BasePlatform"))
        {
            currentPlatform = null;
        }
    }

    public void PowerTriggerEnter(PoweredPlatform poweredPlatform)
    {
        isOnPower = true;

        this.poweredPlatform = poweredPlatform;
    }
    public void PowerTriggerExit(PoweredPlatform poweredPlatform)
    {
        isOnPower = false;
    }
    #endregion

    #region Check Functions
    public virtual void CheckAwakenStateRequirements()
    {
        switch (currentRacerType)
        {
            case RacerType.Player:
                if (litPlatforms.Count >= playerData.requiredLitPlatformsToAwaken && awakenCount > 0)
                {
                    canAwaken = true;
                }
                break;
            case RacerType.Opponent:
                if (litPlatforms.Count >= difficultyData.requiredLitPlatformsToAwaken && awakenCount > 0)
                {
                    canAwaken = true;
                }
                break;
            default:
                break;
        }
    }
    public virtual void WaitToChangeStats(LitPlatformNetwork litPlatform)
    {
        if (litPlatform.isLit)
        {
            if (runner.stickmanNet.currentColor.colorID == litPlatform.colorStateCode.colorID)
            {
                SpeedUp();
            }
            else
            {
                SlowDown();
            }
        }
    }
    public virtual void Flip()
    {
        FacingDirection *= -1;
        transform.Rotate(0f, 180f, 0f);
    }
    public virtual bool CheckIfGrounded()
    {
        return Physics2D.OverlapCircle(GroundCheck.position, playerData.groundCheckRadius, playerData.whatIsGround);
    }
    private RaycastHit2D LitPlatformRaycast()
    {
        Debug.DrawRay(transform.position, Vector2.down * playerData.litCheckDistance, Color.green);
        return Physics2D.Raycast(litCheck.position, Vector2.down, playerData.litCheckDistance, playerData.whatIsLit);
    }
    public virtual bool OnLitPlatform()
    {
        return LitPlatformRaycast();
    }
    public LitPlatformNetwork DetectedLitPlatform()
    {
        RaycastHit2D hit = LitPlatformRaycast();
        litPlatform = hit.collider.GetComponent<LitPlatformNetwork>();
        Debug.Log(hit.collider.name);
        return litPlatform;
    }
    public virtual bool OnPowerPlatform()
    {
        return Physics2D.Raycast(litCheck.position, Vector2.down, playerData.powerCheckDistance, playerData.whatIsPower);
    }
    public virtual void SlopeCheck()
    {
        Vector2 checkPos = transform.position - new Vector3(0f, colliderSize.y / 2, 0f);

        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }
    public virtual void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D hitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, playerData.whatIsSlope);
        RaycastHit2D hitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, playerData.whatIsSlope);

        if (hitFront)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(hitFront.normal, Vector2.up);
        }
        else if (hitBack)
        {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(hitBack.normal, Vector2.up);
        }
        else
        {
            slopeSideAngle = 0f;
            isOnSlope = false;
        }
    }
    public virtual void SlopeCheckVertical(Vector2 checkPos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, playerData.whatIsSlope);

        if (hit)
        {
            slopeNormalPerpendicular = Vector2.Perpendicular(hit.normal).normalized;
            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if(slopeDownAngle != slopeDownAngleOld && slopeDownAngle != 0)
                isOnSlope = true;
            

            slopeDownAngleOld = slopeDownAngle;

            

            Debug.DrawRay(hit.point, slopeNormalPerpendicular, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }

        if(slopeDownAngle > maxSlopeAngle)
        {
            canWalkOnSlope = false;
        }
        else
        {
            canWalkOnSlope = true;
        }
        if (isOnSlope)
        {
            transform.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
        }
        
        // For sticking to platform when not moving
        //if (isOnSlope)
        //    RB.sharedMaterial = fullFriction;
        //else
        //    RB.sharedMaterial = noFriction;
    }
    public virtual void Reseter()
    {
        ResetStats();
    }


    #endregion

    #region LitPlatform Functions
    public virtual void SpeedUp()
    {
        switch (currentRacerType)
        {
            case RacerType.Player:
             //   moveVelocityResource = Utils.PercentageIncreaseOrDecrease(playerData.topSpeed, playerData.litSpeedUpPercentage, Utils.AlterType.Increase);
               moveVelocityResource += Utils.PercentageValue(playerData.topSpeed, playerData.litSpeedUpPercentage);
                break;
            case RacerType.Opponent:
              //  moveVelocityResource = Utils.PercentageIncreaseOrDecrease(playerData.topSpeed, difficultyData.litSpeedUpPercentage, Utils.AlterType.Increase);
                moveVelocityResource += Utils.PercentageValue(difficultyData.topSpeed, difficultyData.litSpeedUpPercentage);
                break;
            default:
                break;
        }
        isFastOnPlatform = true;
        isSlowOnPlatform = false;
    }
    public virtual void SlowDown()
    {
        switch (currentRacerType)
        {
            case RacerType.Player:
            //    moveVelocityResource = Utils.PercentageIncreaseOrDecrease(playerData.topSpeed, playerData.litSlowDownPercentage, Utils.AlterType.Decrease);
                moveVelocityResource -= Utils.PercentageValue(playerData.topSpeed, playerData.litSlowDownPercentage);
                break;
            case RacerType.Opponent:
             //   moveVelocityResource = Utils.PercentageIncreaseOrDecrease(playerData.topSpeed, difficultyData.litSlowDownPercentage, Utils.AlterType.Decrease);
                moveVelocityResource -= Utils.PercentageValue(difficultyData.topSpeed, difficultyData.litSlowDownPercentage);
                break;
            default:
                break;
        }
        isFastOnPlatform = false;
        isSlowOnPlatform = true;
    }
    private void ResetSpeedBools()
    {
        if (!isOnLit)
        {
            isFastOnPlatform = false;
            isSlowOnPlatform = false;
        }
    }
    public virtual void ResetStats()
    {
      //  float newMoveVelocity = 0;
        switch (currentRacerType)
        {
            case RacerType.Player:
                if (isSlowOnPlatform)
                {
                    //newMoveVelocity = moveVelocityResource + Utils.PercentageValue(playerData.topSpeed, playerData.litSlowDownPercentage);
                    //moveVelocityResource += playerData.litSpeedAlterRate * Time.deltaTime;
                    //moveVelocityResource = Mathf.Min(moveVelocityResource, newMoveVelocity);
                    moveVelocityResource += Utils.PercentageValue(playerData.topSpeed, playerData.litSlowDownPercentage);
                }
                else if (isFastOnPlatform)
                {
                    //newMoveVelocity = moveVelocityResource - Utils.PercentageValue(playerData.topSpeed, playerData.litSpeedUpPercentage);
                    //moveVelocityResource -= playerData.litSpeedAlterRate * Time.deltaTime;
                    //moveVelocityResource = Mathf.Max(moveVelocityResource, newMoveVelocity);
                    moveVelocityResource -= Utils.PercentageValue(playerData.topSpeed, playerData.litSpeedUpPercentage);
                }
             //   if(moveVelocityResource == newMoveVelocity)
                {
                    ResetSpeedBools();
                }
                jumpVelocityResource = playerData.maxJumpVelocity;
                break;
            case RacerType.Opponent:
                if (isSlowOnPlatform)
                {
                    moveVelocityResource += Utils.PercentageValue(difficultyData.topSpeed, difficultyData.litSlowDownPercentage);
                }
                else if (isFastOnPlatform)
                {
                    moveVelocityResource -= Utils.PercentageValue(difficultyData.topSpeed, difficultyData.litSpeedUpPercentage);

                }
                
                {
                    timeToReset = false;
                }
                jumpVelocityResource = difficultyData.maxJumpVelocity;
                break;
            default:
                break;
        }
    }
    public virtual int CheckOtherPlayersOnLit()
    {
        int amount = 0;
        foreach (LitPlatformNetwork litPlatform in litPlatforms)
        {
            if (litPlatform.otherIsOnLit && litPlatform.colorStateCode.colorID == runner.stickmanNet.currentColor.colorID)
            {
                amount++;
            }
        }
        return amount;
    }
    public virtual void CheckHighestOtherOnLitCount()
    {
        if (otherIsOnLitCount > highestOtherIsOnLitCount)
        {
            highestOtherIsOnLitCount = otherIsOnLitCount;
        }
    }
    public virtual void CheckIfOnAnotherLit()
    {
        if (isOnLit && litPlatform.colorStateCode.colorID != runner.stickmanNet.currentColor.colorID)
        {
            isOnAnotherLit = true;
        }
        else if ((isOnLit && litPlatform.colorStateCode.colorID != runner.stickmanNet.currentColor.colorID) || !isOnLit)
        {
            isOnAnotherLit = false;
        }
    }
    public virtual void IncreaseSpeedWhenOtherIsOnLit()
    {
        if (otherIsOnLitCount != 0 && isOnLit)
        {
            moveVelocityResource += playerData.otherOnLitIncreaseValue * otherIsOnLitCount;
            overdrive = true;
        }
        else if (otherIsOnLitCount != 0 && !isOnLit)
        {
            moveVelocityResource += playerData.otherOnLitIncreaseValue * otherIsOnLitCount;
            overdrive = false;
        }
    }
    // FUNCTION: Used to change the stats of the player while on lit. Especially if the runner has already collided with the litplatform
    public virtual void ChangeStatsWhileOnLit()
    {

        if (isOnLit)
        {

            WaitToChangeStats(litPlatform);
            Debug.Log("Has run change stats while on lit function!");
        }
    }
    public virtual void ResetOverdrive()
    {
        if (!isOnLit)
            overdrive = false;
    }
    public virtual int LitPlatformCount()
    {
        return litPlatforms.Count;
    }
    #endregion

    #region Graphical Functions
    public virtual void SetStrengthBarValue(Slider slider)
    {
        slider.value = normalizedStrength;
    }
    #endregion
    
    #region Damage Functions
    /// <summary>
    /// The function that gets called when damage occurs. The runner damages operator struct holds the neccessary information to deal appropriate damage.
    /// </summary>
    /// <param name="runnerDamages">The struct that holds all neccessary damage information.</param>
    public virtual void DamageRunner(RunnerDamagesOperator runnerDamages)
    {
        //completely ignore damaging if is invulnerable or damage type is identical to our color
        if (isInvulnerable || runnerDamages.DamageList()[0].damageInt == runner.stickmanNet.currentColor.colorID) { return; }

        for (int i = 0; i < powerupController.activePowerups.Count; i++)
        {
            switch (powerupController.activePowerups[i].powerup.powerupID)
            {
                case Powerup.PowerupID.SpeedUp:
                    powerupController.TurnDurationTo0(powerupController.activePowerups[i]);
                    continue;
                case Powerup.PowerupID.ElementField:
                    powerupController.TurnDurationTo0(powerupController.activePowerups[i]);
                    continue;
                case Powerup.PowerupID.Projectile:
                    // TODO: change to disable powerup selected nature
                    continue;
                case Powerup.PowerupID.Bomb:
                    // TODO: change to disable powerup selected nature
                    continue;
            }
            
        }
        //check if the runner is previously damaged, and if so do damage again, this time instantly depleting the runner's stamina and knocking him out
        if (myDamages.IsDamaged())
        {
            foreach (var damage in runnerDamages.DamageList())
            {
                DoDynamicDamage(runnerDamages, damage);
                Debug.Log("Do Dynamic Damage has run!");
            }
        }
        // if is not previously damaged, this is new damage
        else
        {
            foreach (var damage in runnerDamages.DamageList())
            {
                // change to the appropirate damage state
                ChangeToDamageState(runnerDamages, damage, damageStates[currentRacerType][runnerDamages.Damages.IndexOf(damage)]);
            }
        }
    }
    /// <summary>
    /// Recovers the runner from damage effects, replenishes strength and puts them in a revived state.
    /// </summary>
    public virtual void Recover()
    {
        RestoreStrength();
        RemoveDamage();
    }
    /// <summary>
    /// Replenishes runner's strength.
    /// </summary>
    public virtual void RestoreStrength()
    {
        switch (currentRacerType)
        {
            case RacerType.Player:
                strengthResource = playerData.maxStrength;
                strength = playerData.maxStrength;
                break;
            case RacerType.Opponent:
                strengthResource = difficultyData.maxStrength;
                strength = difficultyData.maxStrength;
                break;
            default:
                break;
        }
    }
    public virtual IEnumerator DamageEffectsOnStrength()
    {
        float finalStrength = 0;
        foreach (var damage in myDamages.DamageList())
        {
            switch (currentRacerType)
            {
                case RacerType.Player:
                    finalStrength = recentStrength - Utils.PercentageValue(playerData.maxStrength, damage.damagePercentage);
                    Debug.Log($"Final strength is: {finalStrength}");
                    break;
                case RacerType.Opponent:
                    finalStrength = recentStrength - Utils.PercentageValue(difficultyData.maxStrength, damage.damagePercentage);
                    break;
                default:
                    break;
            }
            if (strength > finalStrength)
            {
                strength -= damage.damageRate * (Time.deltaTime /* / 5*/ );
                float newMoveVelocityNormalized = 0;
                switch (currentRacerType)
                {
                    case RacerType.Player:
                        normalizedStrength = strength / playerData.maxStrength;
                        newMoveVelocityNormalized = playerData.strengthToTopSpeedCurve.Evaluate(normalizedStrength);
                        moveVelocityResource += recentNormalizedMovementVelocity * playerData.topSpeed;
                        recentNormalizedMovementVelocity = 1 - newMoveVelocityNormalized;
                        moveVelocityResource -= recentNormalizedMovementVelocity * playerData.topSpeed;
                        break;
                    case RacerType.Opponent:
                        normalizedStrength = strength / difficultyData.maxStrength;
                        newMoveVelocityNormalized = difficultyData.strengthToTopSpeedCurve.Evaluate(normalizedStrength);
                        moveVelocityResource += recentNormalizedMovementVelocity * difficultyData.topSpeed;
                        recentNormalizedMovementVelocity = 1 - newMoveVelocityNormalized;
                        moveVelocityResource -= recentNormalizedMovementVelocity * difficultyData.topSpeed;
                        break;
                    default:
                        break;
                }
                
                reducingStrength = strength > finalStrength;
            }

            if (strength <= 0)
                strength = Mathf.Max(strength, 0);
            else
                strength = Mathf.Max(strength, finalStrength);
        }
        if (strength <= 0)
        {
            movementVelocity = 0;
            yield return new WaitForSeconds(3f);
            switch (currentRacerType)
            {
                case RacerType.Player:
                    StateMachine.ChangeDamagedState(playerNullState);
                    StateMachine.ChangeState(playerDamageKnockDownState);
                    break;
                case RacerType.Opponent:
                    StateMachine.ChangeDamagedState(opponentNullState);
                    StateMachine.ChangeState(opponentDamageKnockDownState);
                    break;
                default:
                    break;
            }
        }
        else if(strength <= finalStrength)
        {
            switch (currentRacerType)
            {
                case RacerType.Player:
                    StateMachine.ChangeDamagedState(playerNullState);
                    RemoveDamage();
                    break;
                case RacerType.Opponent:
                    StateMachine.ChangeDamagedState(opponentNullState);
                    RemoveDamage();
                    break;
                default:
                    break;
            }
        }
    }
    public virtual void ChangeToDamageState(RunnerDamagesOperator runnerDamages, DamageForm damage, State stateToChangeTo)
    {
        var index = runnerDamages.Damages.IndexOf(damage);
        var newDamage = myDamages.Damages[index];
        newDamage.damageInt = damage.damageInt;
        newDamage.damageName = damage.damageName;
        newDamage.damagerType = damage.damagerType;
        newDamage.damagePercentage = damage.damagePercentage;
        newDamage.damageRate = damage.damageRate;
        newDamage.damaged = true;
        reducingStrength = true;
        if (damage.racer != null)
        {
            newDamage.racer = damage.racer;
            if ((damage.racer.StateMachine.AwakenedState == damage.racer.playerAwakenedState || damage.racer.StateMachine.AwakenedState == damage.racer.opponentAwakenedState) && GameManager.instance.currentLevel.buttonMap == damage.racer.runner.stickmanNet.currentColor.colorID)
                newDamage.ultimateDamage = true;
            else if (damage.racer.StateMachine.AwakenedState == damage.racer.playerAwakenedState || damage.racer.StateMachine.AwakenedState == damage.racer.opponentAwakenedState || GameManager.instance.currentLevel.buttonMap == damage.racer.runner.stickmanNet.currentColor.colorID)
                newDamage.hyperDamage = true;
        }

        //   StartCoroutine(DamageEffectsOnStrength());
        recentStrength = strength;
        StateMachine.ChangeDamagedState(stateToChangeTo);
    }
    /// <summary>
    /// Resets damage data in the "myDamages" struct.
    /// </summary>
    public virtual void RemoveDamage()
    {
      //  Debug.Log($"Damage count is: {myDamages.DamageList().Count}");

        if (myDamages.IsDamaged())
        {
            foreach (var myDamage in myDamages.DamageList())
            {
                myDamage.damaged = false;
                myDamage.damageInt = 8;
                myDamage.damagePercentage = 0;
                myDamage.hyperDamage = false;
                myDamage.ultimateDamage = false;
                myDamage.racer = null;
            }
            recentStrength = strength;
        }
    }
    public virtual void DoDynamicDamage(RunnerDamagesOperator runnerDamages, DamageForm damage)
    {
        var index = runnerDamages.Damages.IndexOf(damage);
        var myNewDamage = myDamages.Damages[index];
        myNewDamage.damaged = true;
        myNewDamage.damagePercentage = damage.damagePercentage;

        foreach (var previousDamage in myDamages.DamageList())
        {
            foreach (var newDamage in runnerDamages.DamageList())
            {
                DynamicDamage(runnerDamages, previousDamage, newDamage);
                Debug.Log("Dynamic Damage has run!");
            }
        }

    }
    void CheckBoolArray(bool[,] boolArray, State stateToChangeTo)
    {
        for (int x = 0; x < 11; x++)
        {
            for (int y = 0; y < 11; y++)
            {
                if (boolArray[x, y])
                {
                    StateMachine.ChangeDamagedState(stateToChangeTo);

                }
            }
        }
    }
    public virtual void StrengthBreak()
    {
        strength = 0f;
        // play guage break animation
        switch (currentRacerType)
        {
            case RacerType.Player:
                StateMachine.ChangeState(playerDamageKnockDownState);
                break;
            case RacerType.Opponent:
                StateMachine.ChangeState(opponentDamageKnockDownState);
                break;
            default:
                break;
        }
    }
    void DynamicDamage(RunnerDamagesOperator runnerDamages, DamageForm previousDamage, DamageForm newDamage)
    {
        // get the damage index to identify the respective damages
        var previousDamageIndex = myDamages.Damages.IndexOf(previousDamage);
        var newDamageIndex = runnerDamages.Damages.IndexOf(newDamage);

        // create a matrix to determine the relationship between the damages
        bool[,] damageMatrix = new bool[10, 10];

        damageMatrix[previousDamageIndex, newDamageIndex] = true;
        StrengthBreak();
        //   CheckBoolArray(damageMatrix, dynamicDamageStates[currentRacerType][new Vector2(previousDamageIndex, newDamageIndex)]);
    }
    public virtual GameObject InstantiateObject(GameObject original, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject entity = Instantiate(original, position, rotation, parent);
        return entity;
    }
    #endregion

    #region Awaken Functions
    /// <summary>
    /// Puts the runner in an awakened state, where the runner is allowed to use his base power more freely.
    /// </summary>
    /// <param name="colorCode">The color signature of the runner.</param>
    public void Awaken(int colorCode)
    {
        // if the runner is previously damaged, remove the damage
        if (myDamages.IsDamaged())
        {
            Recover();
        }

        switch (currentRacerType)
        {
            case RacerType.Player:
                StateMachine.ChangeAwakenedState(playerAwakenedState);
                break;
            case RacerType.Opponent:
                StateMachine.ChangeAwakenedState(opponentAwakenedState);
                break;
        }
        awakenCount--;
        canAwaken = false;
    }
    public void ActivateAwakenEffects()
    {
        var powerupData = Resources.Load<PowerupData>($"{runner.stickmanNet.currentColor.colorID}/PowerupData");
        var awakenedData = Resources.Load<AwakenedData>($"{runner.stickmanNet.currentColor.colorID}/AwakenedData");

        // vfx open
        VFXConnector.ChangeVFXProperties(runner.stickmanNet.code);
        runnerEffects.runnerVFX.Play();

        //instantiate damage prefab
        var damageEffectPrefab = Resources.Load<GameObject>($"{runner.stickmanNet.currentColor.colorID}/DamageEffect");
        var damageEffect = Instantiate(damageEffectPrefab, transform.position, Quaternion.identity, transform);
        
        //instantiate explosion effect
        var explosionEffectPrefab = Resources.Load<GameObject>($"{runner.stickmanNet.currentColor.colorID}/Explosion");
        var explosionEffect = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity, transform);

        switch (runner.stickmanNet.currentColor.colorID)
        {
            case 1:
                
                break;
            default:
                break;
        }
        var explosionScript = explosionEffect.GetComponent<ElementExplosionScript>();


        // assign the variables
        Utils.SetExplosionVariables(this, explosionScript, runner.stickmanNet.currentColor.colorID, powerupData, awakenedData);

        // initialize and do damage
        explosionScript.runnerDamages.InitDamages();
        explosionScript.Explode(true);
    }
    public void SetAbilityActive()
    {
        switch (currentRacerType)
        {
            case RacerType.Player:
                playerAwakenedState.canUseAbility = true;
                break;
            case RacerType.Opponent:
                opponentAwakenedState.canUseAbility = true;
                break;
            default:
                break;
        }
    }
    private void OnParticleCollision(GameObject other)
    {

    }

    // the players abilities are kept here but these are called in update
    public void AwakenedGimmick(int colorCode)
    {
        switch (colorCode)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                break;
            case 3:
                break;
            case 4:
                break;
            case 5:
                break;
            case 6:
                break;
            case 7:
                break;
            default:
                break;
        }
    }

    // the players abilities are here but these are called once, when the players enters the awakened state
    public void AwakenedAbility(int colorCode)
    {
        switch (colorCode)
        {
            case 0:
                break;
            case 1:
                var speedBurstGO = Resources.Load<GameObject>($"{colorCode}/SpeedBurst");
                var speedBurstInGame = Instantiate(speedBurstGO, transform.position, speedBurstGO.transform.rotation);

                RB.AddForce(new Vector2(20f, 0f), ForceMode2D.Impulse);
                moveVelocityResource += Utils.PercentageValue(playerData.topSpeed, awakenedData.redSpeedIncreasePercentage);
                movementVelocity = moveVelocityResource;
                break;
            case 2:
                switch (currentRacerType)
                {
                    case RacerType.Player:
                        StateMachine.ChangeAwakenedState(playerSlideGlideState);
                        break;
                    case RacerType.Opponent:
                        StateMachine.ChangeAwakenedState(opponentSlideGlideState);
                        break;
                    default:
                        break;
                }
                var iceBoardGO = Resources.Load<GameObject>($"{colorCode}/IceBoard");
                GameObject iceBoardInGame = null;
          //      if(!transform.Find("IceBoard(Clone)"))
                    iceBoardInGame = Instantiate(iceBoardGO, GroundCheck.position, Quaternion.identity, transform);

                moveVelocityResource += Utils.PercentageValue(playerData.topSpeed, awakenedData.blueSpeedIncreasePercentage);

                break;
            case 3:
                break;
            case 4:
                var trailGO = Resources.Load<GameObject>("TrailEffect");
                var trailEffectGO = Instantiate(trailGO, transform.position, Quaternion.identity);
                var trailEffect = trailEffectGO.GetComponent<TrailRenderer>();

                Transform oldPosition = transform;
                transform.position = new Vector3(transform.position.x + Utils.PercentageValue(transform.position.x, awakenedData.yellowTeleportationPositionXPercentage), transform.position.y, transform.position.z);
                Transform newPosition = transform;

                

              //  trailEffect.AddPosition(oldPosition.position);
                trailEffect.AddPosition(newPosition.position);
                break;
            case 5:
                var orbGO = Resources.Load<GameObject>($"{colorCode}/Orb");
                var orbInGame = Instantiate(orbGO, transform.position, Quaternion.identity, transform);
                var orbScript = orbInGame.GetComponent<OrbController>();

                orbScript.orbLifetime = awakenedData.purpleOrbLifeTime;
                orbScript.ownerRacer = this;
                orbScript.orbSpeed = awakenedData.purpleOrbSpeed;
                orbScript.shockRadius =awakenedData.purpleOrbRadius;
                orbScript.damageType = colorCode;
                orbScript.whatToDamage = awakenedData.purpleWhatToHit;
                orbScript.beamProjectile = Resources.Load<GameObject>($"{colorCode}/Beam");

                // spawn electric orb
                // set its variables

                // give it active time
                break;
            case 6:
                var effect = Resources.Load<GameObject>($"{runner.stickmanNet.currentColor.colorID}/ElementField");
                var effectInGame = Instantiate(effect, transform.Find("PlayerCenter").position, Quaternion.identity, transform);
                break;
            case 7:
                break;
            default:
                break;
        }
    }
    #endregion

    #region Other Functions
    public enum RacerType
    {
        Player,
        Opponent
    }
    public virtual void DestroyObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }
    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, powerupData.mineExplosiveRadius);
        switch (currentRacerType)
        {
            case RacerType.Player:
                Gizmos.DrawWireSphere(GroundCheck.position, playerData.groundCheckRadius);
                Gizmos.DrawWireSphere(transform.Find("PlayerCenter").position, playerData.powerupRadius);
                Gizmos.DrawLine(litCheck.position, litCheck.position + (Vector3)(Vector2.down * playerData.litCheckDistance));
                Gizmos.DrawLine(litCheck.position, litCheck.position + (Vector3)(Vector2.down * playerData.powerCheckDistance));
                break;
            case RacerType.Opponent:
                Gizmos.DrawWireSphere(GroundCheck.position, difficultyData.groundCheckRadius);
                Gizmos.DrawWireSphere(transform.Find("PlayerCenter").position, difficultyData.powerupRadius);
                Gizmos.DrawLine(litCheck.position, litCheck.position + (Vector3)(Vector2.down * difficultyData.litCheckDistance));
                Gizmos.DrawLine(litCheck.position, litCheck.position + (Vector3)(Vector2.down * difficultyData.powerCheckDistance));
                break;
            default:
                break;
        }

    }
    #endregion

    #endregion
}

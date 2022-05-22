using DapperDino.Mirror.Tutorials.Lobby;
using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public PlayerNetwork_ReadyingState playerReadyingState { get; set; }
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
    public PlayerNetwork_ChokingState PlayerChokingState { get; set; }
    public PlayerNetwork_BlindedState PlayerBlindedState { get; set; }
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
    public PlayerNetwork_WinState playerWinState { get; set; }
    public PlayerNetwork_LoseState playerLoseState { get; set; }
    public List<State> playerDamageStates;
    public Dictionary<Vector2, State> playerDynamicDamageStates;
    #endregion

    #region Opponent State Objects
    public Enemy_ReadyingState opponentReadyingState { get; set; }
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
    public Enemy_WinState opponentWinState { get; set; }
    public Enemy_LoseState opponentLoseState { get; set; }

    public List<State> opponentDamageStates;
    public Dictionary<Vector2, State> opponentDynamicDamageStates;
    #endregion

    #region Runner Data
    // The type of racer the object is... is it a player, or an opponent
    public RacerType currentRacerType;
    // The player data and stats holder for the player object, in case its a player racer
    public PlayerData playerData;
    // The opponent data and stats holder for the opponent object, in case its an opponent racer
    public D_DifficultyData difficultyData;

    public RacerData racerData;

    public AwakenedData awakenedData;
    public PowerupData powerupData;
    #endregion

    #region Platforms
    public LitPlatformNetwork litPlatform { get; protected set; }
    public PoweredPlatform poweredPlatform { get; protected set; }
    public Collider2D currentPlatform { get; protected set; }
    #endregion

    public Dictionary<RacerType, List<State>> damageStates;
    public Dictionary<RacerType, Dictionary<Vector2, State>> dynamicDamageStates;


    public event Action OnLitPlatformChanged;


    #region State Variables/Properties
    // move variables 
    [Header("Move State")]
    public float moveVelocityResource;
    public float movementVelocity;
    public float normalizedMovementVelocity;
    public bool burstSpeed;
    public float strength;
    public float recentStrength;
    public float previousStrengthNormalized;
    public float normalizedStrength;
    protected float acceleration;
    protected float deceleration;
    protected float brakeRatePerSec;

    // jump variables 
    [Header("Jump State")]
    public float jumpVelocityResource;
    public float jumpVelocity;
    public int amountOfJumps;

    public Vector2 slopeNormalPerpendicular { get; set; }

    public bool isOnAnotherLit { get; private set; }
    public bool isOnLit { get; private set; }
    public bool hasJustLeftLitplatform { get; private set; }
    public bool timeToReset { get; private set; }
    public bool isOnPower { get; private set; }
    public bool isInvulnerable { get; set; } = false;
    public bool canUsePowerup { get; set; } = true;
    public bool isAwakened = false;
    public bool isInNativeMap = false;
    public bool drive { get; private set; }
    public bool overdrive { get; private set; }

    public bool canSpawnDust { get; set; } = false;
    public bool isOnSlope { get; set; }
    public bool canWalkOnSlope { get; set; }
    private bool isFastOnPlatform = false;
    private bool isSlowOnPlatform = false;
    public int otherIsOnLitCount { get; private set; }
    public int highestOtherIsOnLitCount { get; private set; } = 0;
    public bool perfectLaunch { get; set; }
    public bool canRace { get; set; }
    #endregion

    #region Components
    public Runner runner;
    protected NetworkIdentity networkIdentity;
    protected LitPlatformHandler litHandler;
    public RacerDamages racerDamages;
    public RacerAwakening racerAwakening;
    public PerkHandler perkHandler;
    public CameraFollowNetwork cameraFollow;

    public RunnerFeedbacks runnerFeedbacks;
    public Animator Anim { get; private set; }
    // public GameManager GM { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public CapsuleCollider2D ThisCollider { get; private set; }
    public PlayerInputHandlerNetwork InputHandler { get; set; }
    public GameObject powerupManager { get; set; }
    public PowerupController powerupController { get; private set; }
    public GamePlayerLobby GamePlayer { get; set; }
    #endregion

    #region Check Variables   
    public Transform playerCenter;
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

    public int FacingDirection { get; private set; }
    #endregion

    #endregion

    #region Functions

    #region Initialization Functions
    public virtual void Awake()
    {
        StateMachine = new FiniteStateMachine();

        Anim = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
        ThisCollider = GetComponent<CapsuleCollider2D>();
        litHandler = GetComponent<LitPlatformHandler>();
        runner = new Runner(null, GetComponent<StickmanNet>(), null, this);
        PlayerEvents.OnBlackOverride += BlackOverrider_OnBlackOverride;

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

        isOnAnotherLit = false;
        overdrive = false;
        hasJustLeftLitplatform = false;
        colliderSize = ThisCollider.size;

        previousStrengthNormalized = strength / racerData.maxStrength;


        awakenedData = Resources.Load<AwakenedData>($"{runner.stickmanNet.code}/AwakenedData");
        powerupData = Resources.Load<PowerupData>($"{runner.stickmanNet.code}/PowerupData");
    }
    #endregion

    #region Update Functions
    [Client]
    public virtual void Update()
    {
        //  ChangeStatsWhileOnLit();
        ResetOverdrive();
        IncreaseSpeedWhenOtherIsOnLit();
        CapMoveResource();

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

        isOnPower = OnPowerPlatform();

        CurrentVelocity = RB.velocity;
    }
    public virtual void FixedUpdate()
    {
        if (!hasAuthority && currentRacerType == RacerType.Player) { return; }
        SlopeCheck();
        CheckIfOnAnotherLit();
        CheckHighestOtherOnLitCount();

        currentPosition = transform.position.x;

        #region Collision Effects

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
        normalizedStrength = strength / racerData.maxStrength;
        normalizedMovementVelocity = CurrentVelocity.x / racerData.topSpeed;

        if (normalizedMovementVelocity > 1)
        {
            jumpVelocity = 1f * racerData.maxJumpVelocity; // was 1.25f
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
    /// Function to accelerate the runner till he reaches the cap velocity.
    /// </summary>
    /// <param name="capVelocity">Value to limit moveVelocity to.</param>
    public virtual void SetAccelerations(float capVelocity)
    {
        movementVelocity += acceleration * Time.deltaTime;
        float knockbackAccelRate = acceleration;
        racerData.knockbackVelocity.x += knockbackAccelRate * Time.deltaTime;
        racerData.knockbackVelocity.y += knockbackAccelRate * Time.deltaTime;

        racerData.knockbackVelocity.x = Mathf.Min(racerData.knockbackVelocity.x, racerData.maxKnockbackVelocity.x);
        racerData.knockbackVelocity.y = Mathf.Min(racerData.knockbackVelocity.y, racerData.maxKnockbackVelocity.y);
        movementVelocity = Mathf.Min(movementVelocity, capVelocity);

    }
    /// <summary>
    /// Function to decelerate a runner, under given conditions, till he reaches the cap velocity.
    /// </summary>
    /// <param name="capVelocity">Value to limit moveVelocity to.</param>
    public virtual void SetDecelerations(float capVelocity)
    {
        movementVelocity += deceleration * Time.deltaTime;
        movementVelocity = Mathf.Max(movementVelocity, capVelocity);

    }
    /// <summary>
    /// Function to bring a runner to a halt faster than the decelerate function.
    /// </summary>
    public virtual void SetBrakes()
    {
        movementVelocity += brakeRatePerSec * Time.deltaTime;
        movementVelocity = Mathf.Max(movementVelocity, 0);
    }
    /// <summary>
    /// Function to bring a runner to a halt instantly.
    /// </summary>
    public virtual void SetStop()
    {
        movementVelocity = 0f;
    }
    private void CapMoveResource()
    {
        if (moveVelocityResource < 0)
            moveVelocityResource = Mathf.Max(0, moveVelocityResource);
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
                        PlayerEvents.instance.BlackOverride();
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
    public void PowerTriggerExit()
    {
        isOnPower = false;
    }
    #endregion

    #region Check Functions
    
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
        return Physics2D.OverlapCircle(GroundCheck.position, racerData.groundCheckRadius, racerData.whatIsGround);
    }
    private RaycastHit2D LitPlatformRaycast()
    {
        Debug.DrawRay(transform.position, Vector2.down * racerData.litCheckDistance, Color.green);
        return Physics2D.Raycast(litCheck.position, Vector2.down, racerData.litCheckDistance, racerData.whatIsLit);
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
        return Physics2D.Raycast(litCheck.position, Vector2.down, racerData.powerCheckDistance, racerData.whatIsPower);
    }
    public virtual PoweredPlatform PowerPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(litCheck.position, Vector2.down, racerData.powerCheckDistance, racerData.whatIsPower);
        PoweredPlatform poweredPlatform = null;
        if (hit.collider != null)
        {
            poweredPlatform = hit.collider.GetComponent<PoweredPlatform>();
        }

        return poweredPlatform;
    }
    public virtual void SlopeCheck()
    {
        Vector2 checkPos = transform.position - new Vector3(0f, colliderSize.y / 2, 0f);

        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }
    public virtual void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D hitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, racerData.whatIsSlope);
        RaycastHit2D hitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, racerData.whatIsSlope);

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
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, racerData.whatIsSlope);

        if (hit)
        {
            slopeNormalPerpendicular = Vector2.Perpendicular(hit.normal).normalized;
            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeDownAngle != slopeDownAngleOld && slopeDownAngle != 0)
                isOnSlope = true;


            slopeDownAngleOld = slopeDownAngle;



            Debug.DrawRay(hit.point, slopeNormalPerpendicular, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }

        if (slopeDownAngle > maxSlopeAngle)
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
        moveVelocityResource += Utils.PercentageValue(racerData.topSpeed, racerData.litSpeedUpPercentage);

        isFastOnPlatform = true;
        isSlowOnPlatform = false;
    }
    public virtual void SlowDown()
    {
        moveVelocityResource -= Utils.PercentageValue(racerData.topSpeed, racerData.litSlowDownPercentage);

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
        if (isSlowOnPlatform)
        {
            moveVelocityResource += Utils.PercentageValue(racerData.topSpeed, racerData.litSlowDownPercentage);
        }
        else if (isFastOnPlatform)
        {
            moveVelocityResource -= Utils.PercentageValue(racerData.topSpeed, racerData.litSpeedUpPercentage);
        }
        //   if(moveVelocityResource == newMoveVelocity)
        {
            ResetSpeedBools();
        }
        jumpVelocityResource = racerData.maxJumpVelocity;

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
        else
        {
            isOnAnotherLit = false;
        }
    }
    public virtual void IncreaseSpeedWhenOtherIsOnLit()
    {
        int othersOnLit = CheckOtherPlayersOnLit();

        if (othersOnLit != 0 && isOnLit && !overdrive)
        {
            if (othersOnLit >= otherIsOnLitCount)
            {
                otherIsOnLitCount = othersOnLit;
                moveVelocityResource += Utils.PercentageValue(racerData.topSpeed, racerData.overdriveIncreasePercentage * otherIsOnLitCount);
            }
            overdrive = true;
        }
        else if (othersOnLit != 0 && !isOnLit && !drive)
        {
            if (othersOnLit >= otherIsOnLitCount)
            {
                otherIsOnLitCount = othersOnLit;
                moveVelocityResource += Utils.PercentageValue(racerData.topSpeed, racerData.otherOnLitIncreasePercentage * otherIsOnLitCount);
            }
            drive = true;
        }
    }
    /// <summary>
    /// Used to change the stats of the player while on lit. Especially if the runner has already collided with the litplatform.
    /// </summary>
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
        int currentOtherOnLitCount = CheckOtherPlayersOnLit();
        if (overdrive)
        {
            if (!isOnLit || currentOtherOnLitCount != otherIsOnLitCount)
            {
                moveVelocityResource -= Utils.PercentageValue(racerData.topSpeed, racerData.overdriveIncreasePercentage * otherIsOnLitCount);
                otherIsOnLitCount = currentOtherOnLitCount;
                overdrive = false;
            }
        }
        else if (drive)
        {
            if (isOnLit || currentOtherOnLitCount != otherIsOnLitCount)
            {
                moveVelocityResource -= Utils.PercentageValue(racerData.topSpeed, racerData.otherOnLitIncreasePercentage * otherIsOnLitCount);
                otherIsOnLitCount = currentOtherOnLitCount;
                drive = false;
            }
        }
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

    #region Other Functions
    public enum RacerType
    {
        Player,
        Opponent
    }
    public virtual GameObject InstantiateObject(GameObject original, Vector3 position, Quaternion rotation, Transform parent = null)
    {
        GameObject entity = Instantiate(original, position, rotation, parent);
        return entity;
    }
    public virtual void DestroyObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }
    public void Revive()
    {
        StateMachine.ChangeDamagedState(opponentRevivedState);
        StateMachine.ChangeState(opponentMoveState);
    }
    public virtual void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, powerupData.mineExplosiveRadius);
        Gizmos.DrawWireSphere(GroundCheck.position, racerData.groundCheckRadius);
        Gizmos.DrawWireSphere(playerCenter.position, racerData.powerupSelfRadius);
        Gizmos.DrawLine(litCheck.position, litCheck.position + (Vector3)(Vector2.down * racerData.litCheckDistance));
        Gizmos.DrawLine(litCheck.position, litCheck.position + (Vector3)(Vector2.down * racerData.powerCheckDistance));
    }
    #endregion

    #endregion
}

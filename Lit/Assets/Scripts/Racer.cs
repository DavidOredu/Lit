﻿using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;

public class Racer : NetworkBehaviour
{
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
    public PlayerNetwork_QuickHaltState playerQuickHaltState { get; set; }
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
    public Enemy_SlideState opponentSlideState { get; set; }
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

    public List<State> opponentDamageStates;
    public Dictionary<Vector2, State> opponentDynamicDamageStates;
    #endregion

    public Dictionary<RacerType, List<State>> damageStates;
    public Dictionary<RacerType, Dictionary<Vector2, State>> dynamicDamageStates;
    // The type of racer the object is... is it a player, or an opponent
    public RacerType currentRacerType;
    // The player data and stats holder for the player object, in case its a player racer
    public PlayerData playerData { get; private set; }
    // The opponent data and stats holder for the opponent object, in case its an opponent racer
    public D_DifficultyData difficultyData { get; set; }


    public event Action OnLitPlatformChanged;

    protected NetworkIdentity networkIdentity;
    protected LitPlatformHandler litHandler;

    public bool[,] damageMatrix;
    #region State Variables
    // move variables related to the player 
    [Header("Move State")]
    public float moveVelocityResource;
    public float movementVelocity;
    protected float accelRatePerSec;
    protected float decelRatePerSec;
    protected float brakeRatePerSec;
    public float gravityScaleTemp { get; private set; }

    // jump variables 
    [Header("Jump State")]
    public float jumpVelocityResource;
    public float jumpVelocity;

    public bool isAffectedByLaser { get; set; }
    public bool isAffectedByDeathLaser { get; set; }
    public bool isOnAnotherLit { get; private set; }
    public bool isOnLit { get; private set; }
    public bool isStayingOnLit { get; private set; }
    public bool hasJustLeftLitplatform { get; private set; }
    public bool canSpeedUp { get; private set; }
    public bool isOnPower { get; private set; }
    public bool isInvulnerable { get; set; } = false;
    public bool overdrive { get; private set; }

    public Runner runner;
    #endregion
    public LitPlatformNetwork litPlatform { get; private set; }
    protected PoweredPlatform poweredPlatform;

    public Image testImage;

    public int otherIsOnLitCount { get; private set; }
    public int highestOtherIsOnLitCount { get; private set; } = 0;

    public Powerup powerup { get; set; } = null;
    public RunnerEffects runnerEffects;
    public VFXConnector VFXConnector;
    public DamageForm.DamagerType myDamagerType { get; set; }

    #region Components
    public Animator Anim { get; private set; }
    // public GameManager GM { get; private set; }
    public Rigidbody2D RB { get; private set; }
    public PlayerInputHandlerNetwork inputHandler { get; set; }
    #endregion
    #region Check Variables   
    public Transform GroundCheck;
    public Transform litCheck;
    public Transform projectileArm;

    public List<LitPlatformNetwork> litPlatforms = new List<LitPlatformNetwork>();
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

    public RunnerDamagesOperator myDamages;
    public virtual void Awake()
    {
        StateMachine = new FiniteStateMachine();

        if (playerData == null)
            playerData = Resources.Load<PlayerData>("PlayerData");
        Anim = GetComponent<Animator>();
        RB = GetComponent<Rigidbody2D>();
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
        canSpeedUp = false;
        hasJustLeftLitplatform = false;

        SetMyDamagerType();
        var runnerEffectComp = transform.Find("RunnerEffects").GetComponent<VisualEffect>();
        runnerEffects = new RunnerEffects(runnerEffectComp, myDamagerType);
        runnerEffects.runnerVFX.Stop();
    }

    #region Update Functions
    [Client]
    public virtual void Update()
    {
        ChangeStatsWhileOnLit();
        ResetOverdrive();
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
        CheckIfOnAnotherLit();
        CheckOtherPlayersOnLit();
        otherIsOnLitCount = CheckOtherPlayersOnLit();
        CheckHighestOtherOnLitCount();

        #region Collision Effects
        if (!isStayingOnLit && litPlatform != null && hasJustLeftLitplatform)
        {
            isOnLit = false;
            hasJustLeftLitplatform = false;
            StartCoroutine(Reseter());
        }

        #endregion

        currentPosition = transform.position.x;
    }
    public virtual void SetMyDamagerType()
    {
        switch (runner.stickmanNet.code)
        {
            case 1:
                myDamagerType = DamageForm.DamagerType.Fire;
                break;
            case 2:
                myDamagerType = DamageForm.DamagerType.Frost;
                break;
            case 3:
                myDamagerType = DamageForm.DamagerType.PoisonGas;
                break;
            case 4:
                myDamagerType = DamageForm.DamagerType.Light;
                break;
            case 5:
                myDamagerType = DamageForm.DamagerType.Lightning;
                break;
            case 6:
                myDamagerType = DamageForm.DamagerType.Wind;
                break;
            case 7:
                myDamagerType = DamageForm.DamagerType.Magic;
                break;
            case 0:
                myDamagerType = DamageForm.DamagerType.Shadow;
                break;
            default:
                myDamagerType = DamageForm.DamagerType.Laser;
                break;
        }
    }
    #endregion

    #region Set Functions
    public virtual void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, CurrentVelocity.y);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }
    public virtual void SetVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity);
        RB.velocity = workspace;
        CurrentVelocity = workspace;
    }
    public virtual void SetAccelerations(RacerType racerType)
    {
        switch (racerType)
        {
            case RacerType.Player:
                movementVelocity += accelRatePerSec * Time.deltaTime;
                jumpVelocity += accelRatePerSec * Time.deltaTime;
                float knockbackAccelRate = accelRatePerSec;
                playerData.knockbackVelocity.x += knockbackAccelRate * Time.deltaTime;
                playerData.knockbackVelocity.y += knockbackAccelRate * Time.deltaTime;

                playerData.knockbackVelocity.x = Mathf.Min(playerData.knockbackVelocity.x, playerData.maxKnockbackVelocity.x);
                playerData.knockbackVelocity.y = Mathf.Min(playerData.knockbackVelocity.y, playerData.maxKnockbackVelocity.y);
                movementVelocity = Mathf.Min(movementVelocity, moveVelocityResource);
                jumpVelocity = Mathf.Min(jumpVelocity, jumpVelocityResource);
                break;
            case RacerType.Opponent:
                movementVelocity += accelRatePerSec * Time.deltaTime;
                jumpVelocity += accelRatePerSec * Time.deltaTime;
                float knockbackAccelRateO = accelRatePerSec * 0.667f;
                difficultyData.knockbackVelocity.x += knockbackAccelRateO * Time.deltaTime;
                difficultyData.knockbackVelocity.y += knockbackAccelRateO * Time.deltaTime;

                difficultyData.knockbackVelocity.x = Mathf.Min(difficultyData.knockbackVelocity.x, difficultyData.maxKnockbackVelocity.x);
                difficultyData.knockbackVelocity.y = Mathf.Min(difficultyData.knockbackVelocity.y, difficultyData.maxKnockbackVelocity.y);
                movementVelocity = Mathf.Min(movementVelocity, moveVelocityResource);
                jumpVelocity = Mathf.Min(jumpVelocity, jumpVelocityResource);
                break;
            default:
                break;
        }



    }
    public virtual void SetDecelerations()
    {
        movementVelocity += decelRatePerSec * Time.deltaTime;

        movementVelocity = Mathf.Max(movementVelocity, 0);

    }
    public virtual void SetBrakes()
    {
        movementVelocity += brakeRatePerSec * Time.deltaTime;
        jumpVelocity = 0f;
        movementVelocity = Mathf.Max(movementVelocity, 0);
    }
    #endregion

    #region Collision Functions
    public virtual void OnCollisionEnter2D(Collision2D other)
    {
        if (OnLitPlatform())
        {
            if (other.collider.CompareTag("LitPlatform"))
            {


                if (!isOnLit)
                {
                    //   if (!hasAuthority) { return; }
                    litPlatform = other.gameObject.GetComponent<LitPlatformNetwork>();

                    litHandler.litPlatform = litPlatform;

                    networkIdentity = other.gameObject.GetComponent<NetworkIdentity>();
                    if (!litPlatform.isLit || runner.stickmanNet.currentColor.colorID == 0)
                    {
                        if (GetComponent<NetworkIdentity>().hasAuthority || currentRacerType == RacerType.Opponent)
                        {

                            litHandler.UpdateColor();
                            Debug.Log("Trying to run Update Color Logic!");
                            litPlatform.colorStateCode.colorID = runner.stickmanNet.currentColor.colorID;
                        }


                    }

                    if (!litPlatforms.Contains(litPlatform) && litPlatform.colorStateCode.colorID == runner.stickmanNet.currentColor.colorID)
                    {
                        litPlatforms.Add(litPlatform);

                    }
                    if (runner.stickmanNet.currentColor.colorID == 0)
                    {
                        BlackOverrider.instance.Override();
                    }

                    isOnLit = true;

                    WaitToChangeStats(litPlatform);
                }

            }
        }

        if (other.collider.CompareTag("PowerPlatform"))
        {
            isOnPower = true;
            //     testImage.gameObject.SetActive(true); 
            poweredPlatform = other.gameObject.GetComponent<PoweredPlatform>();

        }

    }
    public virtual void OnCollisionStay2D(Collision2D other)
    {
        if (other.collider.CompareTag("LitPlatform"))
        {
            other.gameObject.GetComponent<LitPlatformNetwork>();
        }
    }
    public virtual void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.CompareTag("LitPlatform"))
        {
            hasJustLeftLitplatform = true;
            canSpeedUp = true;
        }
    }
    #endregion

    #region Check Functions
    public virtual void WaitToChangeStats(LitPlatformNetwork litPlatform)
    {
        if (runner.stickmanNet.currentColor.colorID == litPlatform.colorStateCode.colorID && litPlatform.isLit)
        {
            SpeedUp();

        }
        else
        {
            SlowDown();
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
    public RaycastHit2D LitPlatformRaycast()
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
    public virtual IEnumerator Reseter()
    {
        switch (currentRacerType)
        {
            case RacerType.Player:
                yield return new WaitForSeconds(playerData.ResetSpeedTime);
                ResetStats();
                canSpeedUp = false;
                break;
            case RacerType.Opponent:
                yield return new WaitForSeconds(difficultyData.resetSpeedTime);
                ResetStats();
                canSpeedUp = false;
                break;
            default:
                break;
        }
    }


    #endregion

    #region LitPlatform Functions
    public virtual void SpeedUp()
    {
        switch (currentRacerType)
        {
            case RacerType.Player:
                moveVelocityResource = playerData.topSpeed;
                jumpVelocityResource = playerData.maxJumpVelocity;
                jumpVelocityResource += playerData.jumpAddition;
                moveVelocityResource += playerData.litSpeedUpLimit;
                break;
            case RacerType.Opponent:
                moveVelocityResource = difficultyData.topSpeed;
                jumpVelocityResource = difficultyData.maxJumpVelocity;
                jumpVelocityResource += difficultyData.jumpAddition;
                moveVelocityResource += difficultyData.litSpeedUpLimit;
                break;
            default:
                break;
        }
    }
    public virtual void SlowDown()
    {
        switch (currentRacerType)
        {
            case RacerType.Player:
                moveVelocityResource = playerData.topSpeed;
                jumpVelocityResource = playerData.maxJumpVelocity;
                moveVelocityResource -= playerData.litSlowDownLimit;
                break;
            case RacerType.Opponent:
                moveVelocityResource = difficultyData.topSpeed;
                jumpVelocityResource = difficultyData.maxJumpVelocity;
                moveVelocityResource -= difficultyData.litSlowDownLimit;
                break;
            default:
                break;
        }
    }
    public virtual void ResetStats()
    {
        switch (currentRacerType)
        {
            case RacerType.Player:
                moveVelocityResource = playerData.topSpeed;
                jumpVelocityResource = playerData.maxJumpVelocity;
                break;
            case RacerType.Opponent:
                moveVelocityResource = difficultyData.topSpeed;
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
        else
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
    public virtual void DestroyObject(GameObject gameObject)
    {
        Destroy(gameObject);
    }

    public enum RacerType
    {
        Player,
        Opponent
    }
    #region Damage Functions
    public virtual void DamageRunner(RunnerDamagesOperator runnerDamages)
    {
        //check if the runner is previously damaged, and if so do damage again, this time going to a special damage state
        if (myDamages.IsDamaged())
        {
            foreach (var damage in runnerDamages.DamageList())
            {
                DoDynamicDamage(runnerDamages, damage);
                Debug.Log("Do Dynamic Damage has run!");
            }
        }
        else
        {
            foreach (var damage in runnerDamages.DamageList())
            {
                // change to the appropirate damage state
                ChangeToDamageState(runnerDamages, damage, damageStates[currentRacerType][runnerDamages.Damages.IndexOf(damage)]);
            }
        }
    }
    public virtual void ChangeToDamageState(RunnerDamagesOperator runnerDamages, DamageForm damage, State stateToChangeTo)
    {
        var index = runnerDamages.Damages.IndexOf(damage);
        var newDamage = myDamages.Damages[index];
        newDamage.damaged = true;
        newDamage.damageTime = damage.damageTime;

        if(damage.negativeGravityScale != 0f)
        newDamage.negativeGravityScale = damage.negativeGravityScale;
        
        StateMachine.ChangeState(stateToChangeTo);
        StopCoroutine(RecoverRunner());
        StartCoroutine(RecoverRunner());
    }
    public virtual IEnumerator RecoverRunner()
    {
        float totalDamageTime = 0f;
        foreach (var myDamage in myDamages.DamageList())
        {
            totalDamageTime += myDamage.damageTime;
        }
        Debug.Log($"Damage count is: {myDamages.DamageList().Count}");
        yield return new WaitForSeconds(totalDamageTime);

        if (myDamages.IsDamaged())
        {
            foreach (var myDamage in myDamages.DamageList())
            {
                //     var index = myDamages.Damages.IndexOf(damage);
                myDamage.damaged = false;
                myDamage.negativeGravityScale = 0f;
            }
            switch (currentRacerType)
            {
                case RacerType.Player:
                    StateMachine.ChangeState(playerStunState);
                    break;
                case RacerType.Opponent:
                    StateMachine.ChangeState(opponentStunState);
                    break;
                default:
                    break;
            }
        }
    }
    public virtual void DoDynamicDamage(RunnerDamagesOperator runnerDamages, DamageForm damage)
    {
        if (!myDamages.SimilarDamages(runnerDamages))
        {
            var index = runnerDamages.Damages.IndexOf(damage);
            var myNewDamage = myDamages.Damages[index];
            myNewDamage.damaged = true;
            myNewDamage.damageTime = damage.damageTime;
            if (damage.negativeGravityScale != 0f)
                myNewDamage.negativeGravityScale = damage.negativeGravityScale;

            foreach (var previousDamage in myDamages.DamageList())
            {
                foreach (var newDamage in runnerDamages.DamageList())
                {
                    DynamicDamage(runnerDamages, previousDamage, newDamage);
                    Debug.Log("Dynamic Damge has run!");
                }
            }
        }
        else
        {
            Debug.Log("There are similar damages!");
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
                    StopCoroutine(RecoverRunner());
                    StartCoroutine(RecoverRunner());
                    StateMachine.ChangeState(stateToChangeTo);
                   
                }
            }
        }
    }
    void DynamicDamage(RunnerDamagesOperator runnerDamages, DamageForm previousDamage, DamageForm newDamage)
    {
        // get the to damage index to identify the respective damages
        var previousDamageIndex = myDamages.Damages.IndexOf(previousDamage);
        var newDamageIndex = runnerDamages.Damages.IndexOf(newDamage);

        // create a matrix to determine the ralationship between the damages
        bool[,] damageMatrix = new bool[10, 10];

        damageMatrix[previousDamageIndex, newDamageIndex] = true;

        CheckBoolArray(damageMatrix, dynamicDamageStates[currentRacerType][new Vector2(previousDamageIndex, newDamageIndex)]);
    }
    public virtual GameObject InstantiateObject(GameObject original, Vector3 position, Quaternion rotation, Transform parent) 
    {
        GameObject entity = Instantiate(original, position, rotation, parent);
        return entity;
    }
    #endregion
    public virtual void OnDrawGizmos()
    {
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
}

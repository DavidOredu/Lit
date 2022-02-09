using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
public class Player : Racer
{
    #region State Variables

    #endregion

    #region Components
    public CinemachineVirtualCamera vCam;
    //  public SoundManager soundManager { get; private set; }
    //  public HealthBar healthBar { get; private set; }

    #endregion
    #region Check Variables


    // public GameObject dustPS;


    #endregion

    public override void Awake()
    {
        base.Awake();

        playerIdleState = new PlayerNetwork_IdleState(null, StateMachine, "idle", this, playerData, null);
        playerMoveState = new PlayerNetwork_MoveState(null, StateMachine, "move", this, playerData, null);
        playerJumpState = new PlayerNetwork_JumpState(null, StateMachine, "inAir", this, playerData, null);
        playerInAirState = new PlayerNetwork_InAirState(null, StateMachine, "inAir", this, playerData, null);
        playerLandState = new PlayerNetwork_LandState(null, StateMachine, "land", this, playerData, null);
        playerKnockbackState = new PlayerNetwork_KnockbackState(null, StateMachine, "knockback", this, playerData, null);
        playerSlideState = new PlayerNetwork_SlideState(null, StateMachine, "slide", this, playerData, null);
        playerActionState = new PlayerNetwork_ActionState(null, StateMachine, "action", this, playerData, null);
        playerQuickHaltState = new PlayerNetwork_QuickHaltState(null, StateMachine, "quickHalt", this, playerData, null);
        playerFullStopState = new PlayerNetwork_FullStopState(null, StateMachine, "fullStop", this, playerData, null);
        playerShadowedState = new PlayerNetwork_ShadowedState(null, StateMachine, "shadowed", this, playerData, null);
        playerBurningState = new PlayerNetwork_BurningState(null, StateMachine, "burning", this, playerData, null);
        playerFrozenState = new PlayerNetwork_FrozenState(null, StateMachine, "frozen", this, playerData, null);
        playerChokingState = new PlayerNetwork_ChokingState(null, StateMachine, "choking", this, playerData, null);
        playerBlindedState = new PlayerNetwork_BlindedState(null, StateMachine, "blinded", this, playerData, null);
        playerElectrocutedState = new PlayerNetwork_ElectrocutedState(null, StateMachine, "electrocuted", this, playerData, null);
        playerBlownState = new PlayerNetwork_BlownState(null, StateMachine, "blown", this, playerData, null);
        playerCursedState = new PlayerNetwork_CursedState(null, StateMachine, "cursed", this, playerData, null);
        playerLazeredState = new PlayerNetwork_LazeredState(null, StateMachine, "lazered", this, playerData, null);
        playerStunState = new PlayerNetwork_StunState(null, StateMachine, "stun", this, playerData, null);
        playerDeadState = new PlayerNetwork_DeadState(null, StateMachine, "dead", this, playerData, null);
        playerDamageKnockDownState = new PlayerNetwork_DamageKnockDownState(null, StateMachine, "damageKnockDown", this, playerData, null);
        playerRevivedState = new PlayerNetwork_RevivedState(null, StateMachine, "revived", this, playerData, null);
        playerAwakenedState = new PlayerNetwork_AwakenedState(null, StateMachine, "awakened", this, playerData, null);
        playerHoverGlideState = new PlayerNetwork_HoverGlideState(null, StateMachine, "hoverGlide", this, playerData, null);
        playerSlideGlideState = new PlayerNetwork_SlideGlideState(null, StateMachine, "slideGlide", this, playerData, null);
        playerNullState = new PlayerNetwork_NullState(null, StateMachine, "null", this, playerData, null);

        playerDamageStates = new List<State>
        {
            // shadow-affected state
        /* 0 */   playerShadowedState,
            // burning state
        /* 1 */   playerBurningState,
            // frozen state
        /* 2 */   playerFrozenState,
            // poisoned state
        /* 3 */   playerChokingState,
            // blinded state
        /* 4 */   playerBlindedState,
            // electrocuted state
        /* 5 */   playerElectrocutedState,
            // blown state
        /* 6 */   playerBlownState,
            // cursed state
        /* 7 */    playerCursedState,
            // lazered state
        /* 8 */   playerLazeredState,
            // death state
        /* 9 */   playerDeadState,
            // knockout state
        /* 10 */ playerKnockbackState,
        };

        acceleration = playerData.topSpeed / playerData.timeZeroToMax;
        deceleration = -playerData.topSpeed / playerData.timeMaxToZero;
        brakeRatePerSec = -playerData.topSpeed / playerData.timeBrakeToZero;
        movementVelocity = 0f;
        playerData.knockbackVelocity = Vector2.zero;

        moveVelocityResource = playerData.topSpeed;
        strengthResource = playerData.maxStrength;
        jumpVelocityResource = playerData.maxJumpVelocity;
        jumpVelocity = jumpVelocityResource;
        strength = playerData.maxStrength;
    }





    [Client]
    public override void Start()
    {
        base.Start();

        StateMachine.Initialize(playerMoveState);
        StateMachine.InitializeDamage(playerNullState);
        StateMachine.InitializeAwakened(playerNullState);
    }


    [Client]
    public override void Update()
    {
        StateMachine.CurrentState.LogicUpdate();
        StateMachine.DamagedState.LogicUpdate();
        StateMachine.AwakenedState.LogicUpdate();
        base.Update();

        Debug.Log(StateMachine.CurrentState);
        Debug.Log(runner.stickmanNet.currentColor.colorID);
    }
    [Client]
    public override void LateUpdate()
    {
        StateMachine.CurrentState.LateUpdate();
        StateMachine.DamagedState.LateUpdate();
        StateMachine.AwakenedState.LateUpdate();
        base.LateUpdate();
    }
    [Client]
    public override void FixedUpdate()
    {
        if (InputHandler == null)
            InputHandler = GetComponent<PlayerInputHandlerNetwork>();

        StateMachine.CurrentState.PhysicsUpdate();
        StateMachine.DamagedState.PhysicsUpdate();
        StateMachine.AwakenedState.PhysicsUpdate();
        base.FixedUpdate();

        if (isOnPower && StateMachine.CurrentState != playerKnockbackState && StateMachine.CurrentState != playerSlideState && poweredPlatform != null && hasAuthority)
        {
            //    testImage.gameObject.SetActive(true);
            if (InputHandler.JumpInput)
            {
                poweredPlatform.DefinePower(this);
            }

        }
        if (InputHandler.PowerupInput)
        {
            GamePlayer.powerupButton.UsePowerup(false);
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


    #region Check Functions

    public override void OnCollisionEnter2D(Collision2D other)
    {
        StateMachine.CurrentState.OnCollisionEnter(other);
        StateMachine.DamagedState.OnCollisionEnter(other);
        StateMachine.AwakenedState.OnCollisionEnter(other);
        base.OnCollisionEnter2D(other);


        if (other.collider.CompareTag("PowerPlatform"))
        {
            poweredPlatform = other.gameObject.GetComponent<PoweredPlatform>();
        }
    }
    public override void SlopeCheckVertical(Vector2 checkPos)
    {
        base.SlopeCheckVertical(checkPos);

        if (isOnSlope)
        {
            
        }
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
    }



    #endregion

    #region PlaySound Functions
    private void PlayFootstepSound()
    {
        //  soundManager.PlaySound("Walk");

    }

    private void PlayJumpScoff()
    {
        //  soundManager.PlaySound("JumpScoff");
    }

    #endregion
}

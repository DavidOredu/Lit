using System.Collections.Generic;
using UnityEngine;

public class Opponent : Entity
{
   
    public override void Awake()
    {
        base.Awake();

        SetAIDifficulty();
        SetAllVariables();

        opponentMoveState = new Enemy_MoveState(this, StateMachine, "move", this, null, difficultyData);
        opponentIdleState = new Enemy_IdleState(this, StateMachine, "idle", this, null, difficultyData);
        opponentInAirState = new Enemy_InAirState(this, StateMachine, "inAir", this, null, difficultyData);
        opponentLandState = new Enemy_LandState(this, StateMachine, "land", this, null, difficultyData);
        opponentJumpState = new Enemy_JumpState(this, StateMachine, "inAir", this, null, difficultyData);
        opponentKnockbackState = new Enemy_KnockbackState(this, StateMachine, "knockback", this, null, difficultyData);
        opponentSlideState = new Enemy_SlideState(this, StateMachine, "slide", this, null, difficultyData);
        opponentActionState = new Enemy_ActionState(this, StateMachine, "action", this, null, difficultyData);
        opponentQuickHaltState = new Enemy_QuickHaltState(this, StateMachine, "quickHalt", this, null, difficultyData);
        opponentFullStopState = new Enemy_FullStopState(this, StateMachine, "fullStop", this, null, difficultyData);
        opponentShadowedState = new Enemy_ShadowedState(this, StateMachine, "shadowed", this, null, difficultyData);
        opponentBurningState = new Enemy_BurningState(this, StateMachine, "burning", this, null, difficultyData);
        opponentFrozenState = new Enemy_FrozenState(this, StateMachine, "frozen", this, null, difficultyData);
        opponentChokingState = new Enemy_ChokingState(this, StateMachine, "choking", this, null, difficultyData);
        opponentBlindedState = new Enemy_BlindedState(this, StateMachine, "blinded", this, null, difficultyData);
        opponentElectrocutedState = new Enemy_ElectrocutedState(this, StateMachine, "electrocuted", this, null, difficultyData);
        opponentBlownState = new Enemy_BlownState(this, StateMachine, "blown", this, null, difficultyData);
        opponentCursedState = new Enemy_CursedState(this, StateMachine, "cursed", this, null, difficultyData);
        opponentLazeredState = new Enemy_LazeredState(this, StateMachine, "lazered", this, null, difficultyData);
        opponentStunState = new Enemy_StunState(this, StateMachine, "stun", this, null, difficultyData);
        opponentDeadState = new Enemy_DeadState(this, StateMachine, "dead", this, null, difficultyData);

        opponentDamageStates = new List<State>
        {
            // shadow-affected state
        /* 0 */   opponentShadowedState,
            // burning state
        /* 1 */   opponentBurningState,
            // frozen state
        /* 2 */   opponentFrozenState,
            // poisoned state
        /* 3 */   opponentChokingState,
            // blinded state
        /* 4 */   opponentBlindedState,
            // electrocuted state
        /* 5 */   opponentElectrocutedState,
            // blown state
        /* 6 */   opponentBlownState,
            // cursed state
        /* 7 */   opponentCursedState,
            // lazered state
        /* 8 */   opponentLazeredState,
            // death state
        /* 9 */   opponentDeadState,
            // knockout state
        /* 10 */ playerKnockbackState,
        };

        movementVelocity = 0f;
        accelRatePerSec = difficultyData.topSpeed / difficultyData.timeZeroToMax;
        decelRatePerSec = -difficultyData.topSpeed / difficultyData.timeMaxToZero;
        brakeRatePerSec = -difficultyData.topSpeed / difficultyData.timeBrakeToZero;

        moveVelocityResource = difficultyData.topSpeed;
        jumpVelocityResource = difficultyData.maxJumpVelocity;
    }
    public override void Start()
    {
        base.Start();

        

        StateMachine.Initialize(opponentMoveState);


    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (isOnPower && StateMachine.CurrentState != opponentKnockbackState && StateMachine.CurrentState != opponentSlideState && poweredPlatform != null && CheckIfGrounded())
        {
            // some check should be put here to determine difficulty to know the rate at which the ai should respond
            {
                poweredPlatform.DefinePower(this);
            }

        }
        if (FinishLine.hasCrossedLine)
        {
            //        stateMachine.ChangeState(quickHaltState);
            //       FinishLine.hasCrossedLine = false;
        }
    }


    public override void LateUpdate()
    {
        base.LateUpdate();

        Debug.Log($"Current enemy state is:  {StateMachine.CurrentState}");
    }


    // Update is called once per frame

    public override void Update()
    {
        base.Update();
    }

    public override void OnCollisionEnter2D(Collision2D other)
    {
        base.OnCollisionEnter2D(other);

        if (other.collider.CompareTag("Wall") && StateMachine.CurrentState != opponentSlideState)
        {
            StateMachine.ChangeState(opponentKnockbackState);
        }
    }
}

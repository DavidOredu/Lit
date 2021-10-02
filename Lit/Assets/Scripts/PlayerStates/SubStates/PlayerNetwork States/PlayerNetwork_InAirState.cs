using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork_InAirState : State
{
    public bool isGrounded { get; protected set; }
    protected int XInput;
    protected float variableJumpMultiplierNormalized;
    protected bool jumpInputStop;
    protected bool jumpInput;
    protected bool coyoteTime;
    protected bool isJumping;

    public PlayerNetwork_InAirState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
    {
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void AnimationTrigger()
    {
        base.AnimationTrigger();
    }

    public override void DoChecks()
    {
        base.DoChecks();

        isGrounded = racer.CheckIfGrounded();
    }

    public override void Enter()
    {
        base.Enter();
      
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        XInput = racer.InputHandler.NormalizedInputX;
        variableJumpMultiplierNormalized = racer.InputHandler.inputHoldTimeNormalized;
        if (racer.GamePlayer.powerup != null)
        {
            if (!racer.GamePlayer.powerup.isSelected)
            {
                jumpInput = racer.InputHandler.JumpInput;
                jumpInputStop = racer.InputHandler.JumpInputStop;
            }
            else
            {
                jumpInput = false;
                jumpInputStop = true;
            }
        }
        else
        {
            jumpInput = racer.InputHandler.JumpInput;
            jumpInputStop = racer.InputHandler.JumpInputStop;
        }

        
        if (!isGrounded)
        {
            //player.SetDecelerations();
            //player.SetVelocityX(player.movementVelocity);


            racer.Anim.SetFloat("yVelocity", racer.CurrentVelocity.y);
            racer.Anim.SetFloat("xVelocity", Mathf.Abs(racer.CurrentVelocity.x));
        }
    }

    public void CheckCoyoteTIme()
    {
        if(coyoteTime && Time.time > startTime + playerData.coyoteTime)
        {
            coyoteTime = false;
            racer.playerJumpState.DecreaseAmountOfJumpsLeft();
        }
    }

    public void StartCoyoteTime()
    {
        coyoteTime = true;
    }

    public void SetIsJumping()
    {
        isJumping = true;
    }


    public override void LateUpdate()
    {
        base.LateUpdate();
        CheckCoyoteTIme();
        CheckJumpMultiplier();

        if (isGrounded && racer.CurrentVelocity.y < 0.01f)
        {
            StateMachine.ChangeState(racer.playerLandState);
        }
        else if (jumpInput && racer.playerJumpState.CanJump() && !racer.isOnPower && racer.hasAuthority)
        {
            StateMachine.ChangeState(racer.playerJumpState);
        }
        
    }
    public void CheckJumpMultiplier()
    {
        if (isJumping)
        {
            if (jumpInputStop)
            {
                racer.SetVelocityY(racer.CurrentVelocity.y * playerData.variableJumpHeightMultiplier);
                isJumping = false;
            }
            else if (racer.CurrentVelocity.y <= 0f)
            {
                isJumping = false;
            }
        }
    }
}

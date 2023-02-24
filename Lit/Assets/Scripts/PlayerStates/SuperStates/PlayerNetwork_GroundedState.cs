using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork_GroundedState : State
{
    protected bool attackInput;
    protected bool isGrounded;


    protected int XInput;
    protected bool jumpInput;

    public PlayerNetwork_GroundedState(Entity entity, FiniteStateMachine StateMachine, string animBoolName, Racer racer, PlayerData playerData = null, D_DifficultyData difficultyData = null) : base(entity, StateMachine, animBoolName, racer, playerData, difficultyData)
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
        
        racer.playerJumpState.ResetAmountOfJumpLeft();
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LateUpdate()
    {
        base.LateUpdate();

        Debug.Log("Jump input is " + jumpInput);
        if (jumpInput && racer.playerJumpState.CanJump() && !racer.isOnPower && racer.hasAuthority)
        {
            jumpInput = false;
            racer.InputHandler.UseJumpInput();
            StateMachine.ChangeState(racer.playerJumpState);
        }
      
        else if (!isGrounded)
        {
            racer.playerInAirState.StartCoyoteTime();
            StateMachine.ChangeState(racer.playerInAirState);
        }

        
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
        
     //   player.landState.SpawnDust();
    }


    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        XInput = racer.InputHandler.NormalizedInputX;
        if (racer.GamePlayer.powerup != null)
        {
            if (!racer.GamePlayer.powerup.isSelected)
                jumpInput = racer.InputHandler.JumpInput;
            else
                jumpInput = false;
        }
        else
        {
            jumpInput = racer.InputHandler.JumpInput;
        }
        
        attackInput = racer.InputHandler.AttackInput;
    }
   
}
